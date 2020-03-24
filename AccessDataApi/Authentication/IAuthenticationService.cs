using System;
namespace AccessDataApi.Authentication
{
    public interface IAuthenticateService
    {
        bool IsAuthenticated(Credentials request, out string token);
    }
}
