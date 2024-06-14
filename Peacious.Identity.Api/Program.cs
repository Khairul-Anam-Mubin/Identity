using Peacious.Framework;
using Peacious.Framework.Extensions;
using Peacious.Framework.ORM.Migrations;

var builder = WebApplication.CreateBuilder(args);

#region ServiceRegistration

builder
    .AddCustomConfigurationJsonFile(builder.Configuration.TryGetConfig<string>("GlobalConfigPath"))
    .AddAllAssembliesByAssemblyPrefix(builder.Configuration.TryGetConfig<string>("AssemblyPrefix"))
    .InstallServices(AssemblyCache.Instance.GetAddedAssemblies());

#endregion

var app = builder.Build();

#region StartupService

app
    .DoCreateIndexes(app.Configuration.GetConfig<bool>("EnableIndexCreation"))
    .DoMigration(app.Configuration.TryGetConfig<MigrationConfig>("MigrationConfig"))
    .StartInitialServices();

#endregion

#region Middleware

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

#endregion

app.Run();