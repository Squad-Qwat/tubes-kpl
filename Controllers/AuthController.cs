using Microsoft.AspNetCore.Mvc;
using PaperNest_API.Models;
using PaperNest_API.Repository;
using PaperNest_API.Services;

namespace PaperNest_API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : Controller
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
                    Role = string.IsNullOrEmpty(user.Role) ? "Mahasiswa" : user.Role
                };

                UserService.Add(user_obj);

                return Ok(new
                {
                    message = "Berhasil registrasi user",
                    data = user_obj
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
                message = "User berhasil login",
            });
        }

        [HttpPost("reset-password")]
        public IActionResult ResetPassword(string username, string newPassword)
        {
            bool success = UserService.ResetPassword(username, newPassword);
            
            if (!success)
            {
                return NotFound(new
                {
                    message = "User tidak ditemukan [Reset Password Gagal]"
                });
            }

            return Ok(new
            {
                message = "Password berhasil direset"
            });
        }
    }
}
