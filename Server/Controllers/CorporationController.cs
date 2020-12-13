using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;



namespace ForStock.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CorporationController : ControllerBase
    {
        private readonly ILogger<CorporationController> logger;

        public CorporationController(ILogger<CorporationController> logger)
        {
            this.logger = logger;
        }

        [HttpGet("getcorpinfo/{stockcode}")]
        public async Task<User> GetProfile(int userId){
            return await  _context.Users.Where(u => u.UserId == userId).FirstOrDefaultAsync();
        }
        
    
    }
}
