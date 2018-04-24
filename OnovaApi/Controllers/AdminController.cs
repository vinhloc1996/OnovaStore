using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnovaApi.Data;
using OnovaApi.Models.DatabaseModels;
using OnovaApi.Models.IdentityModels;

namespace OnovaApi.Controllers
{
    [Produces("application/json")]
    [Route("api/Admin")]
    public class AdminController : Controller
    {
        private readonly OnovaContext _context;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public AdminController(OnovaContext context, RoleManager<ApplicationRole> roleManager)
        {
            _context = context;
            _roleManager = roleManager;
        }

        [HttpGet]
        [Route("GetRoles")]
        public IEnumerable GetRoles()
        {
            var roles = _context.Roles.Where(r => r.Name != "Administrator").Select(r => new
            {
                r.Name
            });

            return roles.ToList();
        }

        [HttpGet]
        [Route("GetStaffsForAdmin")]
        public IEnumerable GetStaffsForAdmin([FromQuery] string sortOrder, [FromQuery] string searchString)
        {

            var roleAdminId = _context.Roles.FirstOrDefault(r => r.Name == "Administrator").Id;
            var listStaff = _context.UserRoles.Where(u => u.RoleId != roleAdminId).ToList();

            var staffs = _context.Staff.Where(u => listStaff.Any(s => s.UserId == u.StaffId)).Select(s => new
            {
                s.StaffId,
                s.ApplicationUser.FullName,
                s.AddDate,
                s.Salary,
                s.ApplicationUser.Email,
                role = _context.Roles
                    .FirstOrDefault(c => c.Id == listStaff.FirstOrDefault(r => r.UserId == s.StaffId).RoleId).Name
            });

            if (!string.IsNullOrEmpty(searchString))
                staffs = staffs.Where(c => EF.Functions.Like(c.FullName.ToLower(), "%" + searchString.ToLower() + "%") || EF.Functions.Like(c.Email.ToLower(), "%" + searchString.ToLower() + "%") || EF.Functions.Like(c.role.ToLower(), "%" + searchString.ToLower() + "%"));

            switch (sortOrder)
            {
                case "name":
                    staffs = staffs.OrderBy(c => c.FullName);
                    break;
                case "name_desc":
                    staffs = staffs.OrderByDescending(c => c.FullName);
                    break;
                case "adddate":
                    staffs = staffs.OrderBy(c => c.AddDate);
                    break;
                case "adddate_desc":
                    staffs = staffs.OrderByDescending(c => c.AddDate);
                    break;
                case "salary":
                    staffs = staffs.OrderBy(c => c.Salary);
                    break;
                case "salary_desc":
                    staffs = staffs.OrderByDescending(c => c.Salary);
                    break;
                case "email":
                    staffs = staffs.OrderBy(c => c.Email);
                    break;
                case "email_desc":
                    staffs = staffs.OrderByDescending(c => c.Email);
                    break;
                case "role":
                    staffs = staffs.OrderBy(c => c.role);
                    break;
                case "role_desc":
                    staffs = staffs.OrderByDescending(c => c.role);
                    break;
                default:
                    staffs = staffs.OrderBy(c => c.FullName);
                    break;
            }

            return staffs.ToList();
        }

        //admin add staff, reset password staff, edit staff info
    }
}