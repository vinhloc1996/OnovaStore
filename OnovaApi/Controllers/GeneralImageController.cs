using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnovaApi.Data;
using OnovaApi.DTOs;
using OnovaApi.Models.DatabaseModels;

namespace OnovaApi.Controllers
{
    [Produces("application/json")]
    [Route("api/GeneralImage")]
    [Authorize(Policy = "Staff Only")]
    public class GeneralImageController : Controller
    {
        private readonly OnovaContext _context;

        public GeneralImageController(OnovaContext context)
        {
            _context = context;
        }

        // GET: api/GeneralImage
        [HttpGet]
        public IEnumerable<GeneralImage> GetGeneralImage()
        {
            return _context.GeneralImage;
        }

        // GET: api/GeneralImage/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetGeneralImage([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var generalImage = await _context.GeneralImage.SingleOrDefaultAsync(m => m.GeneralImageId == id);

            if (generalImage == null)
            {
                return NotFound();
            }

            return Ok(generalImage);
        }

        // PUT: api/GeneralImage/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGeneralImage([FromRoute] string id, [FromBody] GeneralImage generalImage)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != generalImage.GeneralImageId)
            {
                return BadRequest();
            }

            _context.Entry(generalImage).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GeneralImageExists(id))
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

        // POST: api/GeneralImage
        [HttpPost]
        public async Task<IActionResult> PostGeneralImage([FromBody] ImageUploadDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var image = new GeneralImage
            {
                StaffId = model.StaffId,
                ImageUrl = model.ImageUrl,
                GeneralImageId = model.ImageId,
                AddDate = model.AddDate
            };

            _context.GeneralImage.Add(image);
            

            return await _context.SaveChangesAsync() > 0 ? StatusCode(201) : StatusCode(424);
        }

        // DELETE: api/GeneralImage/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGeneralImage([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var generalImage = await _context.GeneralImage.SingleOrDefaultAsync(m => m.GeneralImageId == id);
            if (generalImage == null)
            {
                return NotFound();
            }

            _context.GeneralImage.Remove(generalImage);
            await _context.SaveChangesAsync();

            return Ok(generalImage);
        }

        private bool GeneralImageExists(string id)
        {
            return _context.GeneralImage.Any(e => e.GeneralImageId == id);
        }
    }
}