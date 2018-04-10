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
    [Route("api/Promotion")]
    public class PromotionController : Controller
    {
        private readonly OnovaContext _context;

        public PromotionController(OnovaContext context)
        {
            _context = context;
        }

        // GET: api/Promotion
        [HttpGet]
        public IEnumerable<Promotion> GetPromotion()
        {
            return _context.Promotion;
        }

        [HttpGet]
        [Route("GetPromotionsForStaff")]
        public async Task<IEnumerable<GetPromotionForStaff>> GetPromotionsForStaff([FromQuery] string sortOrder, [FromQuery] string searchString)
        {
            string sortQuery = "";
            string whereQuery = string.IsNullOrEmpty(searchString) ? "" : " WHERE LOWER(p.PromotionCode) LIKE @searchString OR LOWER(p.TargetApply) LIKE @searchString OR LOWER(p.PercentOff) LIKE @searchString OR LOWER(p.PromotionStatus) LIKE @searchString ";

            sortOrder = string.IsNullOrEmpty(sortOrder) ? "id" : sortOrder.Trim().ToLower();

            switch (sortOrder)
            {
                case "id_desc":
                    sortQuery = "ORDER BY p.PromotionID DESC";
                    break;
                case "code":
                    sortQuery = "ORDER BY p.PromotionCode";
                    break;
                case "code_desc":
                    sortQuery = "ORDER BY p.PromotionCode DESC";
                    break;
                case "targetapply":
                    sortQuery = "ORDER BY p.TargetApply";
                    break;
                case "targetapply_desc":
                    sortQuery = "ORDER BY p.TargetApply DESC";
                    break;
                case "discount":
                    sortQuery = "ORDER BY p.PercentOff";
                    break;
                case "discount_desc":
                    sortQuery = "ORDER BY p.PercentOff DESC";
                    break;
                case "status":
                    sortQuery = "ORDER BY p.PromotionStatus";
                    break;
                case "status_desc":
                    sortQuery = "ORDER BY p.PromotionStatus DESC";
                    break;
                default:
                    sortQuery = "ORDER BY p.PromotionID";
                    break;
            }

            var conn = _context.Database.GetDbConnection();
            var promotions = new List<GetPromotionForStaff>();
            try
            {
                await conn.OpenAsync();
                using (var command = conn.CreateCommand())
                {
                    string query = "SELECT p.PromotionID, p.PromotionCode, p.PromotionStatus, p.TargetApply, p.PercentOff " +
                                   "FROM Promotion p " +
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
                            var row = new GetPromotionForStaff
                            {
                                PromotionId = reader.GetInt32(0),
                                PromotionCode = reader.GetString(1),
                                Status = reader.GetString(2),
                                TargetApply = reader.GetString(3),
                                Discount = reader.GetDecimal(4)
                            };

                            promotions.Add(row);
                        }
                    }

                }
            }
            finally
            {
                conn.Close();
            }

            return promotions;
        }

        // GET: api/Promotion/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPromotion([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var promotion = await _context.Promotion.SingleOrDefaultAsync(m => m.PromotionId == id);

            if (promotion == null)
            {
                return NotFound();
            }

            return Ok(promotion);
        }

        // PUT: api/Promotion/5
        [HttpPut]
        public async Task<IActionResult> PutPromotion([FromBody] Promotion promotion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Entry(promotion).State = EntityState.Modified;

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

        // POST: api/Promotion
        [HttpPost]
        public async Task<IActionResult> PostPromotion([FromBody] AddPromotionDTO promotion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var promo = new Promotion
            {
                PromotionName = promotion.PromotionName,
                PromotionStatus = promotion.PromotionStatus,
                PromotionDescription = promotion.PromotionDescription,
                PromotionCode = promotion.PromotionCode,
                TargetApply = promotion.TargetApply,
                StartDate = promotion.StartDate,
                EndDate = promotion.EndDate,
                PercentOff = promotion.PercentOff
            };

            _context.Promotion.Add(promo);

            if (await _context.SaveChangesAsync() > 0)
            {
                if (promotion.TargetApply == "Brand")
                {
                    var promoBrand = new PromotionBrand
                    {
                        BrandId = promotion.PromotionBrand,
                        PromotionId = promo.PromotionId
                    };

                    _context.PromotionBrand.Add(promoBrand);
                }

                if (promotion.TargetApply == "Category")
                {
                    var promoCategory = new PromotionCategory()
                    {
                        CategoryId = promotion.PromotionCategory,
                        PromotionId = promo.PromotionId
                    };

                    _context.PromotionCategory.Add(promoCategory);
                }

                if (await _context.SaveChangesAsync() > 0)
                {
                    return StatusCode(201);
                }
            }

            return StatusCode(424);
        }

        // DELETE: api/Promotion/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePromotion([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var promotion = await _context.Promotion.SingleOrDefaultAsync(m => m.PromotionId == id);
            if (promotion == null)
            {
                return NotFound();
            }

            _context.Promotion.Remove(promotion);
            await _context.SaveChangesAsync();

            return Ok(promotion);
        }

        private bool PromotionExists(int id)
        {
            return _context.Promotion.Any(e => e.PromotionId == id);
        }
    }
}