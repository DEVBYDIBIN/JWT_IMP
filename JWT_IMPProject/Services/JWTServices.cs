using JWT_IMPProject.Models;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Ocsp;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace JWT_IMPProject.Services
{
    public class JWTServices
    {

        public readonly IConfiguration _config;
       
        public JWTServices(IConfiguration configuration)
        {
            _config = configuration;
        }




        private LogAuthTokenResM BuildToken(UserM user, string reftoken)
        {

            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),            
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("UserID", user.Id.ToString()),              
                new Claim("Status",user.Status.ToString()),               
               };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Issuer"],  
              claims,
              expires: DateTime.Now.AddMinutes(20),
              signingCredentials: creds

              );
            string res2 = string.Empty;
            if (reftoken.Length == 0)
            {
                var refreshtoken = new JwtSecurityToken(_config["Jwt:Issuer"],
                 _config["Jwt:Issuer"],
                 claims,
                 expires: DateTime.Now.AddDays(365),
                 signingCredentials: creds

                 );
                res2 = new JwtSecurityTokenHandler().WriteToken(refreshtoken);
            }
            else
            {
                res2 = reftoken;

            }
            string res1 = new JwtSecurityTokenHandler().WriteToken(token);
            LogAuthTokenResM result = new LogAuthTokenResM();
            result.AccessToken = res1.ToString();
            result.RefreshToken = res2.ToString();
            result.UserId = user.Id;
            result.name = user.UserName.ToString();                 
            result.ExpireInSeconds = 20 * 60;
            result.RefreshTokenExpireInSeconds = 365 * (60 * 60 * 24);
            return result;    
        }


        public LoginAuthTokenM<UserM> GenerateJSONWebToken(LoginAuthTokenM<UserM> LData)
        {
            LoginAuthTokenM<UserM> ReqData = LData;
            if (ReqData.Status == true)
            {
                try
                {
                    string key = _config["Jwt:Key"]; // Access "Jwt:Key" configuration path
                    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
                    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
                    var claims = new[]
                    {
                new Claim("timestamp", DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString()),
                new Claim("Id", LData.Data.Id.ToString()),
                // Add more claims as needed
            };
                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(claims),
                        SigningCredentials = credentials
                    };

                    var tokenHandler = new JwtSecurityTokenHandler();
                    var token = tokenHandler.CreateToken(tokenDescriptor);
                    string Jwtt = tokenHandler.WriteToken(token);
                    ReqData.Token = Jwtt;
                    var ReT = GenerateRefreshToken();
                    ReqData.RefreshToken = ReT;

                }
                catch (Exception ex)
                {
                    ReqData.Status = false;
                    ReqData.StatusMessage = ex.Message;
                }
            }
            else
            {
                ReqData.Status = false;
                ReqData.StatusMessage = "Invalid username or password";
                ReqData.StatusCode = 401;
            }

            return ReqData;
        }



        public string GenerateRefreshToken()
        {
            try
            {
                string key = _config["Jwt:RefreshKey"];
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    _config["Jwt:Issuer"],
                    _config["Jwt:Issuer"],
                    null,
                    expires: DateTime.Now.AddMinutes(Convert.ToDouble(_config["Jwt:RefreshTokenExpirationMinutes"])),
                    signingCredentials: credentials);
                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception)
            {

                throw;
            }
        }



    }
}
