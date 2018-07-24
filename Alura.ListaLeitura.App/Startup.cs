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
            // Utilizar o MVC com o roteamento padrão
            app.UseMvcWithDefaultRoute();
            app.UseDeveloperExceptionPage(); // Deve ser utilizado somente em desenvolvimento

            if (env.IsDevelopment())
            {
                // Development only configuration.
            }
        }

        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddRouting();
            services.AddMvc();
        }
        #endregion ConfigureSection
    }
}