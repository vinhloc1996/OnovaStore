using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using OnovaApi.Data;
using OnovaApi.DTOs;
using OnovaApi.Models.DatabaseModels;

namespace OnovaApi.Controllers
{
    [Produces("application/json")]
    [Route("api/Category")]
    [Authorize(Policy = "Staff Only")]
    public class CategoryController : Controller
    {
        private readonly OnovaContext _context;
        private const string ImageUrl = "http://res.cloudinary.com/vinhloc1996/image/upload/";

        public CategoryController(OnovaContext context)
        {
            _context = context;
        }

        [HttpGet("GetCategoriesForIndexPage")]
        [AllowAnonymous]
        public IActionResult GetCategoriesForIndexPage()
        {
            var debug = _context.Category.Where(c => c.TotalProduct > 0 && c.IsHide != true)
                .Select(c => new
                {
                    c.CategoryId, c.Name, c.Slug, Product = c.Product.Select(p => new
                    {
                        p.Name, p.ProductId, p.Slug,
                        ProductThumbImage = ImageUrl + p.ProductThumbImage, p.DisplayPrice
                    })
                })
                .ToList();

            return Json(debug);
        }

        [HttpGet]
        [Route("GetCategoriesForHeader")]
        [AllowAnonymous]
        public IActionResult GetCategoriesForHeader()
        {
            var debug = _context.Category
                .Where(c => c.TotalProduct > 0 && c.IsHide != true && c.ParentCategoryId == null).Select(c => new {c.CategoryId, c.Name, c.Slug}).ToList();
            var json = Json(debug);
            return json;
        }

        [HttpGet]
        [Route("CheckCategoryExisted")]
        public async Task<IActionResult> CheckCategoryExisted([FromQuery] int id)
        {
            return await _context.Category.FindAsync(id) != null ? StatusCode(200) : StatusCode(404);
        }

        [HttpGet]
        [Route("GetCategories")]
        public IEnumerable<GetCategoriesDTO> GetCategories()
        {
            var categories =
                _context.Category.Select(x => new GetCategoriesDTO {CategoryId = x.CategoryId, CategoryName = x.Name});

            return categories;
        }

        [HttpGet]
        [Route("GetProductsForCategory")]
        [AllowAnonymous]
        public IActionResult GetProductsForCategory([FromQuery] int id, [FromQuery] string sortOrder)
        {
            var categories = new object();

            switch (sortOrder)
            {
                case "name_desc":
                    categories = _context.Category.Where(c => c.CategoryId == id && c.IsHide != true).Select(c => new
                    {
                        c.CategoryId,
                        c.CategoryImage,
                        c.Name,
                        TotalProduct = c.Product.Count,
                        c.Slug,
                        Products = c.Product.Where(p => p.IsHide != true).Select(p => new
                        {
                            p.ProductId,
                            p.DisplayPrice,
                            p.Name,
                            p.Slug,
                            p.ProductThumbImage,
                            BrandName = p.Brand.Name,
                            BranSlug = p.Brand.Slug,
                            p.ProductStatus.StatusCode
                        }).OrderByDescending(p => p.Name).ToList()
                    }).FirstOrDefault();
                    break;
                case "price":
                    categories = _context.Category.Where(c => c.CategoryId == id && c.IsHide != true).Select(c => new
                    {
                        c.CategoryId,
                        c.CategoryImage,
                        c.Name,
                        TotalProduct = c.Product.Count,
                        c.Slug,
                        Products = c.Product.Where(p => p.IsHide != true).Select(p => new
                        {
                            p.ProductId,
                            p.DisplayPrice,
                            p.Name,
                            p.Slug,
                            p.ProductThumbImage,
                            BrandName = p.Brand.Name,
                            BranSlug = p.Brand.Slug,
                            p.ProductStatus.StatusCode
                        }).OrderBy(p => p.DisplayPrice).ToList()
                    }).FirstOrDefault();
                    break;

                case "price_desc":
                    categories = _context.Category.Where(c => c.CategoryId == id && c.IsHide != true).Select(c => new
                    {
                        c.CategoryId,
                        c.CategoryImage,
                        c.Name,
                        TotalProduct = c.Product.Count,
                        c.Slug,
                        Products = c.Product.Where(p => p.IsHide != true).Select(p => new
                        {
                            p.ProductId,
                            p.DisplayPrice,
                            p.Name,
                            p.Slug,
                            p.ProductThumbImage,
                            BrandName = p.Brand.Name,
                            BranSlug = p.Brand.Slug,
                            p.ProductStatus.StatusCode
                        }).OrderByDescending(p => p.DisplayPrice).ToList()
                    }).FirstOrDefault();
                    break;
                default:
                    categories = _context.Category.Where(c => c.CategoryId == id && c.IsHide != true).Select(c => new
                    {
                        c.CategoryId,
                        c.CategoryImage,
                        c.Name,
                        TotalProduct = c.Product.Count,
                        c.Slug,
                        Products = c.Product.Where(p => p.IsHide != true).Select(p => new
                        {
                            p.ProductId,
                            p.DisplayPrice,
                            p.Name,
                            p.Slug,
                            p.ProductThumbImage,
                            BrandName = p.Brand.Name,
                            BranSlug = p.Brand.Slug,
                            p.ProductStatus.StatusCode
                        }).OrderBy(p => p.Name).ToList()
                    }).FirstOrDefault();
                    break;
            }
                

            if (categories == null)
            {
                return Json(new
                {
                    Status = "Failed",
                    Message = "Cannot find the category"
                });
            }

            return Json(new
            {
                Status = "Success",
                Message = "Category products",
                categories
            });
        }

        [HttpGet]
        [Route("GetCategoriesForStaff")]
        public async Task<IEnumerable<GetCategoryForStaff>> GetCategoriesForStaff([FromQuery] string sortOrder, [FromQuery] string searchString)
        {
            string sortQuery = "";
            string whereQuery = string.IsNullOrEmpty(searchString) ? "" : " WHERE LOWER(c.Name) LIKE @searchString";

            sortOrder = string.IsNullOrEmpty(sortOrder) ? "id" : sortOrder.Trim().ToLower();

            switch (sortOrder)
            {
                case "id_desc":
                    sortQuery = "ORDER BY c.CategoryID DESC";
                    break;
                case "name":
                    sortQuery = "ORDER BY c.Name";
                    break;
                case "name_desc":
                    sortQuery = "ORDER BY c.Name DESC";
                    break;
                case "totalproduct":
                    sortQuery = "ORDER BY c.TotalProduct";
                    break;
                case "totalproduct_desc":
                    sortQuery = "ORDER BY c.TotalProduct DESC";
                    break;
                default:
                    sortQuery = "ORDER BY c.CategoryID";
                    break;
            }

            var conn = _context.Database.GetDbConnection();
            var categories = new List<GetCategoryForStaff>();
            try
            {
                await conn.OpenAsync();
                using (var command = conn.CreateCommand())
                {
                    string query = "SELECT c.CategoryID, c.Name, c.TotalProduct " +
                                   "FROM Category c " +
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
                            var row = new GetCategoryForStaff()
                            {
                                CategoryId = reader.GetInt32(0),
                                CategoryName = reader.GetString(1),
                                TotalProduct = reader.GetInt32(2),
                            };

                            categories.Add(row);
                        }
                    }

                }
            }
            finally
            {
                conn.Close();
            }

            return categories;
        }


        // GET: api/Category/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategory([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var category = await _context.Category.SingleOrDefaultAsync(m => m.CategoryId == id);

            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        // PUT: api/Category/5
        [HttpPut]
        public async Task<IActionResult> PutCategory([FromBody] Category category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Entry(category).State = EntityState.Modified;

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

        // POST: api/Category
        [HttpPost]
        public async Task<IActionResult> PostCategory([FromBody] Category category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            category.IsHide = false;
            _context.Category.Add(category);

            if (await _context.SaveChangesAsync() > 0)
            {
                //                category.Slug = "/category" + "/c" + category.CategoryId + "/" + category.Slug;
                category.Slug = "/category/" + category.Slug + "-c" + category.CategoryId;
                _context.Entry(category).State = EntityState.Modified;

                if (await _context.SaveChangesAsync() > 0)
                {
                    return StatusCode(201);
                }
            }

            return StatusCode(424);
        }

        // DELETE: api/Category/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var category = await _context.Category.SingleOrDefaultAsync(m => m.CategoryId == id);
            if (category == null)
            {
                return NotFound();
            }

            _context.Category.Remove(category);
            await _context.SaveChangesAsync();

            return Ok(category);
        }

        private bool CategoryExists(int id)
        {
            return _context.Category.Any(e => e.CategoryId == id);
        }
    }
}