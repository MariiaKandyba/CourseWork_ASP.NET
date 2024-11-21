using DTOs.Orders;
using DTOs.Products;
using Microsoft.EntityFrameworkCore;
using OrderServiceApi.Data;
using OrderServiceApi.Models;
using System.Net.Http;

namespace OrderServiceApi.Services
{
    public class OrderService : IOrderService
    {
        private readonly OrderDbContext _context;

        public OrderService(OrderDbContext context)
        {
            _context = context;
        }

        public async Task<OrderDto> CreateOrder(int userId, List<OrderItemDto> items, AddressDto deliveryAddress)
        {
            var existingAddress = await _context.Addresses
                .FirstOrDefaultAsync(a => a.Street == deliveryAddress.Street &&
                                          a.City == deliveryAddress.City &&
                                          a.ZipCode == deliveryAddress.ZipCode &&
                                          a.Country == deliveryAddress.Country);

            Address addressToUse;

            if (existingAddress != null)
            {
                addressToUse = existingAddress;
            }
            else
            {
                addressToUse = new Address()
                {
                    Country = deliveryAddress.Country,
                    City = deliveryAddress.City,
                    Street = deliveryAddress.Street,
                    ZipCode = deliveryAddress.ZipCode
                };

                _context.Add(addressToUse);
                await _context.SaveChangesAsync();
            }

            var order = new Order
            {
                IdUser = userId,
                OrderItems = items.Select(item => new OrderItem
                {
                    IdProduct = item.ProductId,
                    Quantity = item.Quantity,
                }).ToList(),
                CreatedAt = DateTime.UtcNow,
                Status = OrderStatus.Pending,
                Address = addressToUse 
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return MapToDto(order);
        }



        public async Task<OrderDto> GetOrderByIdAsync(int orderId)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .Include(o => o.Address)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            return MapToDto(order);
        }

        public async Task<List<OrderDto>> GetOrdersByUserIdAsync(int userId)
        {
            var orders = await _context.Orders
                .Where(o => o.IdUser == userId)
                .Include(o => o.OrderItems)
                .Include(o => o.Address)
                .ToListAsync();

            return orders.Select(MapToDto).ToList();
        }

        public async Task<List<OrderDto>> GetAllOrdersAsync()
        {
            var orders = await _context.Orders
                .Include(o => o.OrderItems)
                .Include(o => o.Address)
                .ToListAsync();

            return orders.Select(MapToDto).ToList();
        }
        public async Task<bool> UpdateOrderStatusAsync(int orderId, string newStatus)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
            {
                return false;
            }

            if (Enum.TryParse<OrderStatus>(newStatus, out var status))
            {
                order.Status = status;
                _context.Orders.Update(order);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }

      


        private OrderDto MapToDto(Order order)
        {
            if (order == null) return null;

            return new OrderDto
            {
                Id = order.Id,
                UserId = order.IdUser,
                Items = order.OrderItems.Select(item => new OrderItemDto
                {
                    Id = item.Id,
                    ProductId = item.IdProduct,
                    Quantity = item.Quantity,
                }).ToList(),
                CreatedAt = order.CreatedAt,
                Status = order.Status.ToString(),
                DeliveryAddress = new AddressDto
                {
                    Street = order.Address.Street,
                    City = order.Address.City,
                    ZipCode = order.Address.ZipCode,
                    Country = order.Address.Country
                }
            };
        }

       
    }
}
