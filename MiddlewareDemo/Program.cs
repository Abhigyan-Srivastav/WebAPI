using Microsoft.AspNetCore.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddRateLimiter(_ => _.AddFixedWindowLimiter(policyName: "fixed", options =>
{
    options.PermitLimit = 5;
    options.Window = TimeSpan.FromSeconds(10);
    options.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst;
    options.QueueLimit = 2;
}));

builder.Services.AddRequestTimeouts();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

app.Use(async (context, next) =>
{
    var logger = app.Services.GetRequiredService<ILogger<Program>>();
    logger.LogInformation($"Request Host: {context.Request.Host}");
    logger.LogInformation("My Middleware - Before");
    await next(context);
    logger.LogInformation("My Middleware - After");
    logger.LogInformation($"Response StatusCode: {context.Response.StatusCode}");
}
);

app.Use(async(context, next)=>
{
    var logger = app.Services.GetRequiredService<ILogger<Program>>();
    logger.LogInformation($"ClientName HttpHeader in Middleware 2:{context.Request.Headers["ClientName"]}");
    logger.LogInformation("My Middleware 2 - Before");
    context.Response.StatusCode = StatusCodes.Status202Accepted;
    await next(context);
    logger.LogInformation("My Middleware 2 - After");
    logger.LogInformation($"Response Status Code in Middleware 2: {context.Response.StatusCode}");
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseRateLimiter();

app.UseRequestTimeouts();

app.MapGet("/rate-limiting-mini", () => Results.Ok($"Hello {DateTime.Now.Ticks.ToString()}")).RequireRateLimiting("fixed");


//app.Map("/lottery", app =>
//{
//    var random = new Random();
//    var luckyNumber = random.Next(1, 6);
//    app.UseWhen(context => context.Request.QueryString.Value == $"?{luckyNumber.ToString()}", app =>
//    {
//        app.Run(async context=>
//        {
//            await context.Response.WriteAsync($"You win! You got the lucky number {luckyNumber}!");
//        });
//    });
//    app.UseWhen(context => string.IsNullOrWhiteSpace(context.Request.QueryString.Value), app =>
//    {
//        app.Use(async (context, next) =>
//        {
//            var number = random.Next(1, 6);
//            context.Request.Headers.TryAdd("number", number.ToString());
//            await next(context);
//        });
//        app.UseWhen(context => context.Request.Headers["number"] == luckyNumber.ToString(), app =>
//        {
//            app.Run(async context =>
//            {
//                await context.Response.WriteAsync($"You win! You got the lucky number {luckyNumber}!");
//            });
//        });

//    });
//    app.Run(async context =>
//    {
//        var number = "";
//        if(context.Request.QueryString.HasValue)
//        {
//            number = context.Request.QueryString.Value?.Replace("?","");
//        }
//        else
//        {
//            number = context.Request.Headers["number"];
//        }
//        await context.Response.WriteAsync($"Your number is {number}. Try again!");
//    });

//});
//app.Run(async context =>
//{
//    await context.Response.WriteAsync($"Use the /lottery URL to play. You can choose your number with the format /lottery?1.");
//});


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
