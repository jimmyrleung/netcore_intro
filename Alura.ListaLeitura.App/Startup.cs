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
        private LivroController _livroController;

        public Startup()
        {
            this._livroController = new LivroController();
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
            routeBuilder.MapRoute("Livros/ParaLer", _livroController.LivrosParaLer);
            routeBuilder.MapRoute("Livros/Lendo", _livroController.LivrosLendo);
            routeBuilder.MapRoute("Livros/Lidos", _livroController.LivrosLidos);
            routeBuilder.MapRoute("Livros/Cadastro/{nome}/{autor}", _livroController.NovoLivroParaLer);
            routeBuilder.MapRoute("Livros/Cadastro", _livroController.ExibeFormulario);
            routeBuilder.MapRoute("Livros/Cadastro/Incluir", _livroController.IncluirLivro);
            routeBuilder.MapRoute("Livros/{id:int}", _livroController.ExibeDetalhes);

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
    }
}