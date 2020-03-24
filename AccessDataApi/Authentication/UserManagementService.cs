using System;
namespace AccessDataApi.Authentication
{
    public class UserManagementService : IUserManagementService
    {
        public bool IsValidUser(string userName, string password)
        {
            //Purpose Of this First implementation It is true however it should never be true
            return true;
        }
    }
}
