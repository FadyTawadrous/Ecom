
using Ecom.BLL.ModelVM.Cart;

namespace Ecom.BLL.Service.Abstraction
{
    public interface ICartService
    {
        Task<ResponseResult<GetCartVM>> GetByUserIdAsync(string id);
        Task<ResponseResult<bool>> AddAsync(AddCartVM model);
        Task<ResponseResult<bool>> UpdateAsync(UpdateCartVM model);
        Task<ResponseResult<bool>> DeleteAsync(DeleteCartVM model);
        Task<ResponseResult<bool>> HardDeleteAsync(DeleteCartVM model);
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.BLL.Service.Abstraction
{
    internal class ICartService
    {
    }
}
