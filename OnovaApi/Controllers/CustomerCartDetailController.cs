using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnovaApi.Data;
using OnovaApi.Models.DatabaseModels;

namespace OnovaApi.Controllers
{
    [Produces("application/json")]
    [Route("api/CustomerCartDetail")]
    public class CustomerCartDetailController : Controller
    {
        private readonly OnovaContext _context;

        public CustomerCartDetailController(OnovaContext context)
        {
            _context = context;
        }

        // GET: api/CustomerCartDetail
        [HttpGet]
        public IEnumerable<CustomerCartDetail> GetCustomerCartDetail()
        {
            return _context.CustomerCartDetail;
        }

        // GET: api/CustomerCartDetail/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomerCartDetail([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var customerCartDetail =
                _context.CustomerCartDetail.Where(c => c.CustomerCartId == id).Select(c => new {c.ProductId, c.Quantity, c.DisplayPrice, c.Product.ProductThumbImage, c.Product.Name, c.Product.Slug, TotalPrice = c.CustomerCart.DisplayPrice}).ToList();
            
            return Json(customerCartDetail);
        }

        // PUT: api/CustomerCartDetail/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomerCartDetail([FromRoute] string id,
            [FromBody] CustomerCartDetail customerCartDetail)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != customerCartDetail.CustomerCartId)
            {
                return BadRequest();
            }

            _context.Entry(customerCartDetail).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerCartDetailExists(id))
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

        // POST: api/CustomerCartDetail
        [HttpPost]
        public async Task<IActionResult> PostCustomerCartDetail([FromBody] DTOs.CustomerCartDetail detail)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var productInCart = _context.CustomerCartDetail.Find(detail.CustomerCartId, detail.ProductId);
            if (productInCart != null)
            {
                productInCart.Quantity += detail.Quantity;

                _context.Update(productInCart);
            }
            else
            {
                var customerCartDetail = new CustomerCartDetail
                {
                    ProductId = detail.ProductId,
                    CustomerCartId = detail.CustomerCartId,
                    Price = detail.Price,
                    DisplayPrice = detail.DisplayPrice,
                    Quantity = detail.Quantity
                };

                _context.CustomerCartDetail.Add(customerCartDetail);
            }

            return await _context.SaveChangesAsync() > 0 ? StatusCode(200) : StatusCode(409);
        }

        // DELETE: api/CustomerCartDetail/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomerCartDetail([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var customerCartDetail =
                await _context.CustomerCartDetail.SingleOrDefaultAsync(m => m.CustomerCartId == id);
            if (customerCartDetail == null)
            {
                return NotFound();
            }

            _context.CustomerCartDetail.Remove(customerCartDetail);
            await _context.SaveChangesAsync();

            return Ok(customerCartDetail);
        }

        private bool CustomerCartDetailExists(string id)
        {
            return _context.CustomerCartDetail.Any(e => e.CustomerCartId == id);
        }
    }
}