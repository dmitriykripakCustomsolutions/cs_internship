using ASpNetCoreConfig.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using BusinessLayer.UserService;
using System.Linq;
using DataAccessLayer.Entities;
using BusinessLayer.ComputerService;
using Microsoft.Extensions.Logging;
using BusinessLayer.Lifecycle;

namespace ASpNetCoreConfig
{
	public class Startup 
	{
		public Startup(IConfiguration configuration, IWebHostEnvironment environment)
		{
			var checker = environment;
			Configuration = new ConfigurationBuilder().AddJsonFile($"appSettings.{environment.EnvironmentName}.json").Build();
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{			
			services.AddDbContext<ApplicationDbContext>(option =>
			{
				option.UseSqlServer(Configuration["SqlServerConnectionString"], b => b.MigrationsAssembly("DataAccessLayer"));
			});

			services.AddAuthentication(opt =>
			{
				opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			}).AddJwtBearer(opt =>
			{
				opt.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuer = true,
					ValidateAudience = true,
					ValidateLifetime = true,
					ValidIssuer = "https://localhost:44382",
					ValidAudience = "https://localhost:44382",
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345"))
				};
			});

			var serviceProvider = services.BuildServiceProvider();
			var logger = serviceProvider.GetService<ILogger<object>>();
			services.AddSingleton(typeof(ILogger), logger);

			services.AddScoped<IApplicationDbContext, ApplicationDbContext>();
			services.AddScoped<IUserService, UserService>();
			services.AddScoped<IComputerService, ComputerService>();

			services.AddScoped<IScopedInterface, LifecycleService>();
			services.AddTransient<ITransientInterface, LifecycleService>();
			services.AddSingleton<ISingletonInterface, LifecycleService>();

			services.AddSwaggerGen();


			services.AddControllersWithViews();
			// In production, the Angular files will be served from this directory
			services.AddSpaStaticFiles(configuration =>
			{
				configuration.RootPath = "ClientApp/dist";
			});
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();

				app.UseSwagger();
				app.UseSwaggerUI();
			}
			else
			{
				app.UseExceptionHandler("/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();
			if (!env.IsDevelopment())
			{
				app.UseSpaStaticFiles();
			}

			app.UseRouting();
			app.UseCors(builder => builder.AllowAnyOrigin());

			app.UseAuthentication();
			app.UseAuthorization();

			SeedDefaultData(app);

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller}/{action=Index}/{id?}");
			});

			app.UseSpa(spa =>
			{
				// To learn more about options for serving an Angular SPA from ASP.NET Core,
				// see https://go.microsoft.com/fwlink/?linkid=864501

				spa.Options.SourcePath = "ClientApp";

				if (env.IsDevelopment())
				{
					spa.UseAngularCliServer(npmScript: "start");
				}
			});
		}

		private void SeedDefaultData(IApplicationBuilder app)
		{
			var scopeFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();

			using (var scope = scopeFactory.CreateScope())
			{
				var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

				if(dbContext.Users.FirstOrDefault(u => u.FirstName == "John") == null)
				{
					User johnDoe = new User
					{
						FirstName = "John",
						LastName = "Doe"
					};

					User bStroustrup = new User
					{
						FirstName = "Bjarne",
						LastName = "Stroustrup"
					};

					User lTorvalds = new User
					{
						FirstName = "Linus",
						LastName = "Torvalds"
					};


					dbContext.Users.Add(johnDoe);
					dbContext.Users.Add(bStroustrup);
					dbContext.Users.Add(lTorvalds);

					dbContext.SaveChanges();

				}

				if(dbContext.ComputerManufacturers.FirstOrDefault() == null)
				{
					var computerManufacturerOne = new ComputerManufacturer
					{
						ManufacturerName = "Aser"
					};
					var computerManufacturerTwo = new ComputerManufacturer
					{
						ManufacturerName = "Toshiba"
					};

					dbContext.AddRange(computerManufacturerOne, computerManufacturerTwo);
					dbContext.SaveChanges();

					var computerModelAserOne = new ComputerModel
					{
						ModelName = "A1",
						ComputerManufacturerId = computerManufacturerOne.Id
					};

					var computerModelAserTwo = new ComputerModel
					{
						ModelName = "A2",
						ComputerManufacturerId = computerManufacturerOne.Id
					};


					var computerModelToshibaOne = new ComputerModel
					{
						ModelName = "Rapid",
						ComputerManufacturerId = computerManufacturerTwo.Id
					};

					var computerModelToshibaTwo = new ComputerModel
					{
						ModelName = "More fast",
						ComputerManufacturerId = computerManufacturerTwo.Id
					};

					dbContext.AddRange(computerModelAserOne, computerModelAserTwo, computerModelToshibaOne, computerModelToshibaTwo);
					dbContext.SaveChanges();

					//var asersTagOne = new ComputerModelTag
					//{
					//	TagName = "asersTagOne",
					//	TagMeta = "asersTagOne_Meta",
					//	TagExpiration = "4/6/2021",
					//	ComputerModelId = computerModelAserOne.Id
					//};
					//var asersTagTwo = new ComputerModelTag
					//{
					//	TagName = "asersTagTwo",
					//	TagMeta = "asersTagTwo_Meta",
					//	TagExpiration = "4/18/2021",
					//	ComputerModelId = computerModelAserOne.Id
					//};
					//var asersTagThree = new ComputerModelTag
					//{
					//	TagName = "asersTagThree",
					//	TagMeta = "asersTagThree_Meta",
					//	TagExpiration = "4/18/2025",
					//	ComputerModelId = computerModelAserOne.Id
					//};
					//var asersTagFour = new ComputerModelTag
					//{
					//	TagName = "asersTagFour",
					//	TagMeta = "asersTagFour_Meta",
					//	TagExpiration = "4/18/2030",
					//	ComputerModelId = computerModelAserOne.Id
					//};


					//dbContext.AddRange(asersTagOne, asersTagTwo, asersTagThree, asersTagFour);
					//dbContext.SaveChanges();
				}

				var compModlesTagsExpanded = dbContext.ComputerModelTags.ToList();
			}
		}
	}
}
