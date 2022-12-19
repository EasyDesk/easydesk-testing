using Argon;
using VerifyTests;

namespace EasyDesk.Testing.VerifyConfiguration;

public static class VerifySettingsInitializer
{
    public static void Initialize()
    {
        VerifierSettings.AddExtraSettings(settings =>
        {
            settings.Converters.Add(new OptionConverter());
            settings.Converters.Add(new NoneOptionConverter());
            settings.NullValueHandling = NullValueHandling.Include;
            settings.DefaultValueHandling = DefaultValueHandling.Include;
        });
    }
}
