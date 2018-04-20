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
    [Route("api/AnonymousCustomerCart")]
    public class AnonymousCustomerCartController : Controller
    {
        private readonly OnovaContext _context;

        public AnonymousCustomerCartController(OnovaContext context)
        {
            _context = context;
        }

        // GET: api/AnonymousCustomerCart
        [HttpGet]
        public IEnumerable<AnonymousCustomerCart> GetAnonymousCustomerCart()
        {
            return _context.AnonymousCustomerCart;
        }

        // GET: api/AnonymousCustomerCart/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAnonymousCustomerCart([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var anonymousCustomerCart = await _context.AnonymousCustomerCart.SingleOrDefaultAsync(m => m.AnonymousCustomerCartId == id);

            if (anonymousCustomerCart == null)
            {
                return NotFound();
            }

            return Ok();
        }

        [HttpGet]
        [Route("GetAnonymousCustomerCarts")]
        public IActionResult GetAnonymousCustomerCarts([FromQuery]string customerId)
        {
            var cart = _context.AnonymousCustomerCart.Where(c => c.AnonymousCustomerCartId == customerId).Select(c => new
            {
                c.DisplayPrice,
                c.PriceDiscount,
                c.ShippingFee,
                c.Tax,
                c.Promotion.PromotionCode,
                item = c.AnonymousCustomerCartDetail.Select(i => new
                {
                    i.ProductId,
                    i.Product.Name,
                    i.DisplayPrice,
                    i.Quantity,
                    TotalPrice = i.Quantity * i.DisplayPrice,
                    i.Product.ProductThumbImage
                })
            }).ToList();

            return Json(cart);
        }

        // PUT: api/AnonymousCustomerCart/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAnonymousCustomerCart([FromRoute] string id, [FromBody] AnonymousCustomerCart anonymousCustomerCart)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != anonymousCustomerCart.AnonymousCustomerCartId)
            {
                return BadRequest();
            }

            _context.Entry(anonymousCustomerCart).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AnonymousCustomerCartExists(id))
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

        // POST: api/AnonymousCustomerCart
        [HttpPost]
        public async Task<IActionResult> PostAnonymousCustomerCart([FromBody] AnonymousCustomerCart anonymousCustomerCart)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.AnonymousCustomerCart.Add(anonymousCustomerCart);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (AnonymousCustomerCartExists(anonymousCustomerCart.AnonymousCustomerCartId))
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(201);
        }

        // DELETE: api/AnonymousCustomerCart/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAnonymousCustomerCart([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var anonymousCustomerCart = await _context.AnonymousCustomerCart.SingleOrDefaultAsync(m => m.AnonymousCustomerCartId == id);
            if (anonymousCustomerCart == null)
            {
                return NotFound();
            }

            _context.AnonymousCustomerCart.Remove(anonymousCustomerCart);
            await _context.SaveChangesAsync();

            return Ok(anonymousCustomerCart);
        }

        private bool AnonymousCustomerCartExists(string id)
        {
            return _context.AnonymousCustomerCart.Any(e => e.AnonymousCustomerCartId == id);
        }
    }
}