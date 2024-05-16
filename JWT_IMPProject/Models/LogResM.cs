using NPOI.SS.Formula.Functions;

namespace JWT_IMPProject.Models
{
    public class LogAuthTokenResM
    {
        public string AccessToken { get; set; }
        public Int64 ExpireInSeconds { get; set; }
        public string RefreshToken { get; set; }
        public Int64 RefreshTokenExpireInSeconds { get; set; }
        public int UserId { get; set; }
        public string name { get; set; }
        public string Usertypeid { get; set; }
        public string Usertype { get; set; }
        public string UWarehouse { get; set; }
    }
}
