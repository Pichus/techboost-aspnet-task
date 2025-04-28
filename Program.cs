using Microsoft.EntityFrameworkCore;
using techboost_aspnet.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContextPool<MusicCollectionDbContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("MusicCollectionDbContext")));
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<MusicCollectionDbContext>();
        await db.Database.MigrateAsync();
    }

    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();