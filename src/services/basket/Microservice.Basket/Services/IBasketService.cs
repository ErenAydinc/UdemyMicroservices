using Microservice.Basket.Dtos;
using Microservice.Shared.Dtos;
using System.Threading.Tasks;

namespace Microservice.Basket.Services
{
    public interface IBasketService
    {
        Task<Response<BasketDto>> GetBasket(string userId);
        Task<Response<bool>> SaveOrUpdate(BasketDto basketDto);
        Task<Response<bool>> Delete(string userId);
    }
}
