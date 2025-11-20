global using System.ComponentModel.DataAnnotations;
global using System.IdentityModel.Tokens.Jwt;
global using System.Security.Claims;
global using System.Text;
global using System.Data;
global using System.Linq.Expressions;

global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.AspNetCore.Http;
global using Microsoft.AspNetCore.Identity;
global using Microsoft.AspNetCore.Authentication.JwtBearer;
global using Microsoft.Extensions.Configuration;
global using Microsoft.IdentityModel.Tokens;
global using Microsoft.EntityFrameworkCore;

global using AutoMapper;

global using Ecom.BLL.Helper;
global using Ecom.BLL.Mapper;
global using Ecom.BLL.Responses;
global using Ecom.BLL.ModelVM.Account;
global using Ecom.BLL.ModelVM.ProductImageURL;
global using Ecom.BLL.ModelVM.Brand;
global using Ecom.BLL.Service.Abstraction;
global using Ecom.BLL.Service.Implementation;
global using Ecom.DAL.Entity;
global using Ecom.DAL.Repo.Abstraction;
global using Ecom.BLL.ModelVM.Role;
global using Ecom.BLL.ModelVM.Cart;
global using Ecom.BLL.ModelVM.CartItem;
global using Ecom.BLL.ModelVM.Category;
global using Ecom.BLL.ModelVM.Payment;
global using Ecom.BLL.ModelVM.Address;
global using Ecom.BLL.ModelVM.WishlistItem;