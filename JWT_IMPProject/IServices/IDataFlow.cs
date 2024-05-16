using JWT_IMPProject.Models;
using NPOI.SS.Formula.Functions;

namespace JWT_IMPProject.IServices
{
    public interface IDataFlow
    {
        public Task<LoginAuthTokenM<UserM>> LoginAuth(string username, string password);

    }
}
