namespace Ghumfir.API.Models.AppSettingsModel;

public class TokenSettingModel
{
    public double AccessTokenExpirationInMinutes { get; set; }
    public double RefreshTokenExpirationInDays { get; set; }
}