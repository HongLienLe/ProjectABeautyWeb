using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AccessDataApi.Authentication;
using AutoMapper;
using JWT;
using JWT.Algorithms;
using JWT.Serializers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AccessDataApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private IUserService _userService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;

        public AuthenticationController(
            IUserService userService,
            IMapper mapper,
            IOptions<AppSettings> appSettings)
        {
            _userService = userService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody]AuthenticateModel model)
        {
            var user = _userService.Authenticate(model.Username, model.Password);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            // return basic user info and authentication token
            return Ok(new
            {
                Id = user.Id,
                Username = user.Username,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Token = tokenString
            });
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromBody]RegisterModel model)
        {
            // map model to entity
            var user = _mapper.Map<User>(model);

            try
            {
                // create user
                _userService.Create(user, model.Password);
                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _userService.GetAll();
            var model = _mapper.Map<IList<UserModel>>(users);
            return Ok(model);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var user = _userService.GetById(id);
            var model = _mapper.Map<UserModel>(user);
            return Ok(model);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody]UpdateModel model)
        {
            // map model to entity and set id
            var user = _mapper.Map<User>(model);
            user.Id = id;

            try
            {
                // update user 
                _userService.Update(user, model.Password);
                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _userService.Delete(id);
            return Ok();
        }
    }
}



    //private readonly UserManager<IdentityUser> _userManager;
    //    private readonly SignInManager<IdentityUser> _signInManager;
    //    private readonly IAuthenticateService _authService;
    //    private readonly TokenManagement _options;

    //    public AuthenticationController(IAuthenticateService authService, UserManager<IdentityUser> userManager,
    //  SignInManager<IdentityUser> signInManager, IOptions<TokenManagement> optionsAccessor)
    //    {
    //        _authService = authService;
    //        _userManager = userManager;
    //        _signInManager = signInManager;
    //        _options = optionsAccessor.Value;

    //    }

    //    [HttpPost]
    //    public async Task<IActionResult> Register([FromBody] Credentials Credentials)
    //    {
    //        if (ModelState.IsValid)
    //        {
    //            var user = new IdentityUser { UserName = Credentials.Email, Email = Credentials.Email };
    //            var result = await _userManager.CreateAsync(user, Credentials.Password);
    //            if (result.Succeeded)
    //            {
    //                await _signInManager.SignInAsync(user, isPersistent: false);
    //                return new JsonResult(new Dictionary<string, object>
    //                {
    //                    { "access_token", GetAccessToken(Credentials.Email) },
    //                    { "id_token", GetIdToken(user) }
    //                });
    //            }
    //            return Errors(result);

    //        }
    //        return Error("Unexpected error");
    //    }

    //    [HttpPost("sign-in")]
    //    public async Task<IActionResult> SignIn([FromBody] Credentials Credentials)
    //    {
    //        if (ModelState.IsValid)
    //        {
    //            var result = await _signInManager.PasswordSignInAsync(Credentials.Email, Credentials.Password, false, false);
    //            if (result.Succeeded)
    //            {
    //                var user = await _userManager.FindByEmailAsync(Credentials.Email);
    //                return new JsonResult(new Dictionary<string, object>
    //                {
    //                    { "access_token", GetAccessToken(Credentials.Email) },
    //                    { "id_token", GetIdToken(user) }
    //                });
    //            }
    //            return new JsonResult("Unable to sign in") { StatusCode = 401 };
    //        }
    //        return Error("Unexpected error");
    //    }

    //    private string GetIdToken(IdentityUser user)
    //    {
    //        var payload = new Dictionary<string, object>
    //            {
    //            { "id", user.Id },
    //            { "sub", user.Email },
    //            { "email", user.Email },
    //            { "emailConfirmed", user.EmailConfirmed },
    //            };
    //        return GetToken(payload);
    //    }


    //        private string GetAccessToken(string Email)
    //        {
    //            var payload = new Dictionary<string, object>
    //            {
    //                { "sub", Email },
    //                { "email", Email }
    //            };
    //            return GetToken(payload);
    //        }

    //    private string GetToken(Dictionary<string, object> payload)
    //    {
    //        var secret = _options.Secret;

    //        payload.Add("iss", _options.Issuer);
    //        payload.Add("aud", _options.Audience);
    //        payload.Add("nbf", ConvertToUnixTimestamp(DateTime.Now));
    //        payload.Add("iat", ConvertToUnixTimestamp(DateTime.Now));
    //        payload.Add("exp", ConvertToUnixTimestamp(DateTime.Now.AddDays(7)));
    //        IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
    //        IJsonSerializer serializer = new JsonNetSerializer();
    //        IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
    //        IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);

    //        return encoder.Encode(payload, secret);

    //        }
    //        private JsonResult Errors(IdentityResult result)
    //        {
    //            var items = result.Errors
    //                .Select(x => x.Description)
    //                .ToArray();
    //            return new JsonResult(items) { StatusCode = 400 };
    //        }

    //        private JsonResult Error(string message)
    //        {
    //            return new JsonResult(message) { StatusCode = 400 };
    //        }

    //    private static double ConvertToUnixTimestamp(DateTime date)
    //    {
    //        DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
    //        TimeSpan diff = date.ToUniversalTime() - origin;
    //        return Math.Floor(diff.TotalSeconds);
    //    }

        //[AllowAnonymous]
        //[HttpPost]
        //public ActionResult RequestToken([FromBody] Credentials request)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest("Invalid Request");
        //    }

        //    string token;
        //    if (_authService.IsAuthenticated(request, out token))
        //    {
        //        return Ok(token);
        //    }

        //    return BadRequest("Invalid Request");
        //}




