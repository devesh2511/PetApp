using PetApp.Services;
using PetApp.Services.Repository;
using PetApp.Services.Repository.Admin;
using PetApp.Services.Services;
using PetApp.Services.Services.Admin;
using PetApp.Services.Validators;
using PetApp.Utility;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.Configure<DatabaseSettings>(
    builder.Configuration.GetSection("Database"));

builder.Services.AddSingleton<UserServiceDB>();
builder.Services.AddSingleton<MyPetRepository>();
builder.Services.AddSingleton<PetRepository>();

builder.Services.AddSingleton<JWTService>();
builder.Services.AddSingleton<CompareStrings>();

builder.Services.AddSingleton<UserValidator>();
builder.Services.AddSingleton<MyPetValidator>();

builder.Services.AddSingleton<UserService>();
builder.Services.AddSingleton<MyPetService>();
builder.Services.AddSingleton<PetService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseCors(builder => builder.AllowAnyOrigin()
     .AllowAnyMethod()
     .AllowAnyHeader());
// "http://localhost:4200"
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
