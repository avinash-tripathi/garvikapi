using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using GARVIKService.Middleware;
using Stripe;
using Stripe.Checkout;

namespace GARVIKService
{
    public class Startup
    {
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                                  builder =>
                                  {
                                      builder.WithOrigins("http://localhost:4200",
                                                          "http://localhost:4210",
                                                          "http://localhost:5000",
                                                          "https://checkout.stripe.com"
                                                          )
                                        .AllowAnyHeader()
                                        .AllowAnyOrigin()
                                        .AllowAnyMethod()
                                        .WithMethods("PUT", "DELETE", "GET", "POST", "OPTIONS");
                                  });
            });

            services.AddHttpContextAccessor();
            services.AddSingleton<IConfiguration>(Configuration);

            services.AddMvc(setupAction => {
                setupAction.EnableEndpointRouting = false;
            }).AddJsonOptions(jsonOptions =>
            {
                jsonOptions.JsonSerializerOptions.PropertyNamingPolicy = null;
            })
        .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            services.AddControllers();
            
            services.AddTokenAuthentication(Configuration);
            


        }

            // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
            public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
            {
                if (env.IsDevelopment())
                {
                    app.UseDeveloperExceptionPage();
                   
                }
            //StripeConfiguration.ApiKey = "sk_test_51Nc8teSCPyqIiJIsUPOqn28jfqPAtSEtZy5KkO3FBtmAR7ROmFFSdcm9OJEYVnTiq8XMj03bxlB90codDXuXHmC000Vn5JlzFu";
            StripeConfiguration.ApiKey = "sk_test_oNDqnEvauLy1tNa4PYSDZT0I00OhjFeLN4";


            app.UseHttpsRedirection();
                app.UseRouting();
                app.UseCors(MyAllowSpecificOrigins);
                app.UseAuthentication();
                app.UseAuthorization();
                //app.UseStaticFiles();


                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                });
            }
        }
    }