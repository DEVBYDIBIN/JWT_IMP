using JWT_IMPProject.IServices;
using JWT_IMPProject.Models;
using NPOI.SS.Formula.Functions;

namespace JWT_IMPProject.Services
{
    public class DataFlow: IDataFlow
    {
        public readonly IConfiguration Configuration;
        public DataFlow(IConfiguration configuration)
        {
            Configuration = configuration;  
        }


        public async Task<LoginAuthTokenM<UserM>> LoginAuth(string username, string password)
        {
            LoginAuthTokenM<UserM> userLRes = new LoginAuthTokenM<UserM>();
            try
            {
                UserM Udata = GetUsers(username, password);
                if (Udata != null)
                {
                    userLRes.Status = true;
                    userLRes.StatusMessage = "Login successful";
                    userLRes.StatusCode = 200;
                    userLRes.Data = Udata;
                }
                else
                {
                    userLRes.Status = false;
                    userLRes.StatusMessage = "Invalid username or password";
                    userLRes.StatusCode = 401;
                }
            }
            catch (Exception)
            {
                userLRes.Status = false;
                userLRes.StatusMessage = "An error occurred while processing the request";
                userLRes.StatusCode = 500;
            }
            return userLRes;
        }


        public UserM GetUsers(string username, string password)
        {
            List<UserM> userList = new List<UserM>();
            UserM filteredUsers = new UserM();
            
            userList.Add(new UserM { Id = 1, UserName = "user1", Password = "password1", Email = "user1@example.com", ContactN = "1234567890", Address1 = "Address1", Address2 = "Address2", City = "City1", State = "State1", Status = true });
            userList.Add(new UserM { Id = 2, UserName = "user2", Password = "password2", Email = "user2@example.com", ContactN = "9876543210", Address1 = "Address3", Address2 = "Address4", City = "City2", State = "State2", Status = false });
            filteredUsers = userList.FirstOrDefault(u => u.UserName == username && u.Password == password);
            return filteredUsers;
        }
    }
}
