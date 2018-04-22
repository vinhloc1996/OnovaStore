using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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
    [Route("api/Brand")]
    [Authorize(Policy = "Staff Only")]
    public class BrandController : Controller
    {
        private readonly OnovaContext _context;

        public BrandController(OnovaContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IEnumerable<Brand> GetBrand()
        {
            return _context.Brand;
        }

        [HttpGet("GetBrandsForHeader")]
        [AllowAnonymous]
        public IActionResult GetBrandsForHeader()
        {
            var debug = _context.Brand
                .Where(c => c.TotalProduct > 0 && c.IsHide != true).Select(c => new { c.BrandId, c.Name, c.Slug }).ToList();
            var json = Json(debug);
            return json;
        }

        [HttpGet]
        [Route("GetBrands")]
        public IEnumerable<GetBrandsDTO> GetBrands()
        {
            var brands =
                _context.Brand.Select(x => new GetBrandsDTO { BrandId = x.BrandId, BrandName = x.Name });

            return brands;
        }

        [HttpGet]
        [Route("GetBrandsForStaff")]
        public async Task<IEnumerable<GetBrandForStaff>> GetBrandsForStaff([FromQuery] string sortOrder, [FromQuery] string searchString)
        {
            string sortQuery = "";
            string whereQuery = string.IsNullOrEmpty(searchString) ? "" : " WHERE LOWER(b.Name) LIKE @searchString";
            
            sortOrder = string.IsNullOrEmpty(sortOrder) ? "id" : sortOrder.Trim().ToLower();

            switch (sortOrder)
            {
                case "id_desc":
                    sortQuery = "ORDER BY b.BrandID DESC";
                    break;
                case "name":
                    sortQuery = "ORDER BY b.Name";
                    break;
                case "name_desc":
                    sortQuery = "ORDER BY b.Name DESC";
                    break;
                case "totalproduct":
                    sortQuery = "ORDER BY TotalProduct";
                    break;
                case "totalproduct_desc":
                    sortQuery = "ORDER BY TotalProduct DESC";
                    break;
                default:
                    sortQuery = "ORDER BY b.BrandID";
                    break;
            }

            var conn = _context.Database.GetDbConnection();
            var brands = new List<GetBrandForStaff>();
            try
            {
                await conn.OpenAsync();
                using (var command = conn.CreateCommand())
                {
                    string query = "SELECT b.BrandID, b.Name, b.ContactEmail, b.TotalProduct " +
                                   "FROM Brand b " +
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
                            var row = new GetBrandForStaff
                            {
                                BrandId = reader.GetInt32(0),
                                BrandName = reader.GetString(1),
                                ContactEmail = reader.GetString(2),
                                TotalProducts = reader.GetInt32(3)
                            };

                            brands.Add(row);
                        }
                    }

                }
            }
            finally
            {
                conn.Close();
            }

            return brands;
        }

        // GET: api/Brand/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBrand([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var brand = await _context.Brand.SingleOrDefaultAsync(m => m.BrandId == id);

            if (brand == null)
            {
                return NotFound();
            }

            return Ok(brand);
        }

        // PUT: api/Brand/5
        [HttpPut]
        public async Task<IActionResult> PutBrand([FromBody] Brand brand)
        {
            _context.Entry(brand).State = EntityState.Modified;

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

        // POST: api/Brand
        [HttpPost]
        public async Task<IActionResult> PostBrand([FromBody] Brand brand)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            brand.IsHide = false;

            _context.Brand.Add(brand);

            if (await _context.SaveChangesAsync() > 0)
            {
                brand.Slug = "/brand" + "/b" + brand.BrandId + "/" + brand.Slug;
                _context.Entry(brand).State = EntityState.Modified;

                if (await _context.SaveChangesAsync() > 0)
                {
                    return StatusCode(201);
                }
            }

            return StatusCode(424);
        }

        // DELETE: api/Brand/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBrand([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var brand = await _context.Brand.SingleOrDefaultAsync(m => m.BrandId == id);
            if (brand == null)
            {
                return NotFound();
            }

            _context.Brand.Remove(brand);
            await _context.SaveChangesAsync();

            return Ok(brand);
        }

        private bool BrandExists(int id)
        {
            return _context.Brand.Any(e => e.BrandId == id);
        }
    }
}