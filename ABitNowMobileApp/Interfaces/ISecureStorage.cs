using System;
using System.Threading.Tasks;

namespace ABitNowMobileApp.Interfaces
{
    public interface ISecureStorage
    {
        Task<string> GetSecuredValueAsync(string key);

        Task SetSecuredValueAsync(string key, string value);

        bool RemoveSecuredValue(string key);

        void RemoveAllValues();
    }
}
