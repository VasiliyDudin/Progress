using Entity;
using Entity.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.Entity;
using View.Models.In;
using View.Models.Out;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UserStatistic.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserStatisticsController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public UserStatisticsController(DatabaseContext context)
        {
            _context = context;
        }


        // GET: api/<GetTop100>
        [HttpGet]
        public async Task<ActionResult<List<OutUserStatistics>>> GetTop100()
        {
            var userTop = _context.UserStatistics.AsNoTracking().Take(100).OrderByDescending(f => f.Rating).ToList();
            if (userTop == null)
                return NotFound();

            return Ok(userTop);
        }

        // GET api/<Get>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OutUserStatistics>> Get(long id)
        {
            var userStatistic = await _context.UserStatistics.AsNoTracking().FirstOrDefaultAsync(f => f.UserId == id);
            if (userStatistic == null)
                return NotFound();

            var result = new OutUserStatistics
            {
                UserId = userStatistic.UserId,
                Level = userStatistic.Level,
                Rating = userStatistic.Rating,
                IsPrivileged = userStatistic.IsPrivileged,
                GameCount = userStatistic.GameCount,
                WinGames = userStatistic.WinGames,
                LossGames = userStatistic.LossGames
            };

            return Ok(result);
        }

        // POST api/<UserStatisticsController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<UpdatePrivileged>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePrivileged(long id, bool privileg)
        {
            var userPrivileged = await _context.UserStatistics.FirstOrDefaultAsync(f => f.UserId == id);
            if (userPrivileged == null)
                return new NotFoundResult();

            userPrivileged.IsPrivileged = privileg;

            await _context.SaveChangesAsync();


            return Ok();
        }
    }
}
