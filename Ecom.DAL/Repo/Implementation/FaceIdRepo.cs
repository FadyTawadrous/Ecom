

namespace Ecom.DAL.Repo.Implementation
{
    public class FaceIdRepo : IFaceIdRepo
    {
        private readonly ApplicationDbContext _db;
        public FaceIdRepo(ApplicationDbContext db)
        {
            _db = db;
        }

        // Add a new FaceId
        public async Task<bool> AddAsync(FaceId face)
        {
            try
            {
                // Validate input
                if (face != null)
                {
                    var result = await _db.FaceIds.AddAsync(face);
                    await _db.SaveChangesAsync();
                    return result.Entity.Id > 0;
                }
                return false;
            }
            catch (Exception)
            {

                throw;
            }
        }

        // Delete FaceId by Id
        public async Task<bool> DeleteAsync(string userId)
        {
            try
            {
                if(!string.IsNullOrEmpty(userId))
                {
                    var faces = _db.FaceIds.Where(f => f.AppUserId == userId);
                    _db.FaceIds.RemoveRange(faces);
                    await _db.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {

                throw;
            }
        }

        // Get FaceId by Id
        public async Task<FaceId?> GetByIdAsync(int id)
        {
            try
            {
                if(id > 0)
                {
                    IQueryable<FaceId> query = _db.FaceIds.Where(f => f.Id == id);
                    var face =  await query.FirstOrDefaultAsync();
                    if (face != null)
                    {
                        return face;
                    }
                    return null;
                }
                return null;
            }
            catch (Exception)
            {

                throw;
            }
        }

        // Get FaceIds by UserId
        public async Task<IEnumerable<FaceId>> GetByUserIdAsync(string userId)
        {
            try
            {
                IQueryable<FaceId> query = _db.FaceIds.Where(f => f.AppUserId == userId);
                
                return await query.ToListAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        // Get all users with FaceIds
        public async Task<List<AppUser>> GetAllUsersWithFacesAsync()
        {
            try
            {
                var usersWithFaces = await _db.Users
                    //.Where(u => u.FaceIds.Any())
                    .Include(u => u.FaceIds)
                    .ToListAsync();
                return usersWithFaces;
            }
            catch (Exception)
            {
                throw;
            }
        }

        // Update FaceId
        public async Task<bool> UpdateAsync(FaceId newFace)
        {
            try
            {
                if (newFace == null)
                {
                    return false;
                }
                var oldFace = await _db.FaceIds.FirstOrDefaultAsync(c => c.Id == newFace.Id);
                if (oldFace == null)
                {
                    return false;
                }
                bool result = oldFace.Update(newFace.GetEncodingAsDouble(), newFace.UpdatedBy);
                if (result)
                {
                    await _db.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
