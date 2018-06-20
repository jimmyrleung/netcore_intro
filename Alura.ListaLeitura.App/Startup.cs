﻿using Alura.ListaLeitura.App.Repositorio;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Alura.ListaLeitura.App
{
    public class Startup
    {
        // Recebe uma instancia de IApplicationBuilder via Injeção de dependência
        // TODO: Criar Configure's de acordo com as variáveis de ambiente
        // https://docs.microsoft.com/pt-br/aspnet/core/fundamentals/environments?view=aspnetcore-2.1
        public void Configure(IApplicationBuilder app)
        {
            // Ao executar a aplicação, o método será executado e o usuário irá receber na tela
            // o resultado do método
            app.Run(LivrosParaLer);
        }

        // Um método do tipo Task funciona de forma assíncrona
        public Task LivrosParaLer(HttpContext context)
        {
            var _repo = new LivroRepositorioCSV();

            // Escreve o conteúdo de livros para ler na Response da requisição
            return context.Response.WriteAsync(_repo.ParaLer.ToString());
        }
    }
}