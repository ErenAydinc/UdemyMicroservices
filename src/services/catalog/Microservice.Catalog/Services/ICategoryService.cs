using Microservice.Catalog.Dtos;
using Microservice.Catalog.Models;
using Microservice.Shared.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microservice.Catalog.Services
{
    public interface ICategoryService
    {
        Task<Response<List<CategoryDto>>> GetAllAsync();
        Task<Response<CategoryDto>> CreateAsync(CategoryDto categoryDto);
        Task<Response<CategoryDto>> GetByIdAsync(string id);
    }
}
