using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using OnovaApi.Data;
using OnovaApi.Models.DatabaseModels;

namespace OnovaApi.Controllers
{
    [Produces("application/json")]
    [Route("api/Cart")]
    public class CartController : Controller
    {
        private readonly OnovaContext _context;

        public CartController(OnovaContext context)
        {
            _context = context;
        }

        [Route("AddToCart")]
        [HttpGet]
        public async Task<IActionResult> AddToCart([FromQuery] string customerId, [FromQuery] int productId, [FromQuery] int quantity = 1)
        {
            var product = _context.Product.Find(productId);
//            var customerId = Request.Cookies["AnonymousId"];

            if (product == null)
            {
                return BadRequest();
            }

            if (User.Identity.IsAuthenticated)
            {
                var userId = User.Identities.FirstOrDefault(u => u.IsAuthenticated)
                    ?.FindFirst(c => c.Type == JwtRegisteredClaimNames.NameId)?.Value;

                _context.CustomerCartDetail.Add(new CustomerCartDetail
                {
                    CustomerCartId = userId,
                    ProductId = productId,
                    Quantity = quantity,
                    DisplayPrice = product.DisplayPrice,
                    Price = product.RealPrice,
                });

                await _context.SaveChangesAsync();

                return Json(new
                {
                    Status = "Success",
                    Message = "Add product into cart successful"
                });
            }
            else
            {
                if (string.IsNullOrEmpty(customerId))
                {
                    customerId = Guid.NewGuid().ToString();
                    Set("AnonymousId", customerId);

                    _context.AnonymousCustomer.Add(
                        new AnonymousCustomer {AnonymousCustomerId = customerId, VisitDate = DateTime.Now});

                    await _context.SaveChangesAsync();

                    _context.AnonymousCustomerCart.Add(
                        new AnonymousCustomerCart {AnonymousCustomerCartId = customerId});

                    await _context.SaveChangesAsync();

                    _context.AnonymousCustomerCartDetail.Add(new AnonymousCustomerCartDetail
                    {
                        AnonymousCustomerCartId = customerId,
                        ProductId = productId,
                        Quantity = quantity,
                        DisplayPrice = product.DisplayPrice,
                        Price = product.RealPrice,
                    });

                    await _context.SaveChangesAsync();

                    return Json(new
                    {
                        Status = "Success",
                        Message = "Add product into cart successful"
                    });
                }
                else
                {
                    if (_context.AnonymousCustomerCart.Find(customerId) != null)
                    {
                        var cartDetail = _context.AnonymousCustomerCartDetail.Find(customerId, productId);

                        if (cartDetail != null)
                        {
                            if (product.MaximumQuantity >= cartDetail.Quantity + quantity)
                            {
                                cartDetail.Quantity += quantity;
                                await _context.SaveChangesAsync();

                                return Json(new
                                {
                                    Status = "Success",
                                    Message = "Add product into cart successful"
                                });
                            }

                            return Json(new
                            {
                                Status = "Failed",
                                Message = "Product Quantiy is over maximum quantity can order in a time"
                            });
                        }

                        _context.AnonymousCustomerCartDetail.Add(new AnonymousCustomerCartDetail
                        {
                            AnonymousCustomerCartId = customerId,
                            ProductId = productId,
                            Quantity = quantity,
                            DisplayPrice = product.DisplayPrice,
                            Price = product.RealPrice,
                        });

                        await _context.SaveChangesAsync();

                        return Json(new
                        {
                            Status = "Success",
                            Message = "Add product into cart successful"
                        });
                    }
                    else
                    {
                        customerId = Guid.NewGuid().ToString();

                        Set("AnonymousId", customerId);

                        _context.AnonymousCustomer.Add(
                            new AnonymousCustomer {AnonymousCustomerId = customerId, VisitDate = DateTime.Now});

                        await _context.SaveChangesAsync();

                        _context.AnonymousCustomerCart.Add(
                            new AnonymousCustomerCart {AnonymousCustomerCartId = customerId});

                        await _context.SaveChangesAsync();

                        _context.AnonymousCustomerCartDetail.Add(new AnonymousCustomerCartDetail
                        {
                            AnonymousCustomerCartId = customerId,
                            ProductId = productId,
                            Quantity = quantity,
                            DisplayPrice = product.DisplayPrice,
                            Price = product.RealPrice,
                        });

                        await _context.SaveChangesAsync();

                        return Json(new
                        {
                            Status = "Success",
                            Message = "Add product into cart successful"
                        });
                    }
                }
            }
        }

        public void Set(string key, string value)
        {
            CookieOptions option = new CookieOptions();

            option.Expires = DateTime.Now.AddDays(30);

            Response.Cookies.Append(key, value, option);
        }

        [Route("ShowCartHeader")]
        public IActionResult ShowCartHeader([FromQuery] string customerId)
        {
//            var customerId = Request.Cookies["AnonymousId"];

            if (string.IsNullOrEmpty(customerId))
            {
                return NoContent();
            }

            if (User.Identity.IsAuthenticated)
            {
//                var id = User.Identities.FirstOrDefault(u => u.IsAuthenticated)
//                    ?.FindFirst(c => c.Type == JwtRegisteredClaimNames.NameId)?.Value;

                var customerCart = _context.CustomerCartDetail.Where(c => c.CustomerCartId == customerId).Select(x =>
                        new {x.ProductId, x.DisplayPrice, x.Product.ProductThumbImage, x.Quantity, x.Product.Name, x.Product.Slug})
                    .ToList();

                return Json(customerCart);
            }

            if (_context.AnonymousCustomerCart.Find(customerId) != null)
            {
                var customerCart = _context.AnonymousCustomerCartDetail
                    .Where(c => c.AnonymousCustomerCartId == customerId).Select(x =>
                        new {x.ProductId, x.DisplayPrice, x.Product.ProductThumbImage, x.Quantity, x.Product.Name, x.Product.Slug})
                    .ToList();

                return Json(customerCart);
            }

            return NoContent();
        }

        [Route("UpdateCartDetail")]
        [HttpPost]
        public IActionResult Update([FromBody] JObject data)
        {
            return Ok();
        }
    }
}