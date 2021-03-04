using Contoso.ToDo.Api.Data;
using Contoso.ToDo.Api.Services;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(Contoso.ToDo.Api.Startup))]

namespace Contoso.ToDo.Api
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddEntityFrameworkCosmos();
            builder.Services.AddScoped<ToDoDBContext>();
            builder.Services.AddScoped<IToDoService, ToDoService>();
        }
    }
}
