using JWT_IMPProject.IServices;
using JWT_IMPProject.Models;
using JWT_IMPProject.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JWT_IMPProject.Controllers
{

   
    public class UserController : Controller
    {
        public readonly IDataFlow DataAcess;
        public readonly JWTServices JWTAccess;
        public UserController(IDataFlow dataAcess, JWTServices jWTServicesAccess)
        {
            this.DataAcess = dataAcess;
            JWTAccess = jWTServicesAccess;
        }


        [HttpGet]
        [Route("UserLoginAuth")]
        public async Task<IActionResult> UserLoginAuth(string UserName,string Password) 
        {
            try
            {
                if (string.IsNullOrEmpty(UserName) || string.IsNullOrEmpty(Password))
                {                 
                    return BadRequest("Username or password is empty.");
                }              
                var Resdata = await DataAcess.LoginAuth(UserName, Password);
                if(Resdata.Status == true)
                {
                    UserM data = Resdata.Data;
                    var ReqData = JWTAccess.GenerateJSONWebToken(Resdata);

                    ////string RefreshToken = JWTAccess.GenerateRefreshToken();
                    //Resdata.Token = Token;
                    //Resdata.RefreshToken = RefreshToken;
                    return Ok(ReqData);
                }
                else
                {
                    return BadRequest(Resdata);
               }          
            }
            catch (Exception ex)
            {
                return BadRequest(" Internal Server Error");
            }

        }

    }
}
