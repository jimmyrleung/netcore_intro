using Alura.ListaLeitura.App.Negocio;
using Alura.ListaLeitura.App.Repositorio;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Alura.ListaLeitura.App
{
    public class LivroController : Controller
    {
        private LivroRepositorioCSV _repo;

        public LivroController()
        {
            _repo = new LivroRepositorioCSV();
        }

        public string ParaLer(HttpContext context)
        {
            var html = CarregaArquivoHtml("listaLivros");
            var listaLivros = "";
            foreach (var livro in _repo.ParaLer.Livros)
            {
                listaLivros += $"<li>{livro.ToString()}</li>";
            }
            return html.Replace("{listaLivros}", listaLivros).Replace("{titulo}", _repo.ParaLer.Titulo);
        }

        public string Lendo(HttpContext context)
        {
            var html = CarregaArquivoHtml("listaLivros");
            var listaLivros = "";
            foreach (var livro in _repo.Lendo.Livros)
            {
                listaLivros += $"<li>{livro.ToString()}</li>";
            }
            return html.Replace("{listaLivros}", listaLivros).Replace("{titulo}", _repo.Lendo.Titulo);
        }

        public string Lidos(HttpContext context)
        {
            var html = CarregaArquivoHtml("listaLivros");
            var listaLivros = "";
            foreach (var livro in _repo.Lidos.Livros)
            {
                listaLivros += $"<li>{livro.ToString()}</li>";
            }
            return html.Replace("{listaLivros}", listaLivros).Replace("{titulo}", _repo.Lidos.Titulo);
        }

        private string CarregaArquivoHtml(string nomeArquivo)
        {
            string nomeCompleto = $"../../../HTML/{nomeArquivo}.html";
            using (var arquivo = System.IO.File.OpenText(nomeCompleto))
            {
                return arquivo.ReadToEnd();
            }
        }

        public IActionResult ExibeFormulario(HttpContext context)
        {
            //var html = CarregaArquivoHtml("formLivro");
            return View("formLivro");
        }

        public string Incluir(HttpContext context)
        {
            var livro = new Livro()
            {
                Titulo = Convert.ToString(context.Request.Form["titulo"]),
                Autor = Convert.ToString(context.Request.Form["autor"])
            };

            _repo.Incluir(livro);
            return "Livro incluído com sucesso.";
        }

        public string ExibeDetalhes(int id)
        {
            var livro = _repo.Todos.First(l => l.Id == id);
            return livro.Detalhes();
        }

        public string NovoLivroParaLer(HttpContext context)
        {
            // Como a rota que acessa essa RequestDelegate é uma "Template Route", ou seja, que tem
            // um padrão definido, podemos acessar esses valores através do método "GetRouteValue"
            var livro = new Livro()
            {
                Titulo = Convert.ToString(context.GetRouteValue("nome")),
                Autor = Convert.ToString(context.GetRouteValue("autor"))
            };

            _repo.Incluir(livro);
            return "Livro incluído com sucesso.";
        }

        public string Teste()
        {
            return "Teste";
        }
    }
}
