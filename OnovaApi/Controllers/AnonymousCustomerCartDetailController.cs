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
    [Route("api/AnonymousCustomerCartDetail")]
    public class AnonymousCustomerCartDetailController : Controller
    {
        private readonly OnovaContext _context;

        public AnonymousCustomerCartDetailController(OnovaContext context)
        {
            _context = context;
        }

        // GET: api/AnonymousCustomerCartDetail
        [HttpGet]
        public IEnumerable<AnonymousCustomerCartDetail> GetAnonymousCustomerCartDetail()
        {
            return _context.AnonymousCustomerCartDetail;
        }

        // GET: api/AnonymousCustomerCartDetail/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAnonymousCustomerCartDetail([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var anonymousCustomerCartDetail = await _context.AnonymousCustomerCartDetail.SingleOrDefaultAsync(m => m.AnonymousCustomerCartId == id);

            if (anonymousCustomerCartDetail == null)
            {
                return NotFound();
            }

            return Ok(anonymousCustomerCartDetail);
        }

        // PUT: api/AnonymousCustomerCartDetail/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAnonymousCustomerCartDetail([FromRoute] string id, [FromBody] AnonymousCustomerCartDetail anonymousCustomerCartDetail)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != anonymousCustomerCartDetail.AnonymousCustomerCartId)
            {
                return BadRequest();
            }

            _context.Entry(anonymousCustomerCartDetail).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AnonymousCustomerCartDetailExists(id))
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

        // POST: api/AnonymousCustomerCartDetail
        [HttpPost]
        public async Task<IActionResult> PostAnonymousCustomerCartDetail([FromBody] AnonymousCustomerCartDetail anonymousCustomerCartDetail)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.AnonymousCustomerCartDetail.Add(anonymousCustomerCartDetail);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (AnonymousCustomerCartDetailExists(anonymousCustomerCartDetail.AnonymousCustomerCartId))
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

        // DELETE: api/AnonymousCustomerCartDetail/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAnonymousCustomerCartDetail([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var anonymousCustomerCartDetail = await _context.AnonymousCustomerCartDetail.SingleOrDefaultAsync(m => m.AnonymousCustomerCartId == id);
            if (anonymousCustomerCartDetail == null)
            {
                return NotFound();
            }

            _context.AnonymousCustomerCartDetail.Remove(anonymousCustomerCartDetail);
            await _context.SaveChangesAsync();

            return Ok(anonymousCustomerCartDetail);
        }

        private bool AnonymousCustomerCartDetailExists(string id)
        {
            return _context.AnonymousCustomerCartDetail.Any(e => e.AnonymousCustomerCartId == id);
        }
    }
}