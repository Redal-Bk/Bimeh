﻿using Bimeh.Domain.Entities;
using Bimeh.Domain.DTOs;
using Bimeh.Rpositories;
using Microsoft.EntityFrameworkCore;
namespace Bimeh.Infrastructur
{
    public class Manage : IManage
    {
        private readonly BimehContext _context;
        private readonly IHttpContextAccessor _contextAccessor;
        public Manage(BimehContext context, IHttpContextAccessor contextAccessor)
        {
            _context = context;
            _contextAccessor=contextAccessor;
        }
        public async Task<bool> Login(UserDTO user)
        {
            var res = _contextAccessor.HttpContext.Response;
            string token = Guid.NewGuid().ToString();
            var result = await _context.Users.FirstOrDefaultAsync(us => us.Username == user.Username);
            if (result == null)
            {
                if (string.IsNullOrEmpty(user.Username) || string.IsNullOrEmpty(user.Password))
                {
                    return false;
                }
                var User = new User
                {
                    Username = user.Username,
                    Password = user.Password,
                    Token = token
                };
                res.Cookies.Append("UserToken", token, new CookieOptions
                {
                    Path = "/",
                    Expires = DateTime.UtcNow.AddYears(1)
                });
                _context.Users.Add(User);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
        public async Task<bool> AcceptRequest(RequestDTO request, int userId)
        {
            // بررسی ورودی‌ها
            if (string.IsNullOrEmpty(request.TitleRequst) || request.InsuranceCoverage == null || request.InsuranceCoverage.Length == 0)
            {
                return false;
            }

        
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                return false;
            }

            
            var requestInsurance = await _context.Requests.FirstOrDefaultAsync(x => x.RequestTitle == request.TitleRequst);
            if (requestInsurance != null)
            {
               
                for (var i = 0; i < request.InsuranceCoverage.Length; i++)
                {
                    var coverageTitle = request.InsuranceCoverage[i];
                    var insuranceCoverage = await _context.InsuranceCoverages.FirstOrDefaultAsync(x => x.Title == coverageTitle);

                    if (insuranceCoverage == null)
                    {
                        
                        return false;
                    }

                    
                    var Request = new Request
                    {
                        RequestTitle = request.TitleRequst,
                        DateCreated = DateTime.Now,
                        InsuranceCoverageId = insuranceCoverage.Id,  
                        UserId = user.Id,
                        Price = request.Price
                    };

                    
                    _context.Requests.Add(Request);
                    await _context.SaveChangesAsync();
                }

                return true;
            }

            return false;
        }

      
    }
}
