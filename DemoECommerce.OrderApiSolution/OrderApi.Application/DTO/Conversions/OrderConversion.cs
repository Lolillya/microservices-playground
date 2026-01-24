using OrderApi.Domain.Entities;

namespace OrderApi.Application.DTO.Conversions
{
    public static class OrderConversion
    {
        public static Order ToEntity(OrderDTO order) => new()
        {
            Id = order.Id,
            ClientId = order.ClientId,
            ProductId = order.ProductId,
            OrderDate = order.OrderDate,
            Price = order.Price,
            PurchaseQuantity = order.PurchaseQuantity
        };

        public static (OrderDTO?, IEnumerable<OrderDTO>?) FromEntity(Order? order, IEnumerable<Order>? orders)
        {
            // return single
            if (order is not null || orders is null)
            {
                var singleOrder = new OrderDTO
                (
                    order!.Id,
                    order.ProductId,
                    order.ClientId,
                    order.PurchaseQuantity,
                    order.Price,
                    order.OrderDate
                );

                return (singleOrder, null);
            }

            // return list
            if (orders is not null && order is null)
            {
                var _orders = orders!.Select(o =>
                    new OrderDTO
                    (
                        o.Id,
                        o.ProductId,
                        o.ClientId,
                        o.PurchaseQuantity,
                        o.Price,
                        o.OrderDate
                    )
                );

                return (null, _orders);
            }

            return (null, null);
        }
    }
}