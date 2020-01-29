namespace ABitNow.Contracts.Services.General
{
    public interface ISettingsService
    {
        void AddItem(string key, string value);
        string GetItem(string key);

        string AccessTokenSetting { get; set; }
        
    }
}
