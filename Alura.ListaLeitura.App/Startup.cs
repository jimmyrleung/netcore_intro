using Alura.ListaLeitura.App.Negocio;
using Alura.ListaLeitura.App.Repositorio;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
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
            routeBuilder.MapRoute("Livros/Cadastro", ExibeFormulario);
            routeBuilder.MapRoute("Livros/Cadastro/Incluir", IncluirLivro);
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
        private Task ExibeFormulario(HttpContext context)
        {
            var html = CarregaArquivoHtml("formLivro");
            return context.Response.WriteAsync(html);
        }

        public Task IncluirLivro(HttpContext context)
        {
            var livro = new Livro()
            {
                Titulo = Convert.ToString(context.Request.Form["titulo"]),
                Autor = Convert.ToString(context.Request.Form["autor"])
            };

            _repo.Incluir(livro);
            return context.Response.WriteAsync("Livro incluído com sucesso.");
        }

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
            var html = CarregaArquivoHtml("listaLivros");
            var listaLivros = "";
            foreach (var livro in _repo.ParaLer.Livros)
            {
                listaLivros += $"<li>{livro.ToString()}</li>";
            }
            return context.Response.WriteAsync(html.Replace("{listaLivros}", listaLivros).Replace("{titulo}", _repo.ParaLer.Titulo));
        }

        public Task LivrosLendo(HttpContext context)
        {
            var html = CarregaArquivoHtml("listaLivros");
            var listaLivros = "";
            foreach (var livro in _repo.Lendo.Livros)
            {
                listaLivros += $"<li>{livro.ToString()}</li>";
            }
            return context.Response.WriteAsync(html.Replace("{listaLivros}", listaLivros).Replace("{titulo}", _repo.Lendo.Titulo));
        }

        public Task LivrosLidos(HttpContext context)
        {
            var html = CarregaArquivoHtml("listaLivros");
            var listaLivros = "";
            foreach (var livro in _repo.Lidos.Livros)
            {
                listaLivros += $"<li>{livro.ToString()}</li>";
            }
            return context.Response.WriteAsync(html.Replace("{listaLivros}", listaLivros).Replace("{titulo}", _repo.Lidos.Titulo));
        }
        #endregion Tasks

        private string CarregaArquivoHtml(string nomeArquivo)
        {
            string nomeCompleto = $"../../../HTML/{nomeArquivo}.html";
            using (var arquivo = File.OpenText(nomeCompleto))
            {
                return arquivo.ReadToEnd();
            }
        }
    }
}