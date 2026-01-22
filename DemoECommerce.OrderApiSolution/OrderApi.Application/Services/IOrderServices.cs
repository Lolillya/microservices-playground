using OrderApi.Application.DTO;

namespace OrderApi.Application.Services
{
    public interface IOrderServices
    {
        Task<IEnumerable<OrderDTO>> GetOrdersByClientId(int clientId);
        Task<OrderDetailsDTO> GetOrderDetails(int OrderId);
    }
}