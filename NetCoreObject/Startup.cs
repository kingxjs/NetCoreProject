using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetCoreObject.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using StackExchange.Redis;
using NetCoreObject.Common;
using UEditorNetCore;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace NetCoreObject
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment _hostingEnvironment)
        {
            Configuration = configuration;
            HostingEnvironment = _hostingEnvironment;
        }

        public IConfiguration Configuration { get; }
        public IHostingEnvironment HostingEnvironment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            //   .AddCookie(options =>
            //   {
            //       options.LoginPath = "/Account/LogIn";
            //       options.LogoutPath = "/Account/LogOff";
            //   });
            services.AddUEditorService();
            //services.AddMvc()
            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(MyAuthorizationFilter));
                options.Filters.Add(typeof(MyResourceFilter));
                options.Filters.Add(typeof(MyActionFilter));
                options.Filters.Add(typeof(MyNoActionFilter));
                options.Filters.Add(typeof(MyExceptionFilter));
                options.Filters.Add(typeof(MyResultFilter));
            })
            //全局配置Json序列化处理
            .AddJsonOptions(options =>
                {
                    //忽略循环引用
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    //不使用驼峰样式的key
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                    //设置时间格式
                    //options.SerializerSettings.DateFormatString = "yyyy-MM-dd";
                }
            );
            //services.AddAuthentication("member")
            //   .AddCookie("member", options =>
            //   {
            //       options.AccessDeniedPath = "/error/404";
            //       //options.LoginPath = "/Account/Unauthorized/";
            //   });

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, o =>
        {
            o.LoginPath = new PathString("/Account/Login");
            o.AccessDeniedPath = "/error/404";
        });



            //var urls = Configuration["AppConfig:Cores"].Split(',');
            services.AddCors(options =>
             options.AddPolicy("AllowCors",
         builder => builder.AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin().AllowCredentials()));


            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddHttpContextAccessor();
            services.AddTransient<SugarBase>();
            Utils.ServerPath = HostingEnvironment.ContentRootPath;

            #region 实例化
            RedisHelper.GetIntance(Configuration);
            SugarBase.SetConnectionString(Configuration);
            DataHelper.GetIntance(Configuration);
            #endregion 实例化

            services.Configure<IConfiguration>(Configuration);

            services.AddSession();
            services.AddOptions();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseSession();
            app.UseAuthentication();
            app.UseStaticHttpContext();
            app.UseStatusCodePagesWithReExecute("/error/{0}");
            app.UseForwardedHeaders(new ForwardedHeadersOptions { ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto });


            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "SysAdmin/Login",
                    template: "SysAdmin/Account/Login");

                routes.MapRoute(
                    name: "areaRoute",
                    template: "{area:exists}/{controller}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
            app.UseCors("AllowCors");
        }
    }
}
