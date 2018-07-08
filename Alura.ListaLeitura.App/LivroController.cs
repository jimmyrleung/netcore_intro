using Alura.ListaLeitura.App.Negocio;
using Alura.ListaLeitura.App.Repositorio;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Alura.ListaLeitura.App
{
    class LivroController
    {
        private LivroRepositorioCSV _repo;

        public LivroController()
        {
            _repo = new LivroRepositorioCSV();
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

        private string CarregaArquivoHtml(string nomeArquivo)
        {
            string nomeCompleto = $"../../../HTML/{nomeArquivo}.html";
            using (var arquivo = File.OpenText(nomeCompleto))
            {
                return arquivo.ReadToEnd();
            }
        }

        public Task ExibeFormulario(HttpContext context)
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

        public Task ExibeDetalhes(HttpContext context)
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
    }
}
