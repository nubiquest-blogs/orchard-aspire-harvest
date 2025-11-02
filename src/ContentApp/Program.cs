using ContentApp.Migrations;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services
    .AddOrchardCms()
    .ConfigureServices(services =>
    {
        services.AddMigrations();
    })
    .Configure((app, routes, _) =>
    {
        app.UseCookiePolicy();
        app.UseStaticFiles();
        app.UseAntiforgery();

        routes.MapStaticAssets();
    })
    .AddAzureShellsConfiguration()
    .AddSetupFeatures("OrchardCore.AutoSetup")
    .EnableFeature("OrchardCore.Media.Azure.Storage")
    .EnableFeature("OrchardCore.ContentPreview")
    .EnableFeature("OrchardCore.Contents.Deployment.Download")
    .EnableFeature("OrchardCore.DataProtection.Azure")
    .EnableFeature("OrchardCore.Media.Azure.ImageSharpImageCache")
    .EnableFeature("OrchardCore.Autoroute")
    .EnableFeature("OrchardCore.Alias")
    .EnableFeature("OrchardCore.OrchardCore");


var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseHttpsRedirection();
app.UseStaticFiles();


app.UseOrchardCore(_ =>
{
    // serilog is creating issues
    //c.UseSerilogTenantNameLogging();
});


app.Run();