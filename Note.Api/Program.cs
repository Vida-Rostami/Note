
using Note.Application.Category;
using Note.Application.Note;
using Note.Application.Tag;
using Note.Infrastructure.Category;
using Note.Infrastructure.Note;
using Note.Infrastructure.Tag;
using Note.Model;

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
            builder.Services.AddScoped<ITagServices,ITagServices>();
            builder.Services.AddScoped<ICategoryServices, CategoryServices>();

            builder.Services.AddScoped<INoteRepository, NoteRepository>();
            builder.Services.AddScoped<ITagRepository, TagRepository>();
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
