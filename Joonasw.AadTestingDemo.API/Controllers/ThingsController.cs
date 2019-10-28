using Joonasw.AadTestingDemo.API.Authorization;
using Joonasw.AadTestingDemo.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Joonasw.AadTestingDemo.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ThingsController : ControllerBase
    {
        /// <summary>
        /// Gets all things
        /// </summary>
        [Authorize(Actions.ReadThings)]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ThingModel[]))]
        public IActionResult Get()
        {
            // Fake data
            var data = new[]
            {
                new ThingModel
                {
                    One = "1",
                    Two = "2"
                },
                new ThingModel
                {
                    One = "one",
                    Two = "two"
                }
            };
            return Ok(data);
        }
    }
}
