using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using OnovaApi.Data;
using OnovaApi.Models.DatabaseModels;

namespace OnovaApi.Controllers
{
    [Produces("application/json")]
    [Route("api/Cart")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CartController : Controller
    {
        private readonly OnovaContext _context;

        public CartController(OnovaContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
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

            if (quantity > product.CurrentQuantity)
            {
                return Json(new
                {
                    Status = "Failed",
                    Message = "Product Quantiy is over current quantity in store"
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
                        if (product.CurrentQuantity >= cartDetail.Quantity + quantity)
                        {
                            cartDetail.Quantity += quantity;

                            if (await _context.SaveChangesAsync() > 0)
                            {
                                var cart = _context.CustomerCart.Find(customerId);

                                var quantityRemain = _context.CustomerCartDetail.Where(c => c.CustomerCartId == customerId)
                                    .Sum(c => c.Quantity);
                                var totalPriceRemain = _context.CustomerCartDetail.Where(c => c.CustomerCartId == customerId)
                                    .Select(c => new { totalPriceItem = c.DisplayPrice * c.Quantity }).Sum(c => c.totalPriceItem);

                                cart.DisplayPrice = totalPriceRemain;
                                cart.TotalPrice = _context.CustomerCartDetail.Where(c => c.CustomerCartId == customerId)
                                    .Sum(c => c.Price);
                                cart.TotalQuantity = quantityRemain;
                                cart.ShippingFee = totalPriceRemain > 100 ? 0 : 25;

                                _context.CustomerCart.Update(cart);

                                if (await _context.SaveChangesAsync() > 0)
                                {
                                    return Json(new
                                    {
                                        Status = "Success",
                                        ProductName = product.Name,
                                        Product = _context.CustomerCartDetail.Where(c => c.CustomerCartId == customerId).Select(x =>
                                            new
                                            {
                                                x.ProductId,
                                                x.DisplayPrice,
                                                x.Product.ProductThumbImage,
                                                x.Quantity,
                                                x.Product.Name,
                                                x.Product.Slug
                                            }).ToList()
                                    });
                                }

                                return Json(new
                                {
                                    Status = "Failed",
                                    Message = "Error while updating cart"
                                });
                            }

                            return Json(new
                            {
                                Status = "Failed",
                                Message = "Cannot add product into cart"
                            });
                        }

                        return Json(new
                        {
                            Status = "Failed",
                            Message = "Product Quantiy is over current quantity in store"
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
                    var cart = _context.CustomerCart.Find(customerId);

                    var quantityRemain = _context.CustomerCartDetail.Where(c => c.CustomerCartId == customerId)
                        .Sum(c => c.Quantity);
                    var totalPriceRemain = _context.CustomerCartDetail.Where(c => c.CustomerCartId == customerId)
                        .Select(c => new { totalPriceItem = c.DisplayPrice * c.Quantity }).Sum(c => c.totalPriceItem);

                    cart.DisplayPrice = totalPriceRemain;
                    cart.TotalPrice = _context.CustomerCartDetail.Where(c => c.CustomerCartId == customerId)
                        .Sum(c => c.Price);
                    cart.TotalQuantity = quantityRemain;
                    cart.ShippingFee = totalPriceRemain > 100 ? 0 : 25;

                    _context.CustomerCart.Update(cart);

                    if (await _context.SaveChangesAsync() > 0)
                    {
                        return Json(new
                        {
                            Status = "Success",
                            ProductName = product.Name,
                            Product = _context.CustomerCartDetail.Where(c => c.CustomerCartId == customerId).Select(x =>
                                new
                                {
                                    x.ProductId,
                                    x.DisplayPrice,
                                    x.Product.ProductThumbImage,
                                    x.Quantity,
                                    x.Product.Name,
                                    x.Product.Slug
                                }).ToList()
                        });
                    }

                    return Json(new
                    {
                        Status = "Failed",
                        Message = "Error while updating cart"
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
                
//                Set("AnonymousId", customerId);

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

                if (await _context.SaveChangesAsync() > 0)
                {
                    var cart = _context.AnonymousCustomerCart.Find(customerId);

                    var quantityRemain = _context.AnonymousCustomerCartDetail.Where(c => c.AnonymousCustomerCartId == customerId)
                        .Sum(c => c.Quantity);
                    var totalPriceRemain = _context.AnonymousCustomerCartDetail.Where(c => c.AnonymousCustomerCartId == customerId)
                        .Select(c => new { totalPriceItem = c.DisplayPrice * c.Quantity }).Sum(c => c.totalPriceItem);

                    cart.DisplayPrice = totalPriceRemain;
                    cart.TotalPrice = _context.AnonymousCustomerCartDetail.Where(c => c.AnonymousCustomerCartId == customerId)
                        .Sum(c => c.Price);
                    cart.TotalQuantity = quantityRemain;
                    cart.ShippingFee = totalPriceRemain > 100 ? 0 : 25;

                    _context.AnonymousCustomerCart.Update(cart);

                    if (await _context.SaveChangesAsync() > 0)
                    {
                        return Json(new
                        {
                            Status = "Success",
                            ProductName = product.Name,
                            AnonymousId = customerId,
                            Product = _context.AnonymousCustomerCartDetail.Where(c => c.AnonymousCustomerCartId == customerId).Select(x =>
                                new
                                {
                                    x.ProductId,
                                    x.DisplayPrice,
                                    x.Product.ProductThumbImage,
                                    x.Quantity,
                                    x.Product.Name,
                                    x.Product.Slug
                                }).ToList()
                        });
                    }

                    return Json(new
                    {
                        Status = "Failed",
                        Message = "Error while updating cart"
                    });
                }

                return Json(new
                {
                    Status = "Failed",
                    Message = "Cannot add product into cart"
                });
            }

            var anonymousCart = _context.AnonymousCustomerCart.Find(customerId);

            if (anonymousCart != null)
            {
                var cartDetail = _context.AnonymousCustomerCartDetail.Find(customerId, productId);

                if (cartDetail != null)
                {
                    if (product.MaximumQuantity >= cartDetail.Quantity + quantity)
                    {
                        if (product.CurrentQuantity >= cartDetail.Quantity + quantity)
                        {
                            cartDetail.Quantity += quantity;

                            if (await _context.SaveChangesAsync() > 0)
                            {

                                var quantityRemain = _context.AnonymousCustomerCartDetail.Where(c => c.AnonymousCustomerCartId == customerId)
                                    .Sum(c => c.Quantity);
                                var totalPriceRemain = _context.AnonymousCustomerCartDetail.Where(c => c.AnonymousCustomerCartId == customerId)
                                    .Select(c => new { totalPriceItem = c.DisplayPrice * c.Quantity }).Sum(c => c.totalPriceItem);

                                anonymousCart.DisplayPrice = totalPriceRemain;
                                anonymousCart.TotalPrice = _context.AnonymousCustomerCartDetail.Where(c => c.AnonymousCustomerCartId == customerId)
                                    .Sum(c => c.Price);
                                anonymousCart.TotalQuantity = quantityRemain;
                                anonymousCart.ShippingFee = totalPriceRemain > 100 ? 0 : 25;

                                _context.AnonymousCustomerCart.Update(anonymousCart);

                                if (await _context.SaveChangesAsync() > 0)
                                {
                                    return Json(new
                                    {
                                        Status = "Success",
                                        ProductName = product.Name,
                                        Product = _context.AnonymousCustomerCartDetail.Where(c => c.AnonymousCustomerCartId == customerId).Select(x =>
                                            new
                                            {
                                                x.ProductId,
                                                x.DisplayPrice,
                                                x.Product.ProductThumbImage,
                                                x.Quantity,
                                                x.Product.Name,
                                                x.Product.Slug
                                            }).ToList()
                                    });
                                }

                                return Json(new
                                {
                                    Status = "Failed",
                                    Message = "Error while updating cart"
                                });
                            }

                            return Json(new
                            {
                                Status = "Failed",
                                Message = "Cannot add product into cart"
                            });
                        }

                        return Json(new
                        {
                            Status = "Failed",
                            Message = "Product Quantiy is over current quantity in store"
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

                if (await _context.SaveChangesAsync() > 0)
                {

                    var quantityRemain = _context.AnonymousCustomerCartDetail.Where(c => c.AnonymousCustomerCartId == customerId)
                        .Sum(c => c.Quantity);
                    var totalPriceRemain = _context.AnonymousCustomerCartDetail.Where(c => c.AnonymousCustomerCartId == customerId)
                        .Select(c => new { totalPriceItem = c.DisplayPrice * c.Quantity }).Sum(c => c.totalPriceItem);

                    anonymousCart.DisplayPrice = totalPriceRemain;
                    anonymousCart.TotalPrice = _context.AnonymousCustomerCartDetail.Where(c => c.AnonymousCustomerCartId == customerId)
                        .Sum(c => c.Price);
                    anonymousCart.TotalQuantity = quantityRemain;
                    anonymousCart.ShippingFee = totalPriceRemain > 100 ? 0 : 25;

                    _context.AnonymousCustomerCart.Update(anonymousCart);

                    if (await _context.SaveChangesAsync() > 0)
                    {
                        return Json(new
                        {
                            Status = "Success",
                            ProductName = product.Name,
                            Product = _context.AnonymousCustomerCartDetail.Where(c => c.AnonymousCustomerCartId == customerId).Select(x =>
                                new
                                {
                                    x.ProductId,
                                    x.DisplayPrice,
                                    x.Product.ProductThumbImage,
                                    x.Quantity,
                                    x.Product.Name,
                                    x.Product.Slug
                                }).ToList()
                        });
                    }

                    return Json(new
                    {
                        Status = "Failed",
                        Message = "Error while updating cart"
                    });
                }

                return Json(new
                {
                    Status = "Failed",
                    Message = "Cannot add product into cart"
                });
            }

            customerId = Guid.NewGuid().ToString();
//            Set("AnonymousId", customerId);

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

            if (await _context.SaveChangesAsync() > 0)
            {
                var cart = _context.AnonymousCustomerCart.Find(customerId);

                var quantityRemain = _context.AnonymousCustomerCartDetail.Where(c => c.AnonymousCustomerCartId == customerId)
                    .Sum(c => c.Quantity);
                var totalPriceRemain = _context.AnonymousCustomerCartDetail.Where(c => c.AnonymousCustomerCartId == customerId)
                    .Select(c => new { totalPriceItem = c.DisplayPrice * c.Quantity }).Sum(c => c.totalPriceItem);

                cart.DisplayPrice = totalPriceRemain;
                cart.TotalPrice = _context.AnonymousCustomerCartDetail.Where(c => c.AnonymousCustomerCartId == customerId)
                    .Sum(c => c.Price);
                cart.TotalQuantity = quantityRemain;
                cart.ShippingFee = totalPriceRemain > 100 ? 0 : 25;

                _context.AnonymousCustomerCart.Update(cart);

                if (await _context.SaveChangesAsync() > 0)
                {
                    return Json(new
                    {
                        Status = "Success",
                        ProductName = product.Name,
                        AnonymousId = customerId,
                        Product = _context.AnonymousCustomerCartDetail.Where(c => c.AnonymousCustomerCartId == customerId).Select(x =>
                            new
                            {
                                x.ProductId,
                                x.DisplayPrice,
                                x.Product.ProductThumbImage,
                                x.Quantity,
                                x.Product.Name,
                                x.Product.Slug
                            }).ToList()
                    });
                }

                return Json(new
                {
                    Status = "Failed",
                    Message = "Error while updating cart"
                });
            }

            return Json(new
            {
                Status = "Failed",
                Message = "Cannot add product into cart"
            });
        }

        public void Set(string key, string value)
        {
            CookieOptions option = new CookieOptions();

            option.Expires = DateTime.Now.AddDays(30);

            Response.Cookies.Append(key, value, option);
        }

        [AllowAnonymous]
        [Route("ShowCartHeader")]
        [HttpGet]
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

        [AllowAnonymous]
        [Route("RemoveCartItem")]
        [HttpGet]
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
                        var quantityRemain = _context.CustomerCartDetail.Where(c => c.CustomerCartId == customerId)
                            .Sum(c => c.Quantity);
                        var totalPriceRemain = _context.CustomerCartDetail.Where(c => c.CustomerCartId == customerId)
                            .Select(c => new {totalPriceItem = c.DisplayPrice * c.Quantity}).Sum(c => c.totalPriceItem);

                        return Json(new
                        {
                            Status = "Success",
                            Message = "Remove product " + product.Name + " from cart successful",
                            QuantityRemain = quantityRemain,
                            TotalPriceRemain = totalPriceRemain
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

                if (await _context.SaveChangesAsync() > 0)
                {
                    var quantityRemain = _context.AnonymousCustomerCartDetail.Where(c => c.AnonymousCustomerCartId == customerId)
                        .Sum(c => c.Quantity);
                    var totalPriceRemain = _context.AnonymousCustomerCartDetail.Where(c => c.AnonymousCustomerCartId == customerId)
                        .Select(c => new { totalPriceItem = c.DisplayPrice * c.Quantity }).Sum(c => c.totalPriceItem);

                    return Json(new
                    {
                        Status = "Success",
                        Message = "Remove product " + product.Name + " from cart successful",
                        QuantityRemain = quantityRemain,
                        TotalPriceRemain = totalPriceRemain
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
        [HttpGet]
        public async Task<IActionResult> UpdateCartDetail([FromQuery] string customerId, [FromQuery] int productId, [FromQuery] int quantity)
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

            if (quantity <= 0)
            {
                return Json(new
                {
                    Status = "Failed",
                    Message = "The quantity is invalid"
                });
            }

            if (User.Identity.IsAuthenticated)
            {
                var customerCart = _context.CustomerCart.Find(customerId);

                if (string.IsNullOrEmpty(customerId) || customerCart == null)
                {
                    customerId = User.Identities.FirstOrDefault(u => u.IsAuthenticated)
                        ?.FindFirst(
                            c => c.Type == JwtRegisteredClaimNames.NameId || c.Type == ClaimTypes.NameIdentifier)
                        ?.Value;
                }

                var cartDetail = _context.CustomerCartDetail.Find(customerId, productId);

                if (cartDetail != null)
                {
                    if (quantity > product.CurrentQuantity)
                    {
                        return Json(new
                        {
                            Status = "Failed",
                            Message = "Product Quantiy is over current quantity in store"
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

                    customerCart.TotalQuantity = customerCart.TotalQuantity - cartDetail.Quantity + quantity;
                    customerCart.DisplayPrice = customerCart.DisplayPrice - (cartDetail.Quantity * cartDetail.DisplayPrice) + (quantity * cartDetail.DisplayPrice);

                    cartDetail.Quantity = quantity;

                    _context.CustomerCart.Update(customerCart);
                    _context.CustomerCartDetail.Update(cartDetail);

                    if (await _context.SaveChangesAsync() > 0)
                    {
                        return Json(new
                        {
                            Status = "Success",
                            Message = "Update product quantity successful"
                        });
                    }

                    return Json(new
                    {
                        Status = "Failed",
                        Message = "Cannot update product to database"
                    });
                }

                return Json(new
                {
                    Status = "Failed",
                    Message = "The product is not in cart"
                });
            }

            var anonymousCart = _context.AnonymousCustomerCart.Find(customerId);

            if (anonymousCart != null)
            {
                var cartDetail = _context.AnonymousCustomerCartDetail.Find(customerId, productId);

                if (cartDetail != null)
                {
                    if (quantity > product.CurrentQuantity)
                    {
                        return Json(new
                        {
                            Status = "Failed",
                            Message = "Product Quantiy is over current quantity in store"
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

                    anonymousCart.TotalQuantity = anonymousCart.TotalQuantity - cartDetail.Quantity + quantity;
                    anonymousCart.DisplayPrice = anonymousCart.DisplayPrice - (cartDetail.Quantity * cartDetail.DisplayPrice) + (quantity * cartDetail.DisplayPrice);

                    cartDetail.Quantity = quantity;

                    _context.AnonymousCustomerCart.Update(anonymousCart);
                    _context.AnonymousCustomerCartDetail.Update(cartDetail);

                    if (await _context.SaveChangesAsync() > 0)
                    {
                        return Json(new
                        {
                            Status = "Success",
                            Message = "Update product quantity successful"
                        });
                    }

                    return Json(new
                    {
                        Status = "Failed",
                        Message = "Cannot update product to database"
                    });
                }

                return Json(new
                {
                    Status = "Failed",
                    Message = "The product is not in cart"
                });
            }

            return Json(new
            {
                Status = "Failed",
                Message = "Customer cart not found"
            });
        }
    }
}