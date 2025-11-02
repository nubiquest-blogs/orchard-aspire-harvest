using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var mysql = builder.AddMySql("mysql")
    .WithPhpMyAdmin();

var appDb = mysql.AddDatabase("AppDb");

var apiBackend = builder.AddProject<BackendApp>("backend-apis")
    .WithReference(appDb)
    .WaitFor(mysql);

var clientApp = builder.AddProject<ClientApp>("web-client")
    .WithReference(apiBackend)
    .WaitFor(apiBackend);

builder.Build().Run();