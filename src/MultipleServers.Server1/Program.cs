using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.OpenApi.Models;
using MultipleServers.Jobs.Abstract;
using MultipleServers.Server1.Jobs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Server hagfire 1", Description = "Servidor de hangfire 01", Version = "v1" });
});
builder.Services.AddHangfire(configuration => configuration
    .UseSimpleAssemblyNameTypeSerializer()
    .UseColouredConsoleLogProvider()
    .UseRecommendedSerializerSettings()
    .UsePostgreSqlStorage(builder.Configuration.GetConnectionString("Postgresql_hangfire")));
builder.Services.AddHangfireServer(b =>
{
    b.ServerName = "server1-60eb6436-d909-4b06-9a0d-013b46f196b0";
    b.Queues = new string[] { "server1-job1"};
});
builder.Services.AddScoped<IRecurringJob<SampleRecurringJobServer1>, SampleRecurringJobServer1>();
builder.Services.AddScoped<SampleRecurringJobServer1, SampleRecurringJobServer1>();

var app = builder.Build();

GlobalConfiguration.Configuration.UseActivator(new HangfireJobActivator(app.Services));

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