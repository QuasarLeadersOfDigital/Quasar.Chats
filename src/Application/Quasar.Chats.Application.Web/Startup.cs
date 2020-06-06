using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Quasar.Chats.Application.Web.Data;

namespace Quasar.Chats.Application.Web
{
	public class Startup
	{
		private readonly IConfiguration _configuration;

		public Startup(IConfiguration configuration)
		{
			_configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
		}

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddMvc();

			services.AddControllers();
			services.AddHealthChecks();

			AddSwagger(services);
			
			services.AddCors(option =>
			{
				option.AddPolicy("CorsPolicy", builder =>
					builder.WithOrigins("http://localhost:4200")
						.AllowAnyMethod()
						.AllowAnyHeader()
						.AllowCredentials());
			});

			services.AddSingleton<IChatsData, ChatsData>();
			services.AddSingleton<IMessagesData, MessagesData>();
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			
			app.UseSwagger();
			app.UseSwaggerUI(options =>
			{
				options.SwaggerEndpoint(
					url: "/swagger/v1/swagger.json",
					name: "Quasar Chats API v1");
				options.RoutePrefix = "docs";
			});
			
			app.UseCookiePolicy();
			
			app.UseCors("CorsPolicy");
			app.UseRouting();
			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
				endpoints.MapHealthChecks("/healthcheck", new HealthCheckOptions
				{
					AllowCachingResponses = false,
					ResponseWriter = WriteResponse
				});
			});
		}
		
		private static void AddSwagger(IServiceCollection services)
		{
			services.AddSwaggerGen(options =>
			{
				options.SwaggerDoc("v1", new OpenApiInfo
				{
					Title = "Quasar Chats API",
					Version = "v1"
				});

				// Set the comments path for the Swagger JSON and UI.
				string xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
				string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
				options.IncludeXmlComments(xmlPath);
			});
		}
		
		private static Task WriteResponse(HttpContext context, HealthReport result)
		{
			context.Response.ContentType = "application/json; charset=utf-8";

			var options = new JsonWriterOptions
			{
				Indented = true
			};

			using (var stream = new MemoryStream())
			{
				using (var writer = new Utf8JsonWriter(stream, options))
				{
					writer.WriteStartObject();
					writer.WriteString("status", result.Status.ToString());
					writer.WriteEndObject();
				}

				string json = Encoding.UTF8.GetString(stream.ToArray());

				return context.Response.WriteAsync(json);
			}
		}
	}
}
