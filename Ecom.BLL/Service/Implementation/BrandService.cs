using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ecom.BLL.ModelVM.Brand;
using Ecom.BLL.Service.Abstraction;
using Ecom.DAL.Entity;
using Ecom.DAL.Repo.Abstraction;

namespace Ecom.BLL.Service.Implementation
{
    public class BrandService : IBrandService
    {
        private readonly IBrandRepo _brandRepo;
        private readonly IMapper _mapper;

        public BrandService(IBrandRepo brandRepo, IMapper mapper)
        {
            _brandRepo = brandRepo;
            _mapper = mapper;
        }

        public async Task<ResponseResult<IEnumerable<GetBrandVM>>> GetAllAsync(bool includeDeleted = false)
        {
            try
            {
                var brands = await _brandRepo.GetAllAsync(b => includeDeleted || !b.IsDeleted);
                var result = _mapper.Map<IEnumerable<GetBrandVM>>(brands);
                return new ResponseResult<IEnumerable<GetBrandVM>>(result, null, true);
            }
            catch (Exception ex)
            {
                return new ResponseResult<IEnumerable<GetBrandVM>>(null, ex.Message, false);
            }
        }

        public async Task<ResponseResult<GetBrandVM>> GetByIdAsync(int id)
        {
            try
            {
                var brand = await _brandRepo.GetByIdAsync(id);
                if (brand == null)
                    return new ResponseResult<GetBrandVM>(null, "Brand not found.", false);

                var vm = _mapper.Map<GetBrandVM>(brand);
                return new ResponseResult<GetBrandVM>(vm, null, true);
            }
            catch (Exception ex)
            {
                return new ResponseResult<GetBrandVM>(null, ex.Message, false);
            }
        }

        public async Task<ResponseResult<bool>> CreateAsync(CreateBrandVM model)
        {
            try
            {
                var brand = new Brand(model.Name, model.ImageUrl, model.CreatedBy);
                await _brandRepo.AddAsync(brand);
                await _brandRepo.SaveChangesAsync();
                return new ResponseResult<bool>(true, null, true);
            }
            catch (Exception ex)
            {
                return new ResponseResult<bool>(false, ex.Message, false);
            }
        }

        public async Task<ResponseResult<bool>> UpdateAsync(UpdateBrandVM model)
        {
            try
            {
                var brand = await _brandRepo.GetByIdAsync(model.Id);
                if (brand == null)
                    return new ResponseResult<bool>(false, "Brand not found.", false);

                if (!brand.Update(model.Name, model.ImageUrl, model.UpdatedBy))
                    return new ResponseResult<bool>(false, "Invalid update data.", false);

                await _brandRepo.UpdateAsync(brand);
                await _brandRepo.SaveChangesAsync();
                return new ResponseResult<bool>(true, null, true);
            }
            catch (Exception ex)
            {
                return new ResponseResult<bool>(false, ex.Message, false);
            }
        }

        public async Task<ResponseResult<bool>> DeleteAsync(DeleteBrandVM model)
        {
            try
            {
                var brand = await _brandRepo.GetByIdAsync(model.Id);
                if (brand == null)
                    return new ResponseResult<bool>(false, "Brand not found.", false);

                await _brandRepo.DeleteAsync(model.Id, model.DeletedBy);
                await _brandRepo.SaveChangesAsync();
                return new ResponseResult<bool>(true, null, true);
            }
            catch (Exception ex)
            {
                return new ResponseResult<bool>(false, ex.Message, false);
            }
        }

        public async Task<ResponseResult<DeleteBrandVM>> GetDeleteModelAsync(int id)
        {
            try
            {
                var brand = await _brandRepo.GetByIdAsync(id);
                if (brand == null)
                    return new ResponseResult<DeleteBrandVM>(null, "Brand not found.", false);

                var vm = new DeleteBrandVM
                {
                    Id = brand.Id,
                    DeletedBy = brand.DeletedBy ?? "System"
                };

                return new ResponseResult<DeleteBrandVM>(vm, null, true);
            }
            catch (Exception ex)
            {
                return new ResponseResult<DeleteBrandVM>(null, ex.Message, false);
            }
        }
    }

}
