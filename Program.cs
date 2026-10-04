using System.Text;

var builder = WebApplication.CreateBuilder(args);

if (builder.Configuration.GetValue<bool>("UseController"))
{
    //everything is done in the "CatchAll10APIController.cs" file
    builder.Services.AddControllers();

    var app = builder.Build();
    app.UseAuthentication();
    app.UseAuthorization();
    app.UseRouting();
    app.MapControllers();
    app.Run();
}
else
{    
    //everything is done here, no controller is needed

    // Every standard HTTP verb that can be caught
    var httpMethods = new[] { "GET", "POST", "PUT", "PATCH", "DELETE", "HEAD", "OPTIONS", "TRACE", "CONNECT" };
    var app = builder.Build();
    app.MapMethods("{**path}", httpMethods, async (HttpContext context) =>
    {
        // The ** token is available as a named route value
        var pathSegment = context.GetRouteData().Values["path"]?.ToString() ?? "";

        string? body = null;
        if (!string.Equals(context.Request.Method, "GET", StringComparison.OrdinalIgnoreCase))
        {
            using var reader = new StreamReader(context.Request.Body, Encoding.UTF8);
            var buffer = new char[32 * 1024]; //read just 32kb to prevent buffer overflows
            int charactersRead = await reader.ReadAsync(buffer.AsMemory(), context.RequestAborted);
            if (charactersRead > 0)
            {
                body = new string(buffer, 0, charactersRead);
            }
        }

        return Results.Ok(new
        {
            message = $"All verbs caught (main). Current verb: {context.Request.Method}",
            method = context.Request.Method,
            fullPath = context.Request.Path,
            pathSegment,
            contentType = context.Request.ContentType,
            body,
            queryString = context.Request.QueryString.ToString()
        });
    });

    app.Run();
}

//Sample calls
//curl -X GET https://localhost:7200/CatchAll10API
//curl -X POST https://localhost:7200/api/CatchAll10API -d '{"hello":"world"}'
//curl -X PUT https://localhost:7200/api/whatEver?xx=1 -d 'update-me'
//curl -X DELETE https://localhost:7200/blahblah/CatXXX
