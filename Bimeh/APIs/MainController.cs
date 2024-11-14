using Bimeh.Domain.DTOs;
using Bimeh.Domain.Entities;
using Bimeh.Rpositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bimeh.APIs
{
    [Route("api/[controller]")]
    [ApiController]
    public class MainController : ControllerBase
    {   //نمونه کار تدبیر
        private readonly IManage _manage;
        private readonly BimehContext _context;
        public MainController(IManage manage,BimehContext context)
        {
            _manage = manage;
            _context = context;
        }

        [HttpPost("login")]
        public async Task<IActionResult> login([FromBody] UserDTO user)
        {
            var Res = HttpContext.Response;

            if (ModelState.IsValid)
            {
                var IsLoginOrNot = await _manage.Login(user);
                if (IsLoginOrNot)
                {
                    
                    return Ok(new { Message = "شما با موفقیت ثبت نام کردید" });
                }
                     
                else
                    return BadRequest(new { Message = "مشکلی در ثبت نام به وجود آمد" });
            }
            return BadRequest(new { Message = "ورودی ها معتبر نیستند" });
        }
        [HttpPost("Request")]
        public async Task<IActionResult> RequestAccepter(RequestDTO request)
        {   
           

            for (var i = 0; i < request.InsuranceCoverage.Length; i++)
            {
                var coverage = request.InsuranceCoverage[i]; 

                var RequestInsurance = await _context.InsuranceCoverages.FirstOrDefaultAsync(x => x.Title == coverage);

                if (RequestInsurance == null)
                {
                    return BadRequest(new { Message = "پوشش بیمه‌ای پیدا نشد" });
                }

                if (request.Price > RequestInsurance.MaxPrice || request.Price < RequestInsurance.MinPrice)
                {
                    return BadRequest(new { Message = $"مقدار واریزی برای پوشش {coverage} باید بین {RequestInsurance.MinPrice} و {RequestInsurance.MaxPrice} باشد" });
                }
            }


            var Token = HttpContext.Request.Cookies["UserToken"];
            if(Token == null)
            {
                return BadRequest(new { Message = "وارد حساب کاربری خود شوید" });
            }
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Token == Token);
            if(user == null)
            {
                return NotFound();
            }
            var UserId = user.Id;
            var Result =  await _manage.AcceptRequest(request, UserId);
            if (Result)
            {
                return Ok(new {Message = "درخواست/درخواست های شما با موفقیت ثبت شد"});
            }
            return BadRequest(new {Message = "مشکلی در ثبت درخواست به وجود آمد"});
            
        }
        [HttpGet("GetAllRequest")]
        public async Task<IActionResult> GetAllRequest()
        {
            var Token = HttpContext.Request.Cookies["UserToken"];

            if (Token == null)
            {
                return NotFound(new { Message = "توکن یافت نشد. لطفاً وارد حساب کاربری خود شوید." });
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Token == Token);

            if (user == null)
            {
                return NotFound(new { Message = "کاربری با این شناسه یافت نشد" });
            }

            var requests = await _context.Requests.Where(x => x.UserId == user.Id).ToListAsync();

            if (requests.Count == 0)
            {
                return NotFound(new { Message = "هیچ درخواستی با این شناسه وجود ندارد" });
            }

            foreach (var request in requests)
            {
                var insuranceCoverages = await _context.InsuranceCoverages
                                                        .Where(x => x.RequestId == request.Id)
                                                        .ToListAsync();
                decimal totalPremium = 0;

                foreach (var coverage in insuranceCoverages)
                {
                    decimal coveragePremium = 0;

                    if (coverage.Type == "جراحی")
                    {
                        coveragePremium = coverage.Price * 0.0052M;
                    }
                    else if (coverage.Type == "دندانپزشکی")
                    {
                        coveragePremium = coverage.Price * 0.0042M;
                    }
                    else if (coverage.Type == "بستری")
                    {
                        coveragePremium = coverage.Price * 0.005M;
                    }

                    totalPremium += coveragePremium;
                }

                request.PremiumAmount = totalPremium;

                _context.Requests.Update(request);
            }

            await _context.SaveChangesAsync();

            return Ok(requests);
        }

        [HttpGet("GetSelected")]
        public async Task<IActionResult> GetSelected(int TrackingNumber)
        {
            var SelectedRequest =  await _context.Requests.FirstOrDefaultAsync(x =>  x.TrackingNumber == TrackingNumber);
            if(SelectedRequest == null)
            {
                return NotFound(new {Message = "هیچ درخواستی با این شماره پیگیری یافت نشد"});
            }
            return Ok(new { Request = SelectedRequest });
        }

      
    }
}
