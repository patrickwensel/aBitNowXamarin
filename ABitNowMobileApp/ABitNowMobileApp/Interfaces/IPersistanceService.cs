using System;
namespace ABitNowMobileApp.Interfaces
{
    public interface IPersistanceService
    {
        bool IsLoggedIn { get; set; }

        string UserEmail { get; set; }

        string Password { get; set; }
    }
}
