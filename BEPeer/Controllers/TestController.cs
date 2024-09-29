using DAL;
using Microsoft.AspNetCore.Mvc;

namespace BEPeer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private readonly P2plandingContext _context;

        public TestController(P2plandingContext context)
        {
            _context = context;
        }

        [HttpGet("testdb")]
        public IActionResult TestDatabase()
        {
            try
            {
                // Coba untuk mengakses database
                bool canConnect = _context.Database.CanConnect();

                if (canConnect)
                {
                    return Ok("Koneksi ke database berhasil!");
                }
                else
                {
                    return BadRequest("Tidak dapat terhubung ke database.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error saat menguji koneksi database: {ex.Message}");
            }
        }
    }

}
