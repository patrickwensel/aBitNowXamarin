using System;
namespace ABitNowMobileApp.UiItemModels
{
    public class LocationSuggestion
    {
        public string LocationName { get; set; }

        public string Address { get; set; }

        public TimeSpan DriveTime { get; set; }

        public string FriendlyDriveTime => DriveTime > default(TimeSpan) ? $"{DriveTime.Hours} h {DriveTime.Minutes} mins drive" : string.Empty;
      
        public double Distance { get; set; }
    }
}
