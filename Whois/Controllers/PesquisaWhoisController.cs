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
        public ActionResult Index(string dominio, bool? naoPesquisa) {
            if (string.IsNullOrEmpty(dominio))
                return View(db.Pesquisas.ToList());
            else {
                var pesquisas = db.Pesquisas.ToList().FindAll(pred => pred.Dominio == dominio.ToLower());
                if (pesquisas.Count > 0 || (naoPesquisa.HasValue && naoPesquisa.Value))
                    return View(pesquisas);
                else
                    return Pesquisar(dominio);
            }
        }

        // GET: PesquisaWhois/Pesquisar
        public ActionResult Pesquisar(string dominio) {
            try {
                if (string.IsNullOrEmpty(dominio) || dominio.Trim().Length == 0)
                    throw new Exception("Nada foi digitado");
                dominio = dominio.ToLower();
                var pesquisas = db.Pesquisas.ToList();
                pesquisas = pesquisas.FindAll(pred => pred.Dominio == dominio);
                pesquisas.OrderBy(pred => pred.DataPesquisa.Ticks);
                if (pesquisas.Count > 0 && (DateTime.Now - pesquisas.Last().DataPesquisa).TotalHours < 1)
                    throw new Exception("Não é possível pesquisar informações do mesmo domínio com intervalos menores de uma hora");
                try {
                    var pesquisa = PesquisaWhois.FromDomain(dominio);
                    db.Pesquisas.Add(pesquisa);
                    db.SaveChanges();
                    return RedirectToAction("Index", new { dominio = pesquisa.Dominio });
                } catch (WebException e) {
                    if (e.Response == null)
                        switch (e.Status) {
                            case WebExceptionStatus.NameResolutionFailure:
                                throw new Exception("Não foi possível estabelecer uma conexão ao jsonwhoisapi.com");
                            default:
                                throw e;
                        } else
                        switch ((int)(e.Response as HttpWebResponse).StatusCode) {
                            case 422:
                                throw new Exception("Domínio com formatação incorreta");
                            default:
                                throw e;
                        }
                }
            } catch (Exception e) {
                return RedirectToAction("Index", new { error = e.Message, dominio = dominio, naoPesquisa = true });
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