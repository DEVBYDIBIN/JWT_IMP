using NPOI.SS.Formula.Functions;

namespace JWT_IMPProject.Models
{
    public class LoginAuthTokenM<T>
    {
        public bool Status { get; set; }
        public string StatusMessage { get; set; }
        public int StatusCode { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public T Data { get; set; }

    }
}
