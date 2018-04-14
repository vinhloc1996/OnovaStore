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
    [Route("api/AnonymousCustomer")]
    public class AnonymousCustomerController : Controller
    {
        private readonly OnovaContext _context;

        public AnonymousCustomerController(OnovaContext context)
        {
            _context = context;
        }

        // GET: api/AnonymousCustomer
        [HttpGet]
        public IEnumerable<AnonymousCustomer> GetAnonymousCustomer()
        {
            return _context.AnonymousCustomer;
        }

        // GET: api/AnonymousCustomer/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAnonymousCustomer([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var anonymousCustomer = await _context.AnonymousCustomer.SingleOrDefaultAsync(m => m.AnonymousCustomerId == id);

            if (anonymousCustomer == null)
            {
                return NotFound();
            }

            return Ok();
        }

        // PUT: api/AnonymousCustomer/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAnonymousCustomer([FromRoute] string id, [FromBody] AnonymousCustomer anonymousCustomer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != anonymousCustomer.AnonymousCustomerId)
            {
                return BadRequest();
            }

            _context.Entry(anonymousCustomer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AnonymousCustomerExists(id))
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

        // POST: api/AnonymousCustomer
        [HttpPost]
        public async Task<IActionResult> PostAnonymousCustomer([FromBody] AnonymousCustomer anonymousCustomer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.AnonymousCustomer.Add(anonymousCustomer);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (AnonymousCustomerExists(anonymousCustomer.AnonymousCustomerId))
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetAnonymousCustomer", new { id = anonymousCustomer.AnonymousCustomerId }, anonymousCustomer);
        }

        // DELETE: api/AnonymousCustomer/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAnonymousCustomer([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var anonymousCustomer = await _context.AnonymousCustomer.SingleOrDefaultAsync(m => m.AnonymousCustomerId == id);
            if (anonymousCustomer == null)
            {
                return NotFound();
            }

            _context.AnonymousCustomer.Remove(anonymousCustomer);
            await _context.SaveChangesAsync();

            return Ok(anonymousCustomer);
        }

        private bool AnonymousCustomerExists(string id)
        {
            return _context.AnonymousCustomer.Any(e => e.AnonymousCustomerId == id);
        }
    }
}