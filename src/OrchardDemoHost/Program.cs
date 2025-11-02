using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var mysql = builder.AddMySql("mysql")
        .WithLifetime(ContainerLifetime.Persistent)
        .WithPhpMyAdmin();

var appDb = mysql.AddDatabase("AppDb");
var contentDb = mysql.AddDatabase("ContentDb");

var apiBackend = builder.AddProject<BackendApp>("backend-apis")
    .WithReference(appDb)
    .WaitFor(mysql);


// storage
var storage = builder.AddAzureStorage("storage")
    .RunAsEmulator(azurite =>
{
    azurite.WithLifetime(ContainerLifetime.Persistent);
    azurite.WithBlobPort(10000)
        .WithQueuePort(10001)
        .WithTablePort(10002);
});

var blobs = storage.AddBlobs("blobs");

var shellContainer = storage.AddBlobContainer("shells");
var dpContainer = storage.AddBlobContainer("data-protect");
var mediaContainer = storage.AddBlobContainer("content-media");
var mediaCacheContainer = storage.AddBlobContainer("content-media-cache");

var adminPassword = builder.AddParameter("admin-password", secret: true);

var contentApp = builder.AddProject<ContentApp>("ContentApp")
    .WaitFor(contentDb)
    .WaitFor(shellContainer)
    .WaitFor(dpContainer)
    .WaitFor(mediaContainer)
    .WaitFor(mediaCacheContainer)
    .WithEnvironment("OrchardCore__OrchardCore_DataProtection_Azure__ConnectionString", blobs)
    .WithEnvironment("OrchardCore__OrchardCore_Shells_Azure__ConnectionString", blobs)
    .WithEnvironment("OrchardCore__OrchardCore_Media_Azure__ConnectionString", blobs)
    .WithEnvironment("OrchardCore__OrchardCore_Media_Azure_ImageSharp_Cache__ConnectionString", blobs)
    .WithEnvironment("OrchardCore__OrchardCore_AutoSetup__Tenants__0__DatabaseConnectionString", contentDb)
    .WithEnvironment("OrchardCore__OrchardCore_AutoSetup__Tenants__0__AdminPassword", adminPassword);

// web client
var clientApp = builder.AddProject<ClientApp>("web-client")
    .WithReference(apiBackend)
    .WaitFor(apiBackend)
    .WithReference(contentApp)
    .WaitFor(contentApp);


builder.Build().Run();