using AcessoDados.BLL.Services;
using AcessoDados.DAL.EntityCodeFirst;
using AcessoDados.DAL.EntityCodeFirst.Modelos;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;

namespace AcessoDados.API
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
			//Disponibilizar o usuário logado através de DI
			services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
			services.AddTransient<IPrincipal>
				(provider => provider.GetService<IHttpContextAccessor>().HttpContext.User);

			//Adiciona os repositórios
			services.AddScoped<IRepositorioVideos, RepositorioVideos>(); // Usa oespecifico
			services.AddScoped<IRepositorioComum<Categoria>, RepositorioComum<Categoria>>(); // cria instâncias genericas
			services.AddScoped<IRepositorioComum<Responsavel>, RepositorioComum<Responsavel>>();// cria instâncias genericas
			services.AddScoped<IRepositorioComum<VideoCategoria>, RepositorioComum<VideoCategoria>>();// cria instâncias genericas


			//Adiciona os servicos
			services.AddTransient<CategoriaService>(); // Usa oespecifico

			services.AddControllers();

			services.AddSwaggerGen();

			services.AddTransient<VideoService>(); //
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseHttpsRedirection();

			app.UseRouting();

			app.UseSwagger();

			app.UseSwaggerUI(c =>
			{
				c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
			});

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
