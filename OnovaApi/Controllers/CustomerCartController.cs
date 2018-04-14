﻿using System;
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
    [Route("api/CustomerCart")]
    public class CustomerCartController : Controller
    {
        private readonly OnovaContext _context;

        public CustomerCartController(OnovaContext context)
        {
            _context = context;
        }

        // GET: api/CustomerCart
        [HttpGet]
        public IEnumerable<CustomerCart> GetCustomerCart()
        {
            return _context.CustomerCart;
        }

        // GET: api/CustomerCart/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomerCart([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var customerCart = await _context.CustomerCart.SingleOrDefaultAsync(m => m.CustomerCartId == id);

            if (customerCart == null)
            {
                return NotFound();
            }

            return Ok(customerCart);
        }

        // PUT: api/CustomerCart/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomerCart([FromRoute] string id, [FromBody] CustomerCart customerCart)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != customerCart.CustomerCartId)
            {
                return BadRequest();
            }

            _context.Entry(customerCart).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerCartExists(id))
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

        // POST: api/CustomerCart
        [HttpPost]
        public async Task<IActionResult> PostCustomerCart([FromBody] CustomerCart customerCart)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.CustomerCart.Add(customerCart);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (CustomerCartExists(customerCart.CustomerCartId))
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetCustomerCart", new { id = customerCart.CustomerCartId }, customerCart);
        }

        // DELETE: api/CustomerCart/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomerCart([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var customerCart = await _context.CustomerCart.SingleOrDefaultAsync(m => m.CustomerCartId == id);
            if (customerCart == null)
            {
                return NotFound();
            }

            _context.CustomerCart.Remove(customerCart);
            await _context.SaveChangesAsync();

            return Ok(customerCart);
        }

        private bool CustomerCartExists(string id)
        {
            return _context.CustomerCart.Any(e => e.CustomerCartId == id);
        }
    }
}