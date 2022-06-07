using EarnShardsForCards.Server.Data;
using EarnShardsForCards.Server.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("secrets.json", optional: true).AddEnvironmentVariables();

// Add services to the container.
var mainConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// https://stackoverflow.com/questions/55432473/transient-failure-handling-in-net-core-2-1-mvc-for-mysql-database - Sir Crusher
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseMySql(mainConnectionString,
        new MariaDbServerVersion(new Version(10, 6, 7)), // Hard coded for now to prevent fatal exceptions when database is unreachable
        mySqlOptions =>
        {
            mySqlOptions.EnableRetryOnFailure(
            maxRetryCount: 10,
            maxRetryDelay: TimeSpan.FromSeconds(30),
            errorNumbersToAdd: null);
        });
});

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddIdentityServer()
    .AddApiAuthorization<ApplicationUser, ApplicationDbContext>();

builder.Services.AddAuthentication()
    .AddIdentityServerJwt();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
    app.UseWebAssemblyDebugging();
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

//var policyCollection = new HeaderPolicyCollection()
//        .AddFrameOptionsDeny()
//        .AddXssProtectionBlock()
//        .AddContentTypeOptionsNoSniff()
//        .AddStrictTransportSecurityMaxAgeIncludeSubDomains(maxAgeInSeconds: 60 * 60 * 24 * 365) // maxage = one year in seconds
//        .AddReferrerPolicyStrictOriginWhenCrossOrigin()
//        .RemoveServerHeader()
//        .AddContentSecurityPolicy(builder =>
//        {
//            builder.AddObjectSrc().None();
//            builder.AddFormAction().Self();
//            builder.AddFrameAncestors().Self();
//        })
//        .AddCrossOriginOpenerPolicy(builder =>
//        {
//            builder.SameOrigin();
//        })
//        .AddCrossOriginEmbedderPolicy(builder =>
//        {
//            builder.RequireCorp();
//        })
//        .AddCrossOriginResourcePolicy(builder =>
//        {
//            builder.CrossOrigin();
//        })
//        .AddCustomHeader("X-My-Test-Header", "Header value");

//app.UseSecurityHeaders(policyCollection);

app.UseRouting();

app.UseIdentityServer();
app.UseAuthentication();
app.UseAuthorization();


app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
