using Microsoft.AspNetCore.Mvc;
using PaperNest_API.Models;
using PaperNest_API.Repository;

namespace PaperNest_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
            [HttpPost("register")]
            public IActionResult Register([FromBody] User user)
            {
                var user_obj = new User
                {
                    Name = user.Name,
                    Email = user.Email,
                    Password = user.Password,
                    Username = user.Username,
                    Role = "Mahasiswa" ?? "Dosen"
                };

                UserRepository.userRepository.Add(user);

                return Ok(new
                {
                    message = "Berhasil register user dengan id" + user.Id
                });
            }

        [HttpGet("login")]
        public IActionResult Login(string username, string password)
        {
            var user = UserRepository.userRepository.FirstOrDefault(u => u.Username.Equals(username) && u.Password.Equals(password));

            if (user == null)
            {
                return Unauthorized("Email atau password salah");
            }

            return Ok(new
            {
                message = "Berhasil login user dengan id" + user.Id
            });
        }
    }
}
