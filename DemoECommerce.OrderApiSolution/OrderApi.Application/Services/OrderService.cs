using System.IO.Pipelines;
using System.Net.Http.Json;
using OrderApi.Application.DTO;
using OrderApi.Application.DTO.Conversions;
using OrderApi.Application.Interfaces;
using Polly;
using Polly.Registry;

namespace OrderApi.Application.Services
{
    public class OrderService(IOrder orderInterface, HttpClient httpClient, ResiliencePipelineProvider<string> resiliencePipeline) : IOrderServices
    {
        // get product
        public async Task<ProductDTO> GetProduct(int productId)
        {
            // call product api using httpClient
            // redirect this call to the api gateway since product api is behind the gateway

            var getProduct = await httpClient.GetAsync($"api/products/{productId}");

            if (!getProduct.IsSuccessStatusCode)
                return null!;

            var product = await getProduct.Content.ReadFromJsonAsync<ProductDTO>();
            return product!;
        }

        // get user 
        public async Task<AppUserDTO> GetUser(int userId)
        {
            // call product api using httpClient
            // redirect this call to the api gateway since product api is behind the gateway
            var getUser = await httpClient.GetAsync($"/api/authentication/{userId}");

            if (!getUser.IsSuccessStatusCode)
                return null!;

            var user = await getUser.Content.ReadFromJsonAsync<AppUserDTO>();
            return user!;
        }

        // get order details
        public async Task<OrderDetailsDTO> GetOrderDetails(int orderId)
        {
            // prepare order
            var order = await orderInterface.FindByIdAsync(orderId);

            if (order is null || order.Id <= 0)
                return null!;

            // get retry pipeline
            var retryPipeline = resiliencePipeline.GetPipeline("RetryPolicy-pipeline");

            // prepare product 
            var productDTO = await retryPipeline.ExecuteAsync(async token => await GetProduct(order.ProductId));

            // prepare client
            var appUserDTO = await retryPipeline.ExecuteAsync(async token => await GetUser(order.ClientId));

            // populate order details 
            return new OrderDetailsDTO(
                order.Id,
                productDTO.Id,
                appUserDTO.Id,
                appUserDTO.Name,
                appUserDTO.Email,
                appUserDTO.Address,
                appUserDTO.TelephoneNumber,
                productDTO.Name,
                order.PurchaseQuantity,
                productDTO.Price,
                productDTO.Quantity * order.PurchaseQuantity,
                order.OrderDate
            );
        }

        // get orders by client id
        public async Task<IEnumerable<OrderDTO>> GetOrdersByClientId(int clientId)
        {
            // get all client's orders
            var orders = await orderInterface.GetOrdersAsync(o => o.ClientId == clientId);
            if (orders.Any())
                return null!;

            // convert from entity to dto
            var (_, _orders) = OrderConversion.FromEntity(null, orders);
            return _orders!;
        }
    }
}