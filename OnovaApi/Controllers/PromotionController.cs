using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnovaApi.Data;
using OnovaApi.DTOs;
using OnovaApi.Models.DatabaseModels;

namespace OnovaApi.Controllers
{
    [Produces("application/json")]
    [Route("api/Promotion")]
    public class PromotionController : Controller
    {
        private readonly OnovaContext _context;

        public PromotionController(OnovaContext context)
        {
            _context = context;
        }

        // GET: api/Promotion
        [HttpGet]
        public IEnumerable<Promotion> GetPromotion()
        {
            return _context.Promotion;
        }

        [HttpGet]
        [Route("GetPromotionsForStaff")]
        public async Task<IEnumerable<GetPromotionForStaff>> GetPromotionsForStaff([FromQuery] string sortOrder,
            [FromQuery] string searchString)
        {
            string sortQuery = "";
            string whereQuery = string.IsNullOrEmpty(searchString)
                ? ""
                : " WHERE LOWER(p.PromotionCode) LIKE @searchString OR LOWER(p.TargetApply) LIKE @searchString OR LOWER(p.PercentOff) LIKE @searchString OR LOWER(p.PromotionStatus) LIKE @searchString ";

            sortOrder = string.IsNullOrEmpty(sortOrder) ? "id" : sortOrder.Trim().ToLower();

            switch (sortOrder)
            {
                case "id_desc":
                    sortQuery = "ORDER BY p.PromotionID DESC";
                    break;
                case "code":
                    sortQuery = "ORDER BY p.PromotionCode";
                    break;
                case "code_desc":
                    sortQuery = "ORDER BY p.PromotionCode DESC";
                    break;
                case "targetapply":
                    sortQuery = "ORDER BY p.TargetApply";
                    break;
                case "targetapply_desc":
                    sortQuery = "ORDER BY p.TargetApply DESC";
                    break;
                case "discount":
                    sortQuery = "ORDER BY p.PercentOff";
                    break;
                case "discount_desc":
                    sortQuery = "ORDER BY p.PercentOff DESC";
                    break;
                case "status":
                    sortQuery = "ORDER BY p.PromotionStatus";
                    break;
                case "status_desc":
                    sortQuery = "ORDER BY p.PromotionStatus DESC";
                    break;
                default:
                    sortQuery = "ORDER BY p.PromotionID";
                    break;
            }

            var conn = _context.Database.GetDbConnection();
            var promotions = new List<GetPromotionForStaff>();
            try
            {
                await conn.OpenAsync();
                using (var command = conn.CreateCommand())
                {
                    string query =
                        "SELECT p.PromotionID, p.PromotionCode, p.PromotionStatus, p.TargetApply, p.PercentOff " +
                        "FROM Promotion p " +
                        whereQuery + " " + sortQuery + " ";

                    command.CommandText = query;

                    if (whereQuery != "")
                    {
                        command.Parameters.Add(new SqlParameter("@searchString", "%" + searchString.ToLower() + "%"));
                    }

                    DbDataReader reader = await command.ExecuteReaderAsync();

                    if (reader.HasRows)
                    {
                        while (await reader.ReadAsync())
                        {
                            var row = new GetPromotionForStaff
                            {
                                PromotionId = reader.GetInt32(0),
                                PromotionCode = reader.GetString(1),
                                Status = reader.GetString(2),
                                TargetApply = reader.GetString(3),
                                Discount = reader.GetDecimal(4)
                            };

                            promotions.Add(row);
                        }
                    }
                }
            }
            finally
            {
                conn.Close();
            }

            return promotions;
        }

        [HttpGet]
        [Route("AddPromotionToCart")]
        public async Task<IActionResult> AddPromotionToCart([FromQuery] string code, [FromQuery] string customerId)
        {
            var promotion = _context.Promotion.FirstOrDefault(p => p.PromotionCode == code.Trim());

            if (promotion != null)
            {
                var currentTime = DateTime.Now;

                if (promotion.TargetApply.ToLower() == "all")
                {
                    if (currentTime > promotion.StartDate && currentTime < promotion.EndDate)
                    {
                        if (User.Identity.IsAuthenticated)
                        {
                            if (string.IsNullOrEmpty(customerId))
                            {
                                customerId = User.Identities.FirstOrDefault(u => u.IsAuthenticated)
                                    ?.FindFirst(
                                        c => c.Type == JwtRegisteredClaimNames.NameId ||
                                             c.Type == ClaimTypes.NameIdentifier)
                                    ?.Value;
                            }

                            var customerCart = _context.CustomerCart.Find(customerId);

                            if (customerCart != null)
                            {
                                customerCart.PromotionId = promotion.PromotionId;
                                customerCart.PriceDiscount = customerCart.DisplayPrice * (double) promotion.PercentOff;

                                _context.CustomerCart.Update(customerCart);
                                
                                var customerCartDetail =
                                    _context.CustomerCartDetail.Where(c => c.CustomerCartId == customerId).ToList();

                                if (customerCartDetail.Any())
                                {
                                    customerCartDetail.ForEach(c =>
                                    {
                                        c.PromotionId = promotion.PromotionId;
                                        c.PriceDiscount = c.DisplayPrice * (double) promotion.PercentOff;
                                    });
                                }

                                if (await _context.SaveChangesAsync() > 0)
                                {
                                    return Json(new
                                    {
                                        Status = "Success",
                                        Message = "Promotion is applied",
                                    });
                                }

                                return Json(new
                                {
                                    Status = "Failed",
                                    Message = "Cannot apply promotion to your cart",
                                });
                            }

                            return Json(new
                            {
                                Status = "Failed",
                                Message = "Cannot find customer cart",
                            });
                        }

                        if (string.IsNullOrEmpty(customerId))
                        {
                            return Json(new
                            {
                                Status = "Failed",
                                Message = "Cannot find customer cart",
                            });
                        }

                        var anonymousCart = _context.AnonymousCustomerCart.Find(customerId);

                        if (anonymousCart != null)
                        {
                            anonymousCart.PromotionId = promotion.PromotionId;
                            anonymousCart.PriceDiscount = anonymousCart.DisplayPrice * (double)promotion.PercentOff;

                            _context.AnonymousCustomerCart.Update(anonymousCart);

                            var anonymousCartDetail =
                                _context.AnonymousCustomerCartDetail.Where(c => c.AnonymousCustomerCartId == customerId).ToList();

                            if (anonymousCartDetail.Any())
                            {
                                anonymousCartDetail.ForEach(c =>
                                {
                                    c.PromotionId = promotion.PromotionId;
                                    c.PriceDiscount = c.DisplayPrice * (double)promotion.PercentOff;
                                });
                            }

                            if (await _context.SaveChangesAsync() > 0)
                            {
                                return Json(new
                                {
                                    Status = "Success",
                                    Message = "Promotion is applied",
                                });
                            }

                            return Json(new
                            {
                                Status = "Failed",
                                Message = "Cannot apply promotion to your cart",
                            });
                        }

                        return Json(new
                        {
                            Status = "Failed",
                            Message = "Cannot find customer cart",
                        });
                    }

                    return Json(new
                    {
                        Status = "Failed",
                        Message = "Promotion invalid"
                    });
                }

                if (promotion.TargetApply.ToLower() == "brand")
                {
                    var promoBrand =
                        _context.PromotionBrand.FirstOrDefault(b => b.PromotionId == promotion.PromotionId);
                    if (promoBrand != null)
                    {
                        if (currentTime > promotion.StartDate && currentTime < promotion.EndDate)
                        {
                            if (User.Identity.IsAuthenticated)
                            {
                                if (string.IsNullOrEmpty(customerId))
                                {
                                    customerId = User.Identities.FirstOrDefault(u => u.IsAuthenticated)
                                        ?.FindFirst(
                                            c => c.Type == JwtRegisteredClaimNames.NameId ||
                                                 c.Type == ClaimTypes.NameIdentifier)
                                        ?.Value;
                                }

                                var customerCartDetail =
                                    _context.CustomerCartDetail.Where(c => c.CustomerCartId == customerId && c.Product.BrandId == promoBrand.BrandId).ToList();

                                if (customerCartDetail.Any())
                                {
                                    customerCartDetail.ForEach(c =>
                                    {
                                        c.PromotionId = promotion.PromotionId;
                                        c.PriceDiscount = c.DisplayPrice * (double)promotion.PercentOff;
                                    });

                                    if (await _context.SaveChangesAsync() > 0)
                                    {
                                        var customerCart = _context.CustomerCart.Find(customerId);
                                        customerCart.PromotionId = promotion.PromotionId;
                                        customerCart.PriceDiscount = customerCartDetail.Sum(c => c.PriceDiscount);

                                        _context.CustomerCart.Update(customerCart);

                                        if (await _context.SaveChangesAsync() > 0)
                                        {
                                            return Json(new
                                            {
                                                Status = "Success",
                                                Message = "Promotion is applied",
                                            });
                                        }
                                    }

                                    return Json(new
                                    {
                                        Status = "Failed",
                                        Message = "Cannot apply promotion to your cart",
                                    });
                                }

                                return Json(new
                                {
                                    Status = "Failed",
                                    Message = "Your cart don't have valid product for promotion",
                                });
                            }

                            if (string.IsNullOrEmpty(customerId))
                            {
                                return Json(new
                                {
                                    Status = "Failed",
                                    Message = "Cannot find customer cart",
                                });
                            }

                            var anonymouseCartDetail =
                                    _context.AnonymousCustomerCartDetail.Where(c => c.AnonymousCustomerCartId == customerId && c.Product.BrandId == promoBrand.BrandId).ToList();

                            if (anonymouseCartDetail.Any())
                            {
                                anonymouseCartDetail.ForEach(c =>
                                {
                                    c.PromotionId = promotion.PromotionId;
                                    c.PriceDiscount = c.DisplayPrice * (double)promotion.PercentOff;
                                });

                                if (await _context.SaveChangesAsync() > 0)
                                {
                                    var anonymousCart = _context.AnonymousCustomerCart.Find(customerId);
                                    anonymousCart.PromotionId = promotion.PromotionId;
                                    anonymousCart.PriceDiscount = anonymouseCartDetail.Sum(c => c.PriceDiscount);

                                    _context.AnonymousCustomerCart.Update(anonymousCart);

                                    if (await _context.SaveChangesAsync() > 0)
                                    {
                                        return Json(new
                                        {
                                            Status = "Success",
                                            Message = "Promotion is applied",
                                        });
                                    }
                                }

                                return Json(new
                                {
                                    Status = "Failed",
                                    Message = "Cannot apply promotion to your cart",
                                });
                            }

                            return Json(new
                            {
                                Status = "Failed",
                                Message = "Your cart don't have valid product for promotion",
                            });
                        }

                        return Json(new
                        {
                            Status = "Failed",
                            Message = "Promotion invalid",
                        });
                    }
                }

                if (promotion.TargetApply.ToLower() == "category")
                {
                    var promoCategory =
                        _context.PromotionCategory.FirstOrDefault(b => b.PromotionId == promotion.PromotionId);

                    if (promoCategory != null)
                    {
                        if (currentTime > promotion.StartDate && currentTime < promotion.EndDate)
                        {
                            if (User.Identity.IsAuthenticated)
                            {
                                if (string.IsNullOrEmpty(customerId))
                                {
                                    customerId = User.Identities.FirstOrDefault(u => u.IsAuthenticated)
                                        ?.FindFirst(
                                            c => c.Type == JwtRegisteredClaimNames.NameId ||
                                                 c.Type == ClaimTypes.NameIdentifier)
                                        ?.Value;
                                }

                                var customerCartDetail =
                                    _context.CustomerCartDetail.Where(c => c.CustomerCartId == customerId && c.Product.CategoryId == promoCategory.CategoryId).ToList();

                                if (customerCartDetail.Any())
                                {
                                    customerCartDetail.ForEach(c =>
                                    {
                                        c.PromotionId = promotion.PromotionId;
                                        c.PriceDiscount = c.DisplayPrice * (double)promotion.PercentOff;
                                    });

                                    if (await _context.SaveChangesAsync() > 0)
                                    {
                                        var customerCart = _context.CustomerCart.Find(customerId);
                                        customerCart.PromotionId = promotion.PromotionId;
                                        customerCart.PriceDiscount = customerCartDetail.Sum(c => c.PriceDiscount);

                                        _context.CustomerCart.Update(customerCart);

                                        if (await _context.SaveChangesAsync() > 0)
                                        {
                                            return Json(new
                                            {
                                                Status = "Success",
                                                Message = "Promotion is applied",
                                            });
                                        }
                                    }

                                    return Json(new
                                    {
                                        Status = "Failed",
                                        Message = "Cannot apply promotion to your cart",
                                    });
                                }

                                return Json(new
                                {
                                    Status = "Failed",
                                    Message = "Your cart don't have valid product for promotion",
                                });
                            }

                            if (string.IsNullOrEmpty(customerId))
                            {
                                return Json(new
                                {
                                    Status = "Failed",
                                    Message = "Cannot find customer cart",
                                });
                            }

                            var anonymouseCartDetail =
                                    _context.AnonymousCustomerCartDetail.Where(c => c.AnonymousCustomerCartId == customerId && c.Product.CategoryId == promoCategory.CategoryId).ToList();

                            if (anonymouseCartDetail.Any())
                            {
                                anonymouseCartDetail.ForEach(c =>
                                {
                                    c.PromotionId = promotion.PromotionId;
                                    c.PriceDiscount = c.DisplayPrice * (double)promotion.PercentOff;
                                });

                                if (await _context.SaveChangesAsync() > 0)
                                {
                                    var anonymousCart = _context.AnonymousCustomerCart.Find(customerId);
                                    anonymousCart.PromotionId = promotion.PromotionId;
                                    anonymousCart.PriceDiscount = anonymouseCartDetail.Sum(c => c.PriceDiscount);

                                    _context.AnonymousCustomerCart.Update(anonymousCart);

                                    if (await _context.SaveChangesAsync() > 0)
                                    {
                                        return Json(new
                                        {
                                            Status = "Success",
                                            Message = "Promotion is applied",
                                        });
                                    }
                                }

                                return Json(new
                                {
                                    Status = "Failed",
                                    Message = "Cannot apply promotion to your cart",
                                });
                            }

                            return Json(new
                            {
                                Status = "Failed",
                                Message = "Your cart don't have valid product for promotion",
                            });
                        }

                        return Json(new
                        {
                            Status = "Failed",
                            Message = "Promotion invalid",
                        });
                    }
                }
            }

            return Json(new
            {
                Status = "Failed",
                Message = "Promotion invalid"
            });
        }

        [HttpGet]
        [Route("RemovePromotionFromCart")]
        public async Task<IActionResult> RemovePromotionFromCart([FromQuery] string code, [FromQuery] string customerId)
        {
            var promotion = _context.Promotion.FirstOrDefault(p => p.PromotionCode == code.Trim());

            if (promotion != null)
            {
                if (User.Identity.IsAuthenticated)
                {
                    if (string.IsNullOrEmpty(customerId))
                    {
                        customerId = User.Identities.FirstOrDefault(u => u.IsAuthenticated)
                            ?.FindFirst(
                                c => c.Type == JwtRegisteredClaimNames.NameId ||
                                     c.Type == ClaimTypes.NameIdentifier)
                            ?.Value;
                    }

                    var customerCart = _context.CustomerCart.Find(customerId);

                    if (customerCart != null)
                    {
                        customerCart.PriceDiscount = 0.0;
                        customerCart.PromotionId = null;

                        _context.CustomerCart.Update(customerCart);

                        if (await _context.SaveChangesAsync() > 0)
                        {
                            var customerCartDetail =
                                _context.CustomerCartDetail.Where(c => c.CustomerCartId == customerId).ToList();

                            if (customerCartDetail.Any())
                            {
                                customerCartDetail.ForEach(c =>
                                {
                                    c.PromotionId = null;
                                    c.PriceDiscount = 0.0;
                                });
                            }

                            if (await _context.SaveChangesAsync() > 0)
                            {
                                return Json(new
                                {
                                    Status = "Success",
                                    Message = "Remove promotion code successful"
                                });
                            }
                        }

                        return Json(new
                        {
                            Status = "Failed",
                            Message = "Remove promotion code failed"
                        });
                    }
                }

                if (string.IsNullOrEmpty(customerId))
                {
                    return Json(new
                    {
                        Status = "Failed",
                        Message = "Cannot find customer cart",
                    });
                }

                var anonymouseCart =
                    _context.AnonymousCustomerCart.Find(customerId);

                if (anonymouseCart != null)
                {
                    anonymouseCart.PriceDiscount = 0.0;
                    anonymouseCart.PromotionId = null;

                    _context.AnonymousCustomerCart.Update(anonymouseCart);

                    if (await _context.SaveChangesAsync() > 0)
                    {
                        var anonymousCartDetail =
                            _context.AnonymousCustomerCartDetail.Where(c => c.AnonymousCustomerCartId == customerId).ToList();

                        if (anonymousCartDetail.Any())
                        {
                            anonymousCartDetail.ForEach(c =>
                            {
                                c.PromotionId = null;
                                c.PriceDiscount = 0.0;
                            });
                        }

                        if (await _context.SaveChangesAsync() > 0)
                        {
                            return Json(new
                            {
                                Status = "Success",
                                Message = "Remove promotion code successful"
                            });
                        }
                    }

                    return Json(new
                    {
                        Status = "Failed",
                        Message = "Remove promotion code failed"
                    });
                }

                return Json(new
                {
                    Status = "Failed",
                    Message = "Cannot find customer cart"
                });
            }
            
            return Json(new
            {
                Status = "Failed",
                Message = "Promotion invalid"
            });
        }

        // GET: api/Promotion/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPromotion([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var promotion = await _context.Promotion.SingleOrDefaultAsync(m => m.PromotionId == id);

            if (promotion == null)
            {
                return NotFound();
            }

            return Ok(promotion);
        }

        // PUT: api/Promotion/5
        [HttpPut]
        public async Task<IActionResult> PutPromotion([FromBody] Promotion promotion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Entry(promotion).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound();
            }

            return Ok();
        }

        // POST: api/Promotion
        [HttpPost]
        public async Task<IActionResult> PostPromotion([FromBody] AddPromotionDTO promotion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var promo = new Promotion
            {
                PromotionName = promotion.PromotionName,
                PromotionStatus = promotion.PromotionStatus,
                PromotionDescription = promotion.PromotionDescription,
                PromotionCode = promotion.PromotionCode,
                TargetApply = promotion.TargetApply,
                StartDate = promotion.StartDate,
                EndDate = promotion.EndDate,
                PercentOff = promotion.PercentOff
            };

            _context.Promotion.Add(promo);

            if (await _context.SaveChangesAsync() > 0)
            {
                if (promotion.TargetApply == "Brand")
                {
                    var promoBrand = new PromotionBrand
                    {
                        BrandId = promotion.PromotionBrand,
                        PromotionId = promo.PromotionId
                    };

                    _context.PromotionBrand.Add(promoBrand);
                }

                if (promotion.TargetApply == "Category")
                {
                    var promoCategory = new PromotionCategory()
                    {
                        CategoryId = promotion.PromotionCategory,
                        PromotionId = promo.PromotionId
                    };

                    _context.PromotionCategory.Add(promoCategory);
                }

                if (await _context.SaveChangesAsync() > 0)
                {
                    return StatusCode(201);
                }
            }

            return StatusCode(424);
        }

        // DELETE: api/Promotion/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePromotion([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var promotion = await _context.Promotion.SingleOrDefaultAsync(m => m.PromotionId == id);
            if (promotion == null)
            {
                return NotFound();
            }

            _context.Promotion.Remove(promotion);
            await _context.SaveChangesAsync();

            return Ok(promotion);
        }

        private bool PromotionExists(int id)
        {
            return _context.Promotion.Any(e => e.PromotionId == id);
        }
    }
}