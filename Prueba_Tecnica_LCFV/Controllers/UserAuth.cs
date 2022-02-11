using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace Prueba_Tecnica_LCFV.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAuth : ControllerBase
    {
        public static User user = new User();
        private readonly IConfiguration _configuration;

        public UserAuth(IConfiguration configuration)
        {
            _configuration = configuration;
        }
       
        [HttpPost("registrar")]
        public async Task<ActionResult<User>> Registrar(UserDTO request)
        {
            CrearPasswordHash(request.password, out byte[] passwordHash, out byte[] passwordSalt);
            user.username = request.username;
            user.passwordHash = passwordHash;
            user.passwordSalt = passwordSalt;   
            user.type = request.type;

            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(UserDTO request)
        {
            if (user.username == request.username)
            {
                if (!VerificarPasswordHash(request.password, user.passwordHash, user.passwordSalt))
                {
                    return BadRequest("Password Incorrecto");
                }
                else
                {
                    if (user.type != "AU")
                    {
                        return BadRequest("Usuario No Autorizado");
                    }
                    else
                    {
                        //return Ok("Usuario Autorizado");
                        string token = CrearToken(user);
                        return Ok(token);
                    }
                }
            }
            else
            {
                return BadRequest("Usuario No Encontrado");
            }
        }

        private string CrearToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name,user.username)
            };
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value));

            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials:cred);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        private void CrearPasswordHash(string password, out byte[] passwordHash, out byte[] PasswordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                PasswordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

            }
        }

        private bool VerificarPasswordHash(string password, byte[] passwordHash, byte[] PasswordSalt)
        {
            using (var hmac = new HMACSHA512(PasswordSalt))
            {
                var computeHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computeHash.SequenceEqual(passwordHash);
            }
        }
    }
}
