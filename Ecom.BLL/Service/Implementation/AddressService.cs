using Ecom.BLL.ModelVM.Address;
using Ecom.BLL.Service.Abstraction;
using Ecom.DAL.Entity;
using Ecom.DAL.Repo.Abstraction;

namespace Ecom.BLL.Service.Implementation
{
    public class AddressService : IAddressService
    {
        private readonly IAddressRepo _addressRepo;
        private readonly IMapper _mapper;

        public AddressService(IAddressRepo addressRepo, IMapper mapper)
        {
            _addressRepo = addressRepo;
            _mapper = mapper;
        }

        // Get
        public async Task<ResponseResult<GetAddressVM>> GetByIdAsync(int id)
        {
            try
            {
                var address = await _addressRepo.GetByIdAsync(id, includes: a => a.AppUser);

                if (address == null)
                    return new ResponseResult<GetAddressVM>(null,
                        $"Address with ID {id} not found.", false);

                var mappedAddress = _mapper.Map<GetAddressVM>(address);
                return new ResponseResult<GetAddressVM>(mappedAddress, null, true);
            }
            catch (Exception ex)
            {
                return new ResponseResult<GetAddressVM>(null, ex.Message, false);
            }
        }

        public async Task<ResponseResult<IEnumerable<GetAddressVM>>> GetAllAsync(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var addresses = await _addressRepo.GetAllAsync(
                    filter: a => !a.IsDeleted,
                    pageSize: pageSize,
                    pageNumber: pageNumber,
                    includes: a => a.AppUser);

                if (addresses == null || !addresses.Any())
                    return new ResponseResult<IEnumerable<GetAddressVM>>(null,
                        "No addresses found.", false);

                var mappedAddresses = _mapper.Map<IEnumerable<GetAddressVM>>(addresses);
                return new ResponseResult<IEnumerable<GetAddressVM>>(mappedAddresses, null, true);
            }
            catch (Exception ex)
            {
                return new ResponseResult<IEnumerable<GetAddressVM>>(null, ex.Message, false);
            }
        }

        public async Task<ResponseResult<IEnumerable<GetAddressVM>>> GetAllByUserIdAsync(string userId,
            int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var addresses = await _addressRepo.GetAllByUserIdAsync(
                    userId: userId,
                    filter: a => !a.IsDeleted,
                    pageSize: pageSize,
                    pageNumber: pageNumber);

                if (addresses == null || !addresses.Any())
                    return new ResponseResult<IEnumerable<GetAddressVM>>(null,
                        "No addresses found.", false);

                var mappedAddresses = _mapper.Map<IEnumerable<GetAddressVM>>(addresses);
                return new ResponseResult<IEnumerable<GetAddressVM>>(mappedAddresses, null, true);
            }
            catch (Exception ex)
            {
                return new ResponseResult<IEnumerable<GetAddressVM>>(null, ex.Message, false);
            }
        }

        // Create
        public async Task<ResponseResult<bool>> CreateAsync(CreateAddressVM model)
        {
            try
            {
                // 1- Map VM -> Entity
                var newAddress = _mapper.Map<Address>(model);

                // 2- Call the repo to add the new address
                var result = await _addressRepo.AddAsync(newAddress);

                // 3- Return the response
                if (result)
                {
                    return new ResponseResult<bool>(true, null, true);
                }
                return new ResponseResult<bool>(false, "Failed to save address to the database.", false);
            }
            catch (Exception ex)
            {
                return new ResponseResult<bool>(false, ex.Message, false);
            }
        }

        // Update
        public async Task<ResponseResult<bool>> UpdateAsync(UpdateAddressVM model)
        {
            try
            {
                // 1- Get the tracked entity from the repo
                var oldAddress = await _addressRepo.GetByIdAsync(model.Id);
                if (oldAddress == null)
                {
                    return new ResponseResult<bool>(false, "Address not found.", false);
                }

                // 2- Map VM -> Entity
                var address = _mapper.Map<Address>(model);

                // 3- Call the repo to update the employee
                var result = await _addressRepo.UpdateAsync(address); // Use the new repo method

                // 4- Return the response
                if (result)
                {
                    return new ResponseResult<bool>(true, null, true);
                }
                return new ResponseResult<bool>(false, "Failed to update address in the database.", false);
            }
            catch (Exception ex)
            {
                return new ResponseResult<bool>(false, ex.Message, false);
            }
        }

        // Delete
        public async Task<ResponseResult<bool>> DeleteAsync(DeleteAddressVM model)
        {
            try
            {
                // Get the tracked entity
                var addressToDelete = await _addressRepo.GetByIdAsync(model.Id);
                if (addressToDelete == null)
                {
                    return new ResponseResult<bool>(false, "Address not found.", false);
                }

                // Delete the employee using the repo
                bool result = await _addressRepo.ToggleDeleteStatusAsync(model.Id, model.DeletedBy); // Soft delete
                // bool result = await _addressRepo.DeleteAsync(model.Id); // Hard delete

                if (result)
                {
                    return new ResponseResult<bool>(true, null, true);
                }

                return new ResponseResult<bool>(false, "Failed to toggle delete status.", false);
            }
            catch (Exception ex)
            {
                return new ResponseResult<bool>(false, ex.Message, false);
            }
        }

        // Get Delete Model
        public async Task<ResponseResult<DeleteAddressVM>> GetDeleteModelAsync(int id)
        {
            try
            {
                var address = await _addressRepo.GetByIdAsync(id);
                if (address == null)
                    return new ResponseResult<DeleteAddressVM>(null,
                        $"Address with ID {id} not found.", false);

                var mappedAddress = _mapper.Map<DeleteAddressVM>(address);
                return new ResponseResult<DeleteAddressVM>(mappedAddress, null, true);
            }
            catch (Exception ex)
            {
                return new ResponseResult<DeleteAddressVM>(null, ex.Message, false);
            }
        }
    }
}
