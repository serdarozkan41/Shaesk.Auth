# Shaesk Auth
## This package is designed to automatically install the auth system.


Startup add ConfigureServices methods in.
```sh
 var authSettings = Configuration.GetSection("AuthSettings").Get<AuthSettings>();
            services.AddAuthLayer(authSettings,
                x => x.UseSqlServer(authSettings.ConnectionString,
                y => y.MigrationsAssembly("Shaest.TestApi")));
```
Startup add Configure methods in.
```sh
            app.UseApiResponseAndExceptionWrapper();
            app.UseRouting();
            app.UseAuthentication();
```
Add Application Settings
```sh
"AuthSettings": {
    "ConnectionString": "Data Source=xx;User Id=xx;Password=xx;Initial Catalog=xxx;",
    "SigningKey": "e@_E7Yg4xk;>Bb:e-8PM/6.FpQCm/f46="
  }
```

Last Action

```sh
Add-Migration InitAuth --Context AuthDbContext
Update-Database --Context AuthDbContext
```

Start web api auth system ready
