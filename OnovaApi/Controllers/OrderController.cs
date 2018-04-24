using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnovaApi.Data;
using OnovaApi.DTOs;
using OnovaApi.Helpers;
using OnovaApi.Models.DatabaseModels;
using OnovaApi.Services;

namespace OnovaApi.Controllers
{
    [Produces("application/json")]
    [Route("api/Order")]
    public class OrderController : Controller
    {
        private readonly OnovaContext _context;
        private readonly IEmailSender _emailSender;

        public OrderController(OnovaContext context, IEmailSender emailSender)
        {
            _context = context;
            _emailSender = emailSender;
        }

        // GET: api/Order
        [HttpGet]
        public IEnumerable<Order> GetOrder()
        {
            return _context.Order;
        }

        [HttpGet]
        [Route("ShowOrdersForUser")]
        [Authorize]
        public IActionResult ShowOrdersForUser()
        {
            var currentCustomerId = User.Identities.FirstOrDefault(u => u.IsAuthenticated)
                ?.FindFirst(
                    c => c.Type == JwtRegisteredClaimNames.NameId || c.Type == ClaimTypes.NameIdentifier)
                ?.Value;

            var orders = _context.Order.Where(o => o.CartId == currentCustomerId).Select(o => new
            {
                o.OrderDate,
                o.OrderTrackingNumber,
                o.DisplayPrice,
                o.AddressLine1,
                o.FullName,
                o.Phone,
                o.City,
                o.Zip,
                o.OrderStatus.Name,
                items = o.OrderDetail.Select(d => new
                {
                    d.ProductId,
                    d.DisplayPrice,
                    d.Product.Name,
                    d.Product.Slug,
                    d.Product.ProductThumbImage
                })
            }).OrderByDescending(o => o.OrderDate).ToList();

            return Json(new
            {
                _context.Users.Find(currentCustomerId).FullName,
                NumberOrders = orders.Count,
                orders
            });
        }

        // GET: api/Order/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrder([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var order = await _context.Order.SingleOrDefaultAsync(m => m.OrderId == id);

            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }

        // PUT: api/Order/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder([FromRoute] int id, [FromBody] Order order)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != order.OrderId)
            {
                return BadRequest();
            }

            _context.Entry(order).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Order
        [HttpPost]
        [Route("CreateOrder")]
        public async Task<IActionResult> CreateOrder([FromBody] OrderDTO orderDto)
        {
            if (User.Identity.IsAuthenticated)
            {
                var cart = _context.CustomerCart.Find(orderDto.CartId);
                var shippingInfo = _context.ShippingInfo.Where(c => c.CustomerId == orderDto.CartId)
                    .FirstOrDefault(c => c.IsDefault);
                var email = _context.Users.Find(orderDto.CartId).Email;
//                var currentCustomerId = User.Identities.FirstOrDefault(u => u.IsAuthenticated)
//                    ?.FindFirst(
//                        c => c.Type == JwtRegisteredClaimNames.Email || c.Type == ClaimTypes.Email)
//                    ?.Value;

                if (cart != null)
                {
                    var cartItems = _context.CustomerCartDetail.Where(c => c.CustomerCartId == orderDto.CartId)
                        .ToList();

                    using (var transaction = await _context.Database.BeginTransactionAsync())
                    {
                        foreach (var item in cartItems)
                        {
                            var product = _context.Product.Find(item.ProductId);
                            if (item.Quantity > product.CurrentQuantity)
                            {
                                return Json(new
                                {
                                    Status = "Failed",
                                    Message = "Current product quantity is less than quantity in cart"
                                });
                            }
                        }


                        var order = new Order
                        {
                            CartId = orderDto.CartId,
                            DisplayPrice = orderDto.TotalPrice,
                            ShippingFee = cart.DisplayPrice > 1000 ? 0 : 30,
                            AddressLine1 = shippingInfo.AddressLine1,
                            AddressLine2 = shippingInfo.AddressLine2,
                            City = shippingInfo.City,
                            OrderDate = DateTime.Now,
                            EstimateShippingDate = DateTime.Now.AddDays(3),
                            FullName = shippingInfo.FullName,
                            InvoiceTokenId = orderDto.TokenId,
                            OrderTrackingNumber = Extensions.GenerateString(),
                            OrderStatusId = 1,
                            PaymentTokenId = orderDto.TokenId,
                            Zip = shippingInfo.Zip,
                            Tax = cart.Tax,
                            Phone = shippingInfo.Phone,
                            TotalPrice = cart.TotalPrice,
                            TotalQuantity = cart.TotalQuantity,
                            PaymentStatus = "Paid",
                            PriceDiscount = cart.PriceDiscount,
                            PromotionId = cart.PromotionId
                        };

                        _context.Order.Add(order);
                        await _context.SaveChangesAsync();

                        var orderItems = new List<OrderDetail>();

                        foreach (var item in cartItems)
                        {
                            orderItems.Add(new OrderDetail
                            {
                                ProductId = item.ProductId,
                                DisplayPrice = item.DisplayPrice,
                                OrderId = order.OrderId,
                                Price = item.Price,
                                PriceDiscount = item.PriceDiscount,
                                PromotionId = item.PromotionId,
                                Quantity = item.Quantity
                            });

                            var product = _context.Product.Find(item.ProductId);
                            product.CurrentQuantity = product.CurrentQuantity - item.Quantity.GetValueOrDefault();
                            _context.Product.Update(product);
                        }

                        _context.OrderDetail.AddRange(orderItems);
                        await _context.SaveChangesAsync();

                        _context.CustomerCartDetail.RemoveRange(cartItems);

                        if (await _context.SaveChangesAsync() > 0)
                        {
                            _context.CustomerCart.Remove(cart);

                            if (await _context.SaveChangesAsync() > 0)
                            {
                                _context.CustomerCart.Add(new CustomerCart
                                {
                                    CustomerCartId = orderDto.CartId,
                                    CreateDate = DateTime.Now
                                });

                                if (await _context.SaveChangesAsync() > 0)
                                {
                                    transaction.Commit();
                                    var tax = cart.Tax * (cart.DisplayPrice + order.ShippingFee - order.PriceDiscount);

                                    await _emailSender.SendEmailOrderSuccessfulAsync(order.OrderTrackingNumber,
                                        cart.DisplayPrice,
                                        order.PriceDiscount, order.ShippingFee, tax, order.TotalPrice, order.FullName,
                                        order.Phone, order.AddressLine1, order.City, order.Zip, email);

                                    return Json(new
                                    {
                                        Status = "Success",
                                        Message = "Order successful",
                                        OrderCode = order.OrderTrackingNumber
                                    });
                                }
                            }
                        }

                        return Json(new
                        {
                            Status = "Failed",
                            Message = "Cannot add order"
                        });
                    }
                }

                return Json(new
                {
                    Status = "Failed",
                    Message = "Cannot find cart"
                });
            }

            var anonymousCart = _context.AnonymousCustomerCart.Find(orderDto.CartId);

            if (anonymousCart != null)
            {
                var cartItems = _context.AnonymousCustomerCartDetail
                    .Where(c => c.AnonymousCustomerCartId == orderDto.CartId).ToList();

                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    foreach (var item in cartItems)
                    {
                        var product = _context.Product.Find(item.ProductId);
                        if (item.Quantity > product.CurrentQuantity)
                        {
                            return Json(new
                            {
                                Status = "Failed",
                                Message = "Current product quantity is less than quantity in cart"
                            });
                        }
                    }


                    var order = new Order
                    {
                        CartId = orderDto.CartId,
                        DisplayPrice = orderDto.TotalPrice,
                        ShippingFee = anonymousCart.DisplayPrice > 1000 ? 0 : 30,
                        AddressLine1 = orderDto.AddressLine1,
                        AddressLine2 = orderDto.AddressLine2,
                        City = orderDto.City,
                        OrderDate = DateTime.Now,
                        EstimateShippingDate = DateTime.Now.AddDays(3),
                        FullName = orderDto.FullName,
                        InvoiceTokenId = orderDto.TokenId,
                        OrderTrackingNumber = Extensions.GenerateString(),
                        OrderStatusId = 1,
                        PaymentTokenId = orderDto.TokenId,
                        Zip = orderDto.Zip,
                        Tax = anonymousCart.Tax,
                        Phone = orderDto.Phone,
                        TotalPrice = anonymousCart.TotalPrice,
                        TotalQuantity = anonymousCart.TotalQuantity,
                        PaymentStatus = "Paid",
                        PriceDiscount = anonymousCart.PriceDiscount,
                        PromotionId = anonymousCart.PromotionId
                    };

                    _context.Order.Add(order);
                    await _context.SaveChangesAsync();


                    var orderItems = new List<OrderDetail>();

                    foreach (var item in cartItems)
                    {
                        orderItems.Add(new OrderDetail
                        {
                            ProductId = item.ProductId,
                            DisplayPrice = item.DisplayPrice,
                            OrderId = order.OrderId,
                            Price = item.Price,
                            PriceDiscount = item.PriceDiscount,
                            PromotionId = item.PromotionId,
                            Quantity = item.Quantity
                        });

                        var product = _context.Product.Find(item.ProductId);
                        product.CurrentQuantity = product.CurrentQuantity - item.Quantity.GetValueOrDefault();
                        _context.Product.Update(product);
                    }

                    _context.OrderDetail.AddRange(orderItems);
                    await _context.SaveChangesAsync();

                    _context.AnonymousCustomerCartDetail.RemoveRange(cartItems);

                    if (await _context.SaveChangesAsync() > 0)
                    {
                        _context.AnonymousCustomerCart.Remove(anonymousCart);

                        if (await _context.SaveChangesAsync() > 0)
                        {
                            _context.AnonymousCustomerCart.Add(new AnonymousCustomerCart
                            {
                                AnonymousCustomerCartId = orderDto.CartId,
                                CreateDate = DateTime.Now
                            });

                            if (await _context.SaveChangesAsync() > 0)
                            {
                                transaction.Commit();
                                var tax = anonymousCart.Tax *
                                          (anonymousCart.DisplayPrice + order.ShippingFee - order.PriceDiscount);

                                await _emailSender.SendEmailOrderSuccessfulAsync(order.OrderTrackingNumber,
                                    anonymousCart.DisplayPrice,
                                    order.PriceDiscount, order.ShippingFee, tax, order.TotalPrice, order.FullName,
                                    order.Phone, order.AddressLine1, order.City, order.Zip, orderDto.Email);

                                return Json(new
                                {
                                    Status = "Success",
                                    Message = "Order successful",
                                    OrderCode = order.OrderTrackingNumber
                                });
                            }
                        }
                    }
                }

                return Json(new
                {
                    Status = "Failed",
                    Message = "Cannot add order"
                });
            }

            return Json(new
            {
                Status = "Failed",
                Message = "Cannot find cart"
            });
        }

        // DELETE: api/Order/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var order = await _context.Order.SingleOrDefaultAsync(m => m.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }

            _context.Order.Remove(order);
            await _context.SaveChangesAsync();

            return Ok(order);
        }

        private bool OrderExists(int id)
        {
            return _context.Order.Any(e => e.OrderId == id);
        }
    }
}