using BusinessObjects.CustomModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ProductWebAPI.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LoginModel = BusinessObjects.CustomModel.LoginModel;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUsers> userManager;
        private readonly SignInManager<ApplicationUsers> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration configuration;

        public UserController(UserManager<ApplicationUsers> userManager, SignInManager<ApplicationUsers> signInManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
            this.configuration = configuration;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (model.Email.Equals(configuration["Credentials:Email"]))
            {
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, configuration["Credentials:Email"]),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.Role, UserRoles.Admin)
                };
                var token = GetToken(authClaims);
                return Ok(new JwtSecurityTokenHandler().WriteToken(token));
            }
            var user = await userManager.FindByEmailAsync(model.Email);
            if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
            {
                var authClaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Email, user.Email),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(ClaimTypes.Role, UserRoles.Customer)
                    };
                var token = GetToken(authClaims);
                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    userid = user.Id
                });
            }
            return Unauthorized();
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(BusinessObjects.CustomModel.RegisterModel model)
        {
            var userExists = await userManager.FindByEmailAsync(model.Email);
            if (userExists != null || model.Email.Equals(configuration["Credentials:Email"]))
            {
                return StatusCode(StatusCodes.Status400BadRequest, new Response { Status = "Error", Message = "User already exists!" });
            }
            var user = new ApplicationUsers
            {
                UserName = model.UserName,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PasswordHash = model.Password
            };
            var result = await userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Failed to create user." });
            }
            return StatusCode(StatusCodes.Status201Created, new Response { Status = "Success", Message = "User created successfully!" });
        }

        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: configuration["JWT:Issuer"],
                audience: configuration["JWT:Audience"],
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }
    }
}
