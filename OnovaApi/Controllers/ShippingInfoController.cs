using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using OnovaApi.Data;
using OnovaApi.Models.DatabaseModels;

namespace OnovaApi.Controllers
{
    [Produces("application/json")]
    [Route("api/ShippingInfo")]
    public class ShippingInfoController : Controller
    {
        private readonly OnovaContext _context;

        public ShippingInfoController(OnovaContext context)
        {
            _context = context;
        }

        // GET: api/ShippingInfo
        [HttpGet]
        public IEnumerable<ShippingInfo> GetShippingInfo()
        {
            return _context.ShippingInfo;
        }

        [HttpGet]
        [Route("UpdateAddressDefault")]
        public async Task<IActionResult> UpdateAddressDefault([FromQuery] int infoId)
        {
            if (User.Identity.IsAuthenticated)
            {
                var info = _context.ShippingInfo.Find(infoId);

                if (info != null)
                {
                    var currentCustomerId = User.Identities.FirstOrDefault(u => u.IsAuthenticated)
                        ?.FindFirst(
                            c => c.Type == JwtRegisteredClaimNames.NameId || c.Type == ClaimTypes.NameIdentifier)
                        ?.Value;

                    if (currentCustomerId == info.CustomerId)
                    {
                        if (!info.IsDefault)
                        {
                            var shippingInfos = _context.ShippingInfo.Where(c => c.CustomerId == currentCustomerId).ToList();

                            shippingInfos.ForEach(c => c.IsDefault = false);
                            info.IsDefault = true;

                            if (await _context.SaveChangesAsync() > 0)
                            {
                                return Json(new
                                {
                                    Status = "Success",
                                    Message = "The address info is set to default"
                                });
                            }

                            return Json(new
                            {
                                Status = "Failed",
                                Message = "Cannot update this address info"
                            });
                        }

                        return Json(new
                        {
                            Status = "Failed",
                            Message = "This address info is default already"
                        });
                    }

                    return Json(new
                    {
                        Status = "Failed",
                        Message = "The current user doesn't own this address info"
                    });
                }

                return Json(new
                {
                    Status = "Failed",
                    Message = "Shipping address not found"
                });
            }

            return Json(new
            {
                Status = "Failed",
                Message = "Unauthorize user"
            });
        }

        // GET: api/ShippingInfo/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetShippingInfo([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var shippingInfo = await _context.ShippingInfo.SingleOrDefaultAsync(m => m.ShippingInfoId == id);

            if (shippingInfo == null)
            {
                return NotFound();
            }

            return Ok(shippingInfo);
        }

        // PUT: api/ShippingInfo/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutShippingInfo([FromRoute] int id, [FromBody] ShippingInfo shippingInfo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != shippingInfo.ShippingInfoId)
            {
                return BadRequest();
            }

            _context.Entry(shippingInfo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ShippingInfoExists(id))
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

        [HttpGet]
        [Route("RemoveAddressInfo")]
        public async Task<IActionResult> RemoveAddressInfo([FromQuery] int infoId)
        {
            if (User.Identity.IsAuthenticated)
            {
                var info = _context.ShippingInfo.Find(infoId);

                if (info != null)
                {
                    var currentCustomerId = User.Identities.FirstOrDefault(u => u.IsAuthenticated)
                        ?.FindFirst(
                            c => c.Type == JwtRegisteredClaimNames.NameId || c.Type == ClaimTypes.NameIdentifier)
                        ?.Value;

                    if (currentCustomerId == info.CustomerId)
                    {
                        if (!info.IsDefault)
                        {
                            _context.ShippingInfo.Remove(info);
                            if (await _context.SaveChangesAsync() > 0)
                            {
                                return Json(new
                                {
                                    Status = "Success",
                                    Message = "The shipping info has been deleted"
                                });
                            }

                            return Json(new
                            {
                                Status = "Failed",
                                Message = "Cannot remove this shipping info"
                            });
                        }

                        return Json(new
                        {
                            Status = "Failed",
                            Message = "Cannot remove default address info"
                        });
                    }

                    return Json(new
                    {
                        Status = "Failed",
                        Message = "The current user doesn't own this address info"
                    });
                }

                return Json(new
                {
                    Status = "Failed",
                    Message = "Shipping address not found"
                });
            }

            return Json(new
            {
                Status = "Failed",
                Message = "Unauthorize user"
            });
        }

        // POST: api/ShippingInfo
        [HttpPost]
        [Route("AddAddressInfo")]
        public async Task<IActionResult> AddAddressInfo([FromBody] JObject info)
        {
            var fullname = (string) info.GetValue("fullName");
            var addressLine1 = (string)info.GetValue("addressLine1");
            var city = (string)info.GetValue("city");
            var phone = (string)info.GetValue("phone");
            var zip = (string)info.GetValue("zip");

            if (User.Identity.IsAuthenticated)
            {
                var currentCustomerId = User.Identities.FirstOrDefault(u => u.IsAuthenticated)
                    ?.FindFirst(
                        c => c.Type == JwtRegisteredClaimNames.NameId || c.Type == ClaimTypes.NameIdentifier)
                    ?.Value;

                var isOnly = _context.ShippingInfo.Where(c => c.CustomerId == currentCustomerId);

                bool isDefault = !isOnly.Any();

                var newInfo = new ShippingInfo
                {
                    IsDefault = isDefault,
                    CustomerId = currentCustomerId,
                    AddressLine1 = addressLine1,
                    City = city,
                    Phone = phone,
                    Zip = zip,
                    FullName = fullname
                };

                _context.ShippingInfo.Add(newInfo);
                if (await _context.SaveChangesAsync() > 0)
                {
                    return Json(new
                    {
                        Status = "Success",
                        Message = "Shipping info has been added successful"
                    });
                }

                return Json(new
                {
                    Status = "Failed",
                    Message = "Shipping address hasn't been added"
                });
            }

            return Json(new
            {
                Status = "Failed",
                Message = "Unauthorize user"
            });
        }

        // DELETE: api/ShippingInfo/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteShippingInfo([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var shippingInfo = await _context.ShippingInfo.SingleOrDefaultAsync(m => m.ShippingInfoId == id);
            if (shippingInfo == null)
            {
                return NotFound();
            }

            _context.ShippingInfo.Remove(shippingInfo);
            await _context.SaveChangesAsync();

            return Ok(shippingInfo);
        }

        private bool ShippingInfoExists(int id)
        {
            return _context.ShippingInfo.Any(e => e.ShippingInfoId == id);
        }
    }
}