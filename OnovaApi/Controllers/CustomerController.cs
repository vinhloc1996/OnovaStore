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
using Newtonsoft.Json.Linq;
using OnovaApi.Data;
using OnovaApi.Models.DatabaseModels;
using OnovaApi.Services;

namespace OnovaApi.Controllers
{
    [Produces("application/json")]
    [Route("api/Customer")]
    public class CustomerController : Controller
    {
        private readonly OnovaContext _context;
        private readonly IAuthRepository _auth;

        public CustomerController(OnovaContext context, IAuthRepository auth)
        {
            _context = context;
            _auth = auth;
        }

        // GET: api/Customer
        [HttpGet]
        public IEnumerable<Customer> GetCustomer()
        {
            return _context.Customer;
        }

        [HttpGet]
        [Route("GetCustomerInfo")]
        [Authorize]
        public IActionResult GetCustomerInfo()
        {
            var currentCustomerId = User.Identities.FirstOrDefault(u => u.IsAuthenticated)
                ?.FindFirst(
                    c => c.Type == JwtRegisteredClaimNames.NameId || c.Type == ClaimTypes.NameIdentifier)
                ?.Value;

            var customer = _context.Users.Find(currentCustomerId);

            if (customer != null)
            {
                return Json(new
                {
                    customer.Email,
                    customer.FullName,
                    DOB = customer.DateOfBirth,
                    customer.Gender
                });
            }

            return Json(null);
        }

        [HttpPost]
        [Route("UpdateCustomerInfo")]
        [Authorize]
        public async Task<IActionResult> UpdateCustomerInfo([FromBody] JObject info)
        {
            var fullname = (string)info.GetValue("fullname");
            var dob = DateTime.Parse(info.GetValue("dob").ToString());
            var gender = (string)info.GetValue("gender");

            var currentCustomerId = User.Identities.FirstOrDefault(u => u.IsAuthenticated)
                ?.FindFirst(
                    c => c.Type == JwtRegisteredClaimNames.NameId || c.Type == ClaimTypes.NameIdentifier)
                ?.Value;

            var customer = _context.Users.Find(currentCustomerId);

            customer.DateOfBirth = dob;
            customer.FullName = fullname;
            customer.Gender = gender == "male";

            _context.Users.Update(customer);

            if (await _context.SaveChangesAsync() > 0)
            {
                var current = _context.Users.Find(currentCustomerId);

                return Json(new
                {
                    Status = "Success",
                    Message = "Update user info success",
                    customer = new
                    {
                        current.Email,
                        current.FullName,
                        DOB = current.DateOfBirth,
                        current.Gender
                    }
                });
            }

            return Json(new
            {
                Status = "Failed",
                Message = "Cannot update user info"
            });
        }

        [HttpPost]
        [Route("ChangePassword")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] JObject pass)
        {
            var currentPassword = (string)pass.GetValue("CurrentPassword");
            var newPassword = (string)pass.GetValue("NewPassword");

            var currentCustomerId = User.Identities.FirstOrDefault(u => u.IsAuthenticated)
                ?.FindFirst(
                    c => c.Type == JwtRegisteredClaimNames.NameId || c.Type == ClaimTypes.NameIdentifier)
                ?.Value;

            var customer = _context.Users.Find(currentCustomerId);

            var result = await _auth.ChangePassword(customer, currentPassword, newPassword);

            if (result.Succeeded)
            {
                return Json(new
                {
                    Status = "Success",
                    Message = "Update user info success"
                });
            }

            return Json(new
            {
                Status = "Failed",
                Message = "Cannot update user info"
            });
        }

        // GET: api/Customer/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomer([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var customer = await _context.Customer.SingleOrDefaultAsync(m => m.CustomerId == id);

            if (customer == null)
            {
                return NotFound();
            }

            return Ok(customer);
        }

        // PUT: api/Customer/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer([FromRoute] string id, [FromBody] Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != customer.CustomerId)
            {
                return BadRequest();
            }

            _context.Entry(customer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(id))
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

        // POST: api/Customer
        [HttpPost]
        public async Task<IActionResult> PostCustomer([FromBody] Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Customer.Add(customer);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (CustomerExists(customer.CustomerId))
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetCustomer", new { id = customer.CustomerId }, customer);
        }

        // DELETE: api/Customer/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var customer = await _context.Customer.SingleOrDefaultAsync(m => m.CustomerId == id);
            if (customer == null)
            {
                return NotFound();
            }

            _context.Customer.Remove(customer);
            await _context.SaveChangesAsync();

            return Ok(customer);
        }

        private bool CustomerExists(string id)
        {
            return _context.Customer.Any(e => e.CustomerId == id);
        }
    }
}