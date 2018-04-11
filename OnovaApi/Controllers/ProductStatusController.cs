using System;
using System.Collections.Generic;
using System.Linq;
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
    [Route("api/ProductStatus")]
    public class ProductStatusController : Controller
    {
        private readonly OnovaContext _context;

        public ProductStatusController(OnovaContext context)
        {
            _context = context;
        }

        // GET: api/ProductStatus
        [HttpGet]
        [Route("GetProductStatus")]
        public IEnumerable<GetProductStatusDTO> GetProductStatus()
        {
            return _context.ProductStatus.Select(x => new GetProductStatusDTO{StatusId = x.ProductStatusId, StatusName = x.StatusName});
        }

        // GET: api/ProductStatus/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductStatus([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var productStatus = await _context.ProductStatus.SingleOrDefaultAsync(m => m.ProductStatusId == id);

            if (productStatus == null)
            {
                return NotFound();
            }

            return Ok(productStatus);
        }

        // PUT: api/ProductStatus/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProductStatus([FromRoute] int id, [FromBody] ProductStatus productStatus)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != productStatus.ProductStatusId)
            {
                return BadRequest();
            }

            _context.Entry(productStatus).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductStatusExists(id))
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

        // POST: api/ProductStatus
        [HttpPost]
        public async Task<IActionResult> PostProductStatus([FromBody] ProductStatus productStatus)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.ProductStatus.Add(productStatus);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProductStatus", new { id = productStatus.ProductStatusId }, productStatus);
        }

        // DELETE: api/ProductStatus/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductStatus([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var productStatus = await _context.ProductStatus.SingleOrDefaultAsync(m => m.ProductStatusId == id);
            if (productStatus == null)
            {
                return NotFound();
            }

            _context.ProductStatus.Remove(productStatus);
            await _context.SaveChangesAsync();

            return Ok(productStatus);
        }

        private bool ProductStatusExists(int id)
        {
            return _context.ProductStatus.Any(e => e.ProductStatusId == id);
        }
    }
}