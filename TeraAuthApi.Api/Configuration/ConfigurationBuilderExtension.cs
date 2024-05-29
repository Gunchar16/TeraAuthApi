namespace TeraAuthApi.Api.Configuration;

internal static class ConfigurationBuilderExtension
{
    public static IConfigurationBuilder AddCustomConfigurations(this IConfigurationBuilder builder)
    {
        builder
            .AddJsonFile("settings/SmtpSettings.json", optional: false)
            ;

        return builder;
    }
}