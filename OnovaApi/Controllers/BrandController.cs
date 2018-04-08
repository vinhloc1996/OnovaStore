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
        [Route("GetBrandsForStaff")]
        public async Task<IEnumerable<GetBrandForStaff>> GetBrandsForStaff([FromQuery] string sortOrder, [FromQuery] string searchString)
        {
            string sortQuery = "";
            string groupByQuery = " GROUP BY b.BrandID, b.Name, b.ContactEmail, sale.TotalSale ";
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
                    groupByQuery += ", TotalProduct";
                    break;
                case "totalproduct_desc":
                    sortQuery = "ORDER BY TotalProduct DESC";
                    groupByQuery += ", TotalProduct";
                    break;
                case "totalsale":
                    sortQuery = "ORDER BY sale.TotalSale";
                    break;
                case "totalsale_desc":
                    sortQuery = "ORDER BY sale.TotalSale DESC";
                    break;
                case "rate":
                    sortQuery = "ORDER BY Rate";
                    groupByQuery += ", Rate";
                    break;
                case "rate_desc":
                    sortQuery = "ORDER BY Rate DESC";
                    groupByQuery += ", Rate";
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
                    string query = "SELECT b.BrandID, b.Name, b.ContactEmail, " +
                                   "COUNT(p.ProductID) AS TotalProduct, AVG(p.Rating) AS Rate, sale.TotalSale " +
                                   "FROM Brand b JOIN Product p ON b.BrandID = p.BrandID JOIN (" +
                                   "SELECT b.BrandID, SUM(od.Price) AS TotalSale " +
                                   "FROM OrderDetail od JOIN Product p ON od.ProductID = p.ProductID " +
                                   "JOIN Brand b ON p.BrandID = b.BrandID GROUP BY b.BrandID" +
                                   ") sale ON b.BrandID = sale.BrandID " +
                                   whereQuery + groupByQuery + " " + sortQuery + " ";
                    
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
                                TotalProducts = reader.GetInt32(3),
                                Rate = Math.Round(reader.GetDouble(4), 1),
                                TotalSales = Math.Round(reader.GetDouble(5), 3)
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
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBrand([FromRoute] int id, [FromBody] Brand brand)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != brand.BrandId)
            {
                return BadRequest();
            }

            _context.Entry(brand).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BrandExists(id))
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

        // POST: api/Brand
        [HttpPost]
        public async Task<IActionResult> PostBrand([FromBody] Brand brand)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Brand.Add(brand);
            if (await _context.SaveChangesAsync() > 0)
            {
                brand.Slug = "/b" + brand.BrandId + "/" + brand.Slug;
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