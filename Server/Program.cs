using Microsoft.AspNetCore.SignalR;
using Server;
using Server.Hub;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IUserIdProvider, MyCustomProvider>();

// Add services to the container.
builder.Services.AddSignalR();
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapHub<ChatHub>("/Chat"); // Faz com que o hub seja acessível em /Chat

app.UseAuthorization();

app.MapControllers();

app.Run();