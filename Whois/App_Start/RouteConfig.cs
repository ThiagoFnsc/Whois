using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Web.Routing;
using Whois.Models;

namespace Whois {
    public class RouteConfig {
        public static void RegisterRoutes(RouteCollection routes) {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "PesquisaWhois", action = "Index", id = UrlParameter.Optional }
            );

            //routes.MapRoute(name: "Pesquisar", url: "pesquisar/{dominio}", defaults: new { });

            //var pesquisa = PesquisaWhois.FromDomain("facebook.com");
            //Console.WriteLine(pesquisa.Dominio);
        }
    }
}