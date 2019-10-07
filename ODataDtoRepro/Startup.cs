using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ODataDtoRepro.Models;

namespace ODataDtoRepro
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddOData();

      services.AddMvc(options =>
      {
        options.EnableEndpointRouting = false;
      })
        .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }
      else
      {
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
      }

      app.UseHttpsRedirection();
      app.UseMvc(routeBuilder =>
      {
        routeBuilder.EnableDependencyInjection();
        routeBuilder
          .Select()
          .Expand()
          .Filter()
          .OrderBy()
          .MaxTop(128)
          .Count();
        var edmModel = new ODataConventionModelBuilder();
        edmModel.EntitySet<Contact>("Contacts");
        edmModel.EntityType<ContactDto>();

        routeBuilder.MapODataServiceRoute("API Route", Constants.Api, edmModel.GetEdmModel());

      });
    }
  }
}
