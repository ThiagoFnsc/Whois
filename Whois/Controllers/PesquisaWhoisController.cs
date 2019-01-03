using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Whois.Models;

namespace Whois.Controllers {
    public class PesquisaWhoisController : Controller {
        private WhoisDbContext db = new WhoisDbContext();

        // GET: PesquisaWhois
        public ActionResult Index(string dominio) {
            if (string.IsNullOrEmpty(dominio))
                return View(db.Pesquisas.ToList());
            else {
                var pesquisas = db.Pesquisas.ToList().FindAll(pred => pred.Dominio == dominio.ToLower());
                if (pesquisas.Count > 0)
                    return View(pesquisas);
                else
                    return Pesquisar(dominio);
            }
        }

        // GET: PesquisaWhois/Pesquisar
        public ActionResult Pesquisar(string dominio) {
            try {
                dominio = dominio.ToLower();
                var pesquisas = db.Pesquisas.ToList();
                pesquisas = pesquisas.FindAll(pred => pred.Dominio == dominio);
                pesquisas.OrderBy(pred => pred.DataPesquisa.Ticks);
                if (pesquisas.Count > 0 && (DateTime.Now - pesquisas.Last().DataPesquisa).TotalHours < 1)
                    throw new Exception("Não é possível pesquisar informações do mesmo domínio com intervalos menores de uma hora");
                var pesquisa = PesquisaWhois.FromDomain(dominio);
                db.Pesquisas.Add(pesquisa);
                db.SaveChanges();
                return RedirectToAction("Index", new { dominio = pesquisa.Dominio });
            } catch (Exception e) {
                return RedirectToAction("Index", new { error = e.Message, dominio = dominio });
            }
        }

        protected override void Dispose(bool disposing) {
            if (disposing) {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}