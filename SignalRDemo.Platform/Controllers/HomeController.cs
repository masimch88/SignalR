using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SignalRDemo.Platform.Data;
using SignalRDemo.Platform.Hubs;
using SignalRDemo.Platform.Models;
using System.Diagnostics;
using System.Security.Claims;

namespace SignalRDemo.Platform.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HomeController(
            ILogger<HomeController> logger, 
            ApplicationDbContext applicationDbContext,
            IHubContext<ChatHub> hubContext,
            IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _applicationDbContext = applicationDbContext;
            _hubContext = hubContext;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IActionResult> Index()
        {            
            IdentityUser[] users = await _applicationDbContext.Users.ToArrayAsync();
            return View(users);
        }

        public async Task<IActionResult> Sender()
        {
            IdentityUser[] users = await _applicationDbContext.Users.ToArrayAsync();
            return View(users);
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage([FromBody]MessageRequest request)
        {
           // HttpContext httpContext = _httpContextAccessor.HttpContext;
            //await _hubContext.Clients.All.SendAsync("ReceiveMessage", $"[from: {request.Email}] ======> {request.Message}");.

            
            await _hubContext.Clients.User(request.UserId).SendAsync("ReceiveMessage", request.Message);                     

            return Ok();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }


    public class MessageRequest
    {
        public string UserId { get; set; } = null!;
        public string Message { get; set; } = null!;
    }
}