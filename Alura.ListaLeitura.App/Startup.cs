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
            var _repo = new LivroRepositorioCSV();

            var routes = new Dictionary<string, string>
            {
                { "/Livros/ParaLer", _repo.ParaLer.ToString() },
                { "/Livros/Lendo", _repo.Lendo.ToString() },
                { "/Livros/Lidos", _repo.Lidos.ToString() }
            };

            if (routes.ContainsKey(context.Request.Path))
            {
                return context.Response.WriteAsync(routes[context.Request.Path]);
            }
            else
            {
                // Escreve o conteúdo de livros para ler na Response da requisição
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                return context.Response.WriteAsync("Rota inexistente");
            }
        }
    }
}