using Alura.ListaLeitura.App.Repositorio;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Alura.ListaLeitura.App
{
    public class Startup
    {
        private LivroRepositorioCSV _repo;

        public Startup()
        {
            _repo = new LivroRepositorioCSV();
        }

        // Recebe uma instancia de IApplicationBuilder via Injeção de dependência
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // Criamos um RouteBuilder que irá gerar nosso roteamento
            var routeBuilder = new RouteBuilder(app);

            // Mapeamos as rotas e os RequestDelegates para cada rota
            // Request delegate: Tipo de método que sabe processar uma requisição HTTP
            routeBuilder.MapRoute("Livros/ParaLer", LivrosParaLer);
            routeBuilder.MapRoute("Livros/Lendo", LivrosLendo);
            routeBuilder.MapRoute("Livros/Lidos", LivrosLidos);
            
            // Construímos de fato o objeto responsável pelo roteamento
            var routes = routeBuilder.Build();

            // Solicitamos ao app que use nosso objeto de roteamento
            app.UseRouter(routes);

            if (env.IsDevelopment())
            {
                // Development only configuration.
            }
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRouting();
        }

        public Task LivrosParaLer(HttpContext context)
        {
            return context.Response.WriteAsync(_repo.ParaLer.ToString());
        }

        public Task LivrosLendo(HttpContext context)
        {
            return context.Response.WriteAsync(_repo.Lendo.ToString());
        }

        public Task LivrosLidos(HttpContext context)
        {
            return context.Response.WriteAsync(_repo.Lidos.ToString());
        }
    }
}