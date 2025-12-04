
using Ecom.BLL.ModelVM.FaceId;
using Ecom.BLL.Service.Abstraction;
using FaceRecognitionDotNet;

namespace Ecom.BLL.Service.Implementation
{
    public class FaceIdService : IFaceIdService
    {
        private readonly IFaceIdRepo _faceRepo;
        private readonly FaceRecognition _faceRecognition;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;

        public FaceIdService(IFaceIdRepo faceRepo, FaceRecognition faceRecognition, IMapper mapper, ITokenService tokenService)
        {
            _faceRepo = faceRepo;
            _faceRecognition = faceRecognition;
            _mapper = mapper;
            _tokenService = tokenService;
        }

        // Helper method to extract face encoding from IFormFile
        private async Task<double[]?> ExtractEncoding(IFormFile file)
        {
            try
            {
                // 1) Save IFormFile to a temporary file
                var tempFilePath = Path.GetTempFileName();

                // Copy IFormFile to temp file
                await using (var stream = new FileStream(tempFilePath, FileMode.Create))
                {
                    // Copy the file content to the stream
                    await file.CopyToAsync(stream);
                }

                // 2) Load image using FaceRecognitionDotNet
                using var img = FaceRecognition.LoadImageFile(tempFilePath);

                // 3) Detect face locations and compute encodings
                // Assuming only one face per image for registration/login
                // If multiple faces are detected, we take the first one
                // If no faces are detected, return null
                var locations = _faceRecognition.FaceLocations(img).ToList();
                if (!locations.Any())
                    return null;

                // Compute face encodings for detected locations 
                // and take the first encoding
                // Each encoding is a 128-d vector
                // If multiple faces, we only consider the first one
                var enc = _faceRecognition.FaceEncodings(img, locations).FirstOrDefault();

                // 4) Clean up temporary file
                File.Delete(tempFilePath);

                // 5) Return encoding as double array of 128 elements
                // If no encoding found, return null
                return enc?.GetRawEncoding();
            }
            catch
            {
                return null;
            }
        }

        // Helper method to calculate Euclidean distance between two encodings
        // Smaller distance means more similar
        // Used for face verification
        private static double CalculateDistance(double[] a, double[] b)
        {
            // Ensure both arrays are non-null and of same length
            // If not, return a large distance
            if (a == null || b == null || a.Length != b.Length)
                return double.MaxValue;

            // Calculate Euclidean distance
            double sum = 0;
            for (int i = 0; i < a.Length; i++)
            {
                var diff = a[i] - b[i];
                sum += diff * diff;
            }

            // Return square root of sum of squared differences
            // This gives the Euclidean distance
            // Smaller values indicate more similar encodings
            return Math.Sqrt(sum);
        }

        // Register a new face
        public async Task<ResponseResult<FaceId>> RegisterFaceAsync(RegisterFaceIdVM model)
        {
            try
            {
                // 1) Extract encoding from uploaded image file 
                // and assign to model
                // If no face detected, return error response
                var encoding = await ExtractEncoding(model.imageFile);
                if (encoding != null)
                {
                    model.Encoding = encoding;
                }
                else
                {
                    return new ResponseResult<FaceId>(null, "No face detected in the image", false);
                }

                // 2) Map ViewModel to Entity
                var faceId = _mapper.Map<FaceId>(model);

                // 3) Save FaceId entity to database
                // Return success or failure response accordingly
                var isAdded = await _faceRepo.AddAsync(faceId);

                if (isAdded)
                {
                    return new ResponseResult<FaceId>(faceId, "Face registered successfully", true);
                }
                return new ResponseResult<FaceId>(null, "Failed to register face", false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        // Update face encoding for existing FaceId
        public async Task<ResponseResult<bool>> UpdateFaceAsync(UpdateFaceIdVM model)
        {
            try
            {
                // 1) Extract encoding from uploaded image file
                // If no face detected, return error response
                var encoding = await ExtractEncoding(model.imageFile);
                if (encoding == null)
                    return new ResponseResult<bool>(false, "No face detected in the image", false);

                // 2) Update model with new encoding
                model.Encoding = encoding;

                // 3) Map ViewModel to Entity
                var face = _mapper.Map<FaceId>(model);

                // 4) Update FaceId entity in database
                var isUpdated = await _faceRepo.UpdateAsync(face);

                // Return success or failure response accordingly
                if (isUpdated)
                {
                    return new ResponseResult<bool>(true, "Face updated successfully", true);
                }
                return new ResponseResult<bool>(false, "Failed to update face", false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        // Verify face by userId - retrieve all FaceIds for a user
        public async Task<ResponseResult<IEnumerable<FaceId>>> VerifyFaceByUserIdAsync(string userId)
        {
            try
            {
                // 1) Retrieve FaceIds from database by userId
                var faces = await _faceRepo.GetByUserIdAsync(userId);

                // 2) Return success or failure response accordingly
                if (!faces.Any())
                    return new ResponseResult<IEnumerable<FaceId>>(Enumerable.Empty<FaceId>(), "No faces found for this user", false);

                return new ResponseResult<IEnumerable<FaceId>>(faces, "Faces retrieved successfully", true);
            }
            catch (Exception)
            {
                throw;
            }
        }

        // Verify face for login
        public async Task<ResponseResult<AuthResponseVM>> VerifyFaceLoginAsync(IFormFile image)
        {
            try
            {
                // 1) Extract encoding from uploaded image
                // If no face detected, return error response
                var encoding = await ExtractEncoding(image);
                if (encoding == null)
                    return new ResponseResult<AuthResponseVM>(null, "No face detected", false);

                // 2) Load all users with their faces
                var users = await _faceRepo.GetAllUsersWithFacesAsync();

                // 3) If no users with faces, return error response
                if (users == null || !users.Any())
                    return new ResponseResult<AuthResponseVM>(null, "No users with faces in database", false);

                // 4) Compare extracted encoding with each stored encoding
                // and find the best match
                AppUser? bestUser = null;

                // Variable to track best (smallest) distance found
                double bestDistance = double.MaxValue;

                // 5) Iterate through each user and their face encodings
                foreach (var user in users)
                {
                    // Skip users with no face encodings
                    if (user.FaceIds == null || !user.FaceIds.Any())
                        continue;

                    // Compare with each face encoding of the user
                    foreach (var savedFace in user.FaceIds)
                    {
                        // Get stored encoding as double array
                        var dbEnc = savedFace.GetEncodingAsDouble();

                        // Calculate distance between the two encodings
                        // Smaller distance means more similar
                        var distance = CalculateDistance(encoding, dbEnc);

                        // Update best match if this distance is smaller
                        // Note: In face recognition, we typically look for the smallest distance
                        // to find the best match
                        // So we check if the current distance is less than the best found so far
                        // and update accordingly
                        // This is different from traditional matching where higher similarity scores are better
                        // Here, lower distance indicates better match
                        if (distance < bestDistance)
                        {
                            bestDistance = distance;
                            bestUser = user;
                        }
                    }
                }

                // 6) Determine if the best match is good enough based on a threshold
                // If best distance is below threshold, consider it a match
                // Otherwise, login fails
                // Note: The threshold value may need to be tuned based on the specific application
                // and the face recognition model used
                // A common threshold for face recognition distances is around 0.6
                // Values lower than this indicate a good match
                // Typical values are between 0.4 to 0.6
                const double threshold = 0.6;

                // 7) Return success response with JWT if matched, else failure response
                if (bestUser != null && bestDistance < threshold)
                {
                    // Generate JWT token for the authenticated user
                    // Return success response with token
                    var token = await _tokenService.CreateToken(bestUser); // Generate JWT token
                    var userVM = _mapper.Map<GetUserVM>(bestUser); // Map AppUser to GetUserVM
                    var authResponse = new AuthResponseVM // Create AuthresponseVM
                    {
                        User = userVM,
                        Token = token,
                        TokenExpiration = DateTime.UtcNow.AddDays(7)
                    };
                    return new ResponseResult<AuthResponseVM>(authResponse, $"Login success (distance = {bestDistance:F4})", true);
                }

                // No good match found
                return new ResponseResult<AuthResponseVM>(null, $"Face not recognized (best distance = {bestDistance:F4})", false);
            }
            catch
            {
                throw;
            }
        }

        // Delete face by userId
        public async Task<ResponseResult<bool>> DeleteFaceAsync(string userId)
        {
            try
            {
                // 1) Validate userId
                // If null or empty, return error response
                if (string.IsNullOrEmpty(userId))
                    return new ResponseResult<bool>(false, "User not authenticated", false);

                // 2) Delete FaceId(s) from database by userId
                var deleted = await _faceRepo.DeleteAsync(userId);

                // 3) Return success or failure response accordingly
                if (deleted)
                {
                    return new ResponseResult<bool>(true, "Face deleted successfully", true);
                }
                return new ResponseResult<bool>(false, "FaceId not found", false);
            }
            catch
            {
                throw;
            }
        }


        
    }
        
}

