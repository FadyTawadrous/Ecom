
using Ecom.BLL.ModelVM.Category;
using Ecom.DAL.Entity;

namespace Ecom.BLL.AutoMapper
{
    public class DomainProfile : Profile
    {
        public DomainProfile()
        {
            // ----------------------------------------
            // ## Category Mappings
            // ----------------------------------------
            // Category <-> CreateCategoryVM
            CreateMap<Category, AddCategoryVM>().ReverseMap();
            // Category <-> UpdateCategoryVM
            CreateMap<Category, UpdateCategoryVM>().ReverseMap();
            // Category <-> GetCategoryVM
            CreateMap<Category, GetCategoryVM>().ReverseMap();
            // Category <-> DeleteCategoryVM
            CreateMap<Category, DeleteCategoryVM>().ReverseMap();
            // ----------------------------------------
            // ## End Category Mappings
            // ----------------------------------------

           

        }

    }
}
