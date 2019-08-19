using DatingApp.API.Data;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers
{
    public class DemoController : ControllerBase
    {
        private readonly DataContext _context;
        public DemoController(DataContext context)
        {
            _context = context;
        }

        public IActionResult GetUser(){
            
            return Ok();
        }
    }
}