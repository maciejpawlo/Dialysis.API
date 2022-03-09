using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Dialysis.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExaminationsController : ControllerBase
    {
        public ExaminationsController()
        {

        }

        [HttpGet]
        public async Task<IActionResult> GetExaminations()
        {
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> CreateExaminations(int id)
        {
            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteExaminations(int id)
        {
            return Ok();
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> EditExaminations(int id)
        {
            return Ok();
        }
    }
}
