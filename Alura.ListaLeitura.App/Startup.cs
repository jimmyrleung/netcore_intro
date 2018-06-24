using Alura.ListaLeitura.App.Repositorio;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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

        // TODO: Criar Configure's de acordo com as variáveis de ambiente
        // https://docs.microsoft.com/pt-br/aspnet/core/fundamentals/environments?view=aspnetcore-2.1
        // Recebe uma instancia de IApplicationBuilder via Injeção de dependência
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                // Development only configuration.
            }

            // Ao executar a aplicação, o método será executado e o usuário irá receber na tela
            // o resultado do método
            app.Run(Routing);
        }

        // Um método do tipo Task funciona de forma assíncrona
        public Task Routing(HttpContext context)
        {
            // Request delegate: Tipo de método que sabe processar uma requisição HTTP
            var routes = new Dictionary<string, RequestDelegate>
            {
                { "/Livros/ParaLer", LivrosParaLer },
                { "/Livros/Lendo", LivrosLendo },
                { "/Livros/Lidos", LivrosLidos }
            };

            if (routes.ContainsKey(context.Request.Path))
            {
                // Executa um request delegate sob um determinado contexto http
                return routes[context.Request.Path].Invoke(context);
            }
            else
            {
                // Escreve o conteúdo de livros para ler na Response da requisição
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                return context.Response.WriteAsync("Rota inexistente");
            }
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