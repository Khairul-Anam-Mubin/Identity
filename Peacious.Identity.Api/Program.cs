using Peacious.Framework;
using Peacious.Framework.Extensions;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

builder
    .AddGlobalConfig(configuration.TryGetConfig<string>("GlobalConfigPath"))
    .AddAllAssembliesByAssemblyPrefix(configuration.TryGetConfig<string>("AssemblyPrefix"))
    .InstallServices(AssemblyCache.Instance.GetAddedAssemblies());

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();