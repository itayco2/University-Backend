namespace University_backend;

static class AppConfig
{
    public static bool IsDevelopment { get; private set; }
    public static bool IsProduction { get; private set; }
    public static string ConnectionString { get; private set; } = null!;
    public static string HostUrl { get; private set; } = null!;
    public static string JwtKey { get; private set; } = "משהו רנדומלי#@$#%!#%!#$משהו רנדומלי#@$#%!#%!#$משהו רנדומלי#@$#%!#%!#$משהו רנדומלי#@$#%!#%!#$משהו רנדומלי#@$#%!#%!#$משהו רנדומלי#@$#%!#%!#$";
    public static int JwtExpireHours { get; private set; }

    public static void Configure(IWebHostEnvironment env)
    {
        IsDevelopment = env.IsDevelopment();
        IsProduction = env.IsProduction();

        IConfigurationRoot settings = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .AddJsonFile($"appsettings.{env.EnvironmentName}.json") // appsettings.Development.json / appsettings.Production.json
            .Build();

        ConnectionString = settings.GetConnectionString("UniversityDB")!;

        HostUrl = Environment.GetEnvironmentVariable("ASPNETCORE_URLS")!; 

        JwtExpireHours = IsDevelopment ? 5 : 1;
    }
}