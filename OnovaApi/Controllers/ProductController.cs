using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
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
    [Route("api/Product")]
    public class ProductController : Controller
    {
        private readonly OnovaContext _context;

        public ProductController(OnovaContext context)
        {
            _context = context;
        }

        // GET: api/Product
        [HttpGet]
        public IEnumerable<Product> GetProduct()
        {
            return _context.Product;
        }

        // GET: api/Product/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var product = await _context.Product.SingleOrDefaultAsync(m => m.ProductId == id);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        [HttpGet]
        [Route("GetProductsForStaff")]
        public async Task<IEnumerable<GetProductForStaff>> GetProductsForStaff([FromQuery] string sortOrder, [FromQuery] string searchString)
        {
            string sortQuery = "";
            string whereQuery = string.IsNullOrEmpty(searchString) ? "" : " WHERE LOWER(p.Name) LIKE @searchString OR LOWER(ps.StatusName) LIKE @searchString ";
            string groupByQuery = " GROUP BY p.ProductID, P.Name , p.Rating, p.WishCounting, ps.StatusName ";

            sortOrder = string.IsNullOrEmpty(sortOrder) ? "id" : sortOrder.Trim().ToLower();

            switch (sortOrder)
            {
                case "id_desc":
                    sortQuery = "ORDER BY p.ProductID DESC";
                    break;
                case "name":
                    sortQuery = "ORDER BY p.Name";
                    break;
                case "name_desc":
                    sortQuery = "ORDER BY p.Name DESC";
                    break;
                case "sales":
                    sortQuery = "ORDER BY Sales";
                    break;
                case "sales_desc":
                    sortQuery = "ORDER BY Sales DESC";
                    break;
                case "status":
                    sortQuery = "ORDER BY ps.StatusName";
                    break;
                case "status_desc":
                    sortQuery = "ORDER BY ps.StatusName DESC";
                    break;
                case "numberorder":
                    sortQuery = "ORDER BY NumberOrder";
                    break;
                case "numberorder_desc":
                    sortQuery = "ORDER BY NumberOrder DESC";
                    break;
                case "rating":
                    sortQuery = "ORDER BY p.Rating";
                    break;
                case "rating_desc":
                    sortQuery = "ORDER BY p.Rating DESC";
                    break;
                case "wishcounting":
                    sortQuery = "ORDER BY p.WishCounting";
                    break;
                case "wishcounting_desc":
                    sortQuery = "ORDER BY p.WishCounting DESC";
                    break;
                default:
                    sortQuery = "ORDER BY p.ProductID";
                    break;
            }

            var conn = _context.Database.GetDbConnection();
            var products = new List<GetProductForStaff>();
            try
            {
                await conn.OpenAsync();
                using (var command = conn.CreateCommand())
                {
                    string query = "SELECT p.ProductID, p.Name, ps.StatusName, " +
                                   "ISNULL(SUM(od.DisplayPrice), 0) as Sales, COUNT(DISTINCT o.OrderID) as NumberOrder, " +
                                   "p.Rating, p.WishCounting " +
                                   "FROM Product p LEFT JOIN OrderDetail od " +
                                   "ON p.ProductID = od.ProductID LEFT JOIN [Order] o " +
                                   "ON o.OrderID = od.OrderID LEFT JOIN ProductStatus ps " +
                                   "ON p.ProductStatusID = ps.ProductStatusID " +
                                   whereQuery + " " +
                                   groupByQuery + " " + sortQuery + " ";

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
                            var row = new GetProductForStaff()
                            {
                                ProductId = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                ProductStatus = reader.GetString(2),
                                Sales = reader.GetDouble(3),
                                NumberOrder = reader.GetInt32(4),
                                Rating = reader.GetFloat(5),
                                WishCounting = reader.GetInt32(6)
                            };

                            products.Add(row);
                        }
                    }

                }
            }
            finally
            {
                conn.Close();
            }

            return products;
        }

        // PUT: api/Product/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct([FromRoute] int id, [FromBody] Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != product.ProductId)
            {
                return BadRequest();
            }

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
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

        // POST: api/Product
        [HttpPost]
        public async Task<IActionResult> PostProduct([FromBody] Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Product.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProduct", new { id = product.ProductId }, product);
        }

        // DELETE: api/Product/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var product = await _context.Product.SingleOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Product.Remove(product);
            await _context.SaveChangesAsync();

            return Ok(product);
        }

        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.ProductId == id);
        }
    }
}