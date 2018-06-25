using Alura.ListaLeitura.App.Negocio;
using Alura.ListaLeitura.App.Repositorio;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Alura.ListaLeitura.App
{
    public class Startup
    {
        #region Constructor
        private LivroRepositorioCSV _repo;

        public Startup()
        {
            _repo = new LivroRepositorioCSV();
        }
        #endregion
        
        #region ConfigureSection
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
            routeBuilder.MapRoute("Livros/Cadastro/{nome}/{autor}", NovoLivroParaLer);
            routeBuilder.MapRoute("Livros/{id:int}", ExibeDetalhes);

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
        #endregion ConfigureSection

        #region Tasks
        private Task ExibeDetalhes(HttpContext context)
        {
            int id = Convert.ToInt32(context.GetRouteValue("id"));
            var livro = _repo.Todos.First(l => l.Id == id);
            return context.Response.WriteAsync(livro.Detalhes());
        }

        public Task NovoLivroParaLer(HttpContext context)
        {
            // Como a rota que acessa essa RequestDelegate é uma "Template Route", ou seja, que tem
            // um padrão definido, podemos acessar esses valores através do método "GetRouteValue"
            var livro = new Livro()
            {
                Titulo = Convert.ToString(context.GetRouteValue("nome")),
                Autor = Convert.ToString(context.GetRouteValue("autor"))
            };

            _repo.Incluir(livro);
            return context.Response.WriteAsync("Livro incluído com sucesso.");
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
        #endregion Tasks
    }
}