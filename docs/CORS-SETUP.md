# CORS Setup for Local Development

## Why CORS is Needed
When developing the Order Management System, the frontend (React, Vite) and backend (ASP.NET Core) run on different ports/hosts. Browsers block API requests between different origins unless the backend explicitly allows them via CORS (Cross-Origin Resource Sharing).

## Enabling CORS in ASP.NET Core
To allow the frontend to access the backend API during development, add the following to your `Program.cs`:

```
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy => policy
            .AllowAnyOrigin() // TEMPORARY: allow all origins for local development
            .AllowAnyHeader()
            .AllowAnyMethod());
});
```

And register the middleware **before controllers**:

```
app.UseHttpsRedirection();
app.UseCors("AllowFrontend");
// ...
app.MapControllers();
```

**Note:** For production, restrict origins instead of using `.AllowAnyOrigin()`.

## Important: Rebuild Docker After CORS Changes
Whenever you add or change the CORS setup in `Program.cs`, you **must** rebuild and restart the backend Docker container for the changes to take effect:

```
docker compose up --build
```

If you skip this step, your changes will not be picked up and CORS errors will persist.

## Troubleshooting
- If you see errors like `No 'Access-Control-Allow-Origin' header is present`, CORS is not enabled or Docker is not running the latest build.
- Always rebuild Docker after CORS changes.

---

For more info, see the [Microsoft CORS docs](https://learn.microsoft.com/en-us/aspnet/core/security/cors?view=aspnetcore-7.0).
