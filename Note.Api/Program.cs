
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
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
    