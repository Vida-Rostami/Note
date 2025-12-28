
using Note.Api.Middleware;
using Note.Application.Category;
using Note.Application.Note;
using Note.Application.Tag;
using Note.Infrastructure.Category;
using Note.Infrastructure.Log.ExceptionLoggerService;
using Note.Infrastructure.Note;
using Note.Infrastructure.Tag;
using Note.Domain;
using Note.Infrastructure.Log.AppLogger;
using Note.Infrastructure.Caching;
using Microsoft.AspNetCore.RateLimiting;
using System.Net.Mime;

namespace Note.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("ConnectionStrings"));
            builder.Services.AddScoped<INoteServices, NoteServices>();
            builder.Services.AddScoped<ITagServices, TagServices>();
            builder.Services.AddScoped<ICategoryServices, CategoryServices>();

            builder.Services.AddScoped<INoteRepository, NoteRepository>();
            builder.Services.AddScoped<ITagRepository, TagRepository>();
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.Services.AddScoped<IExceptionLogger, ExceptionLogger>();

            builder.Services.AddScoped<IAppLogger, AppLogger>();
            builder.Services.AddScoped<ICacheService, RedisCacheService>();

            builder.Services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = builder.Configuration.GetSection("Redis:ConnectionString").Value;
                options.InstanceName = "NoteApp";
            });
            builder.Services.AddRateLimiter(options =>
            {
                options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
                options.AddTokenBucketLimiter("token", limiterOptions =>
                {
                    limiterOptions.TokenLimit = 1;
                    limiterOptions.TokensPerPeriod = 1;
                    limiterOptions.ReplenishmentPeriod = TimeSpan.FromMinutes(1);
                    limiterOptions.AutoReplenishment = true;
                });
                options.OnRejected = async (context, token) =>
                {
                    context.HttpContext.Response.ContentType = "application/json";
                   await context.HttpContext.Response.WriteAsJsonAsync(new
                    {
                        isSuccess = false,
                        code = 429,
                        Message = "تعداد درخواست‌ها بیش از حد مجاز است. لطفا بعدا تلاش کنید."
                    },token);
                };
            });

            var redisConnectionString = builder.Configuration.GetValue<string>("Redis:ConnectionString");
            var oracleConnection = builder.Configuration.GetConnectionString("OracleConnection");
            builder.Services.AddHealthChecks()
                .AddRedis(redisConnectionString)
                .AddOracle(oracleConnection);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            //app.UseExceptionHandler(appBuilder =>
            //{
            //    appBuilder.Run(async context =>
            //    {
            //        context.Response.StatusCode = 500;
            //        await context.Response.WriteAsync("Internal Server Error");
            //    });
            //});
            app.UseMiddleware<CorrelationIdMiddleware>();
            app.UseMiddleware<RequestResponseLoggingMiddleware>();
            app.UseMiddleware<ExceptionMiddleware>();
            app.UseStatusCodePages(async context =>
            {
                var response = context.HttpContext.Response;
                if (response.StatusCode == StatusCodes.Status404NotFound)
                {
                    response.ContentType = "application/json";
                    await response.WriteAsJsonAsync(new
                    {
                        message = ErrorMessages.NotFound
                    });
                }
            });
            app.UseHttpsRedirection();
            app.UseRateLimiter();
            app.UseAuthorization();


            app.MapControllers();

            app.MapHealthChecks("/health");
            app.Run();
        }
    }
}
