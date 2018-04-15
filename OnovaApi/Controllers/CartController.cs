using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
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
        public async Task<IActionResult> AddToCart([FromQuery] string customerId, [FromQuery] int productId,
            [FromQuery] int quantity = 1)
        {
            var product = _context.Product.Find(productId);

            if (product == null)
            {
                return Json(new
                {
                    Status = "Failed",
                    Message = "Cannot find the product"
                });
            }

            if (quantity > product.MaximumQuantity)
            {
                return Json(new
                {
                    Status = "Failed",
                    Message = "Product Quantiy is over maximum quantity can order in a time"
                });
            }

            if (User.Identity.IsAuthenticated)
            {
                var currentCustomerId = User.Identities.FirstOrDefault(u => u.IsAuthenticated)
                    ?.FindFirst(
                        c => c.Type == JwtRegisteredClaimNames.NameId || c.Type == ClaimTypes.NameIdentifier)
                    ?.Value;

                if (string.IsNullOrEmpty(customerId) || currentCustomerId != customerId)
                {
                    //try to get id through identity
                    customerId = currentCustomerId;
                }

                var cartDetail = _context.CustomerCartDetail.Find(customerId, productId);

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

                _context.CustomerCartDetail.Add(new CustomerCartDetail
                {
                    CustomerCartId = customerId,
                    ProductId = productId,
                    Quantity = quantity,
                    DisplayPrice = product.DisplayPrice,
                    Price = product.RealPrice,
                });

                if (await _context.SaveChangesAsync() > 0)
                {
                    return Json(new
                    {
                        Status = "Success",
                        Message = "Add product into cart successful"
                    });
                }

                return Json(new
                {
                    Status = "Failed",
                    Message = "Cannot add product into cart"
                });

            }

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

        public void Set(string key, string value)
        {
            CookieOptions option = new CookieOptions();

            option.Expires = DateTime.Now.AddDays(30);

            Response.Cookies.Append(key, value, option);
        }

        [Route("ShowCartHeader")]
        public IActionResult ShowCartHeader([FromQuery] string customerId)
        {
            //Work to find user id
//            var currentSigned = User.Identities.FirstOrDefault(u => u.IsAuthenticated)
//                ?.FindFirst(c => c.Type == JwtRegisteredClaimNames.NameId || c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (User.Identity.IsAuthenticated)
            {
                if (string.IsNullOrEmpty(customerId))
                {
                    //try to get id through identity
                    customerId = User.Identities.FirstOrDefault(u => u.IsAuthenticated)
                        ?.FindFirst(
                            c => c.Type == JwtRegisteredClaimNames.NameId || c.Type == ClaimTypes.NameIdentifier)
                        ?.Value;
                }

                var customerCart = _context.CustomerCartDetail.Where(c => c.CustomerCartId == customerId).Select(x =>
                        new
                        {
                            x.ProductId,
                            x.DisplayPrice,
                            x.Product.ProductThumbImage,
                            x.Quantity,
                            x.Product.Name,
                            x.Product.Slug
                        })
                    .ToList();

                return Json(customerCart);
            }

            if (string.IsNullOrEmpty(customerId))
            {
                return NoContent();
            }

            if (_context.AnonymousCustomerCart.Find(customerId) != null)
            {
                var customerCart = _context.AnonymousCustomerCartDetail
                    .Where(c => c.AnonymousCustomerCartId == customerId).Select(x =>
                        new
                        {
                            x.ProductId,
                            x.DisplayPrice,
                            x.Product.ProductThumbImage,
                            x.Quantity,
                            x.Product.Name,
                            x.Product.Slug
                        })
                    .ToList();

                return Json(customerCart);
            }

            return NoContent();
        }

        [Route("RemoveCartItem")]
        public async Task<IActionResult> RemoveCartItem([FromQuery] string customerId, [FromQuery] int productId = 0)
        {
            var product = productId == 0 ? null : _context.Product.Find(productId);

            if (product == null)
            {
                return Json(new
                {
                    Status = "Failed",
                    Message = "The product not found"
                });
            }

            if (User.Identity.IsAuthenticated)
            {
                if (string.IsNullOrEmpty(customerId) || _context.CustomerCart.Find(customerId) == null)
                {
                    //try to get id through identity
                    customerId = User.Identities.FirstOrDefault(u => u.IsAuthenticated)
                        ?.FindFirst(
                            c => c.Type == JwtRegisteredClaimNames.NameId || c.Type == ClaimTypes.NameIdentifier)
                        ?.Value;
                }

                var cartDetail = _context.CustomerCartDetail.Find(customerId, productId);

                if (cartDetail != null)
                {
                    _context.CustomerCartDetail.Remove(cartDetail);

                    if (await _context.SaveChangesAsync() > 0)
                    {
                        return Json(new
                        {
                            Status = "Success",
                            Message = "Remove product from cart successful"
                        });
                    }

                    return Json(new
                    {
                        Status = "Failed",
                        Message = "Cannot remove current product from cart"
                    });
                }

                return Json(new
                {
                    Status = "Failed",
                    Message = "the current customer cart doesn't contain this item"
                });
            }

            if (string.IsNullOrEmpty(customerId))
            {
                return Json(new
                {
                    Status = "Failed",
                    Message = "Cannot get current customer cart"
                });
            }

            var cart = _context.AnonymousCustomerCartDetail.Find(customerId, productId);

            if (cart != null)
            {
                _context.AnonymousCustomerCartDetail.Remove(cart);
                await _context.SaveChangesAsync();

                return Json(new
                {
                    Status = "Success",
                    Message = "Remove product from cart successful"
                });
            }

            return Json(new
            {
                Status = "Failed",
                Message = "The product in current cart is not existed"
            });
        }

        [Route("CleanCart")]
        public async Task<IActionResult> CleanCart([FromQuery] string customerId)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (string.IsNullOrEmpty(customerId) || _context.CustomerCart.Find(customerId) == null)
                {
                    //try to get id through identity
                    customerId = User.Identities.FirstOrDefault(u => u.IsAuthenticated)
                        ?.FindFirst(
                            c => c.Type == JwtRegisteredClaimNames.NameId || c.Type == ClaimTypes.NameIdentifier)
                        ?.Value;
                }

                var countItems = _context.CustomerCartDetail.Count(c => c.CustomerCartId == customerId);

                if (countItems == 0)
                {
                    return Json(new
                    {
                        Status = "Failed",
                        Message = "The cart doesn't contain any item to be cleaned"
                    });
                }

                _context.CustomerCartDetail.RemoveRange(
                    _context.CustomerCartDetail.Where(c => c.CustomerCartId == customerId));

                if (await _context.SaveChangesAsync() > 0)
                {
                    return Json(new
                    {
                        Status = "Success",
                        Message = "The cart is cleaned up"
                    });
                }

                return Json(new
                {
                    Status = "Failed",
                    Message = "Cannot clean the items in cart"
                });
            }

            if (string.IsNullOrEmpty(customerId) ||
                _context.AnonymousCustomerCartDetail.Count(c => c.AnonymousCustomerCartId == customerId) == 0)
            {
                return Json(new
                {
                    Status = "Failed",
                    Message = "Cannot find the customer cart"
                });
            }

            _context.AnonymousCustomerCartDetail.RemoveRange(
                _context.AnonymousCustomerCartDetail.Where(c => c.AnonymousCustomerCartId == customerId));
            if (await _context.SaveChangesAsync() > 0)
            {
                return Json(new
                {
                    Status = "Success",
                    Message = "The cart is cleaned up"
                });
            }

            return Json(new
            {
                Status = "Success",
                Message = "The cart is cleaned up"
            });
        }

        [Route("UpdateCartDetail")]
        [HttpPost]
        public IActionResult Update([FromBody] JObject data)
        {
            return Ok();
        }
    }
}