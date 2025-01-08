using System.Diagnostics;
using LiteDBTest.Models;
using Microsoft.AspNetCore.Mvc;

namespace LiteDBTest.Controllers
{
    public class ImageController : Controller
    {
        private readonly ILogger<ImageController> _logger;

        public ImageController(ILogger<ImageController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Route("image/{file}")]

        public IActionResult File(string file)
        {
            var pic = LiteDBLib.DB.Read(file);

            return pic == null ? Content("") : File(LiteDBLib.DB.Read(pic), "image/jpeg");
        }
    }
}
