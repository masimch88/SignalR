using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SignalRDemo.Platform.Data;
using SignalRDemo.Platform.Hubs;

namespace SignalRDemo.Platform
{
    public class UserForMessage
    {
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
    }

    public static class MessageSender
    {
        private static System.Timers.Timer _timer = null!;
        private static UserForMessage[] _users = null!;
        private static int _index = -1;
        private static IHubContext<ChatHub> _hubContext = null!;

        public static void Start(IServiceProvider services)
        {
            _timer = new System.Timers.Timer(1000);
            _timer.Stop();
            _timer.Elapsed += _timer_Elapsed;

            _hubContext = services.GetRequiredService<IHubContext<ChatHub>>();

            var opt = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlServer("Server=(local);Database=SignalRDemo_Platform;Trusted_Connection=True;MultipleActiveResultSets=true");

            var db = new ApplicationDbContext(opt.Options);

            _users = db.Users
                .OrderBy(u => u.NormalizedUserName)
                .Select(u => new UserForMessage 
                { 
                    Id = u.Id, 
                    Name = u.UserName 
                })
                .ToArray();

            _timer.Start();
        }

        private static async void _timer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            _index++;

            UserForMessage u = _users[_index % _users.Length];

            await _hubContext.Clients.User(u.Id).SendAsync("ReceiveMessage", $"Hello {u.Name}, message number {_index} is chosen for you.");
        }
    }
}
