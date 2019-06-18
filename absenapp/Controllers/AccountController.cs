using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using absenapp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using absenapp.Helpers;
using absenapp.Models;

namespace absenapp.Controllers
{
    [Authorize]
    [ApiController]
    [Route ("[controller]")]
    public class AccountController : ControllerBase {
        private IUserService _userService;
        //  private IMapper _mapper;
        private readonly AppSettings _appSettings;

        public AccountController (IUserService userService,
            IOptions<AppSettings> appSettings) {
            _userService = userService;
            _appSettings = appSettings.Value;
        }

        [AllowAnonymous]
        [HttpPost ("authenticate")]
        public async Task<IActionResult> Authenticate ([FromBody] User userDto) {
            try {
                Console.WriteLine (userDto.username + " Ok");
                var user = await _userService.Authenticate (userDto.username, userDto.password);

                if (user == null)
                    throw new SystemException ("Username or password is incorrect");

                var tokenHandler = new JwtSecurityTokenHandler ();
                var key = Encoding.ASCII.GetBytes (_appSettings.Secret);
                var tokenDescriptor = new SecurityTokenDescriptor {
                    Subject = new ClaimsIdentity (new Claim[] {
                    new Claim (ClaimTypes.Name, user.iduser.ToString ())
                    }),
                    Expires = DateTime.UtcNow.AddDays (7),
                    SigningCredentials = new SigningCredentials (new SymmetricSecurityKey (key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken (tokenDescriptor);
                var tokenString = tokenHandler.WriteToken (token);

                // return basic user info (without password) and token to store client side
                return Ok (new User {
                    iduser = user.iduser, roles = user.roles,
                        username = user.username,
                        token = tokenString, avatar = user.avatar
                });
            } catch (System.Exception ex) {
                return BadRequest (new { message = ex.Message });
            }
        }

        

        [AllowAnonymous]
        [HttpPost ("resetpassword")]
        public IActionResult ResetPassword (string email) {
            try {
                var userData = _userService.GetByEmail (email);
                if (userData != null) {
                    var tokenHandler = new JwtSecurityTokenHandler ();
                    var key = Encoding.ASCII.GetBytes (_appSettings.Secret);
                    var tokenDescriptor = new SecurityTokenDescriptor {
                        Subject = new ClaimsIdentity (new Claim[] {
                        new Claim (ClaimTypes.Name, userData.iduser.ToString ())
                        }),
                        Expires = DateTime.UtcNow.AddDays (1),
                        SigningCredentials = new SigningCredentials (new SymmetricSecurityKey (key), SecurityAlgorithms.HmacSha256Signature)
                    };
                    var token = tokenHandler.CreateToken (tokenDescriptor);
                    var tokenString = tokenHandler.WriteToken (token);

                    var callbackUrl = Url.Action (
                        "change", "user",
                        new { userId = userData.iduser, email=email, code = tokenString },
                        Request.Scheme);

                  
                    if (!string.IsNullOrEmpty (tokenString))
                        return Ok (new{result="Silahkan Periksa Email Anda Untuk Reset Password"});

                    throw new SystemException ("Coba Ulang Lagi");
                } else {
                    throw new SystemException ("Anda Belum Terdaftar");
                }

            } catch (System.Exception ex) {

                return BadRequest (ex.Message);
            }

        }

        [HttpPost ("changepassword")]
        public IActionResult ChangePasswordAction ([FromBody] ChangePassword data) {
            try {

                if (data != null && !string.IsNullOrEmpty (data.newpassword) &&
                    !string.IsNullOrEmpty (data.confirmpassword) && data.newpassword == data.confirmpassword) {
                    var userData = _userService.GetByEmail (data.email);
                    if (_userService.ChangePassword (data, userData))
                        return Ok (new{result="Password Berhasil Diubah"});
                }
                throw new SystemException ("Periksa Kembali Data Anda");
            } catch (System.Exception ex) {

                return BadRequest (ex.Message);
            }
        }
    }
}