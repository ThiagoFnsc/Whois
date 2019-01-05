using System;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using System.Globalization;

namespace Whois.Models {
    public class PesquisaWhois {
        public int Id { get; set; }
        public string Dominio { get; set; }
        public DateTime DataPesquisa { get; set; }
        public bool Registrado { get; set; }
        public DateTime? DataRegistro { get; set; }
        public DateTime? UltimaAlteracao { get; set; }
        public DateTime? ExpiraEm { get; set; }
        public string NameServers { get; set; }
        public string JSON { get; set; }

        public PesquisaWhois() {
            DataPesquisa = DateTime.Now;
        }

        public static PesquisaWhois FromDomain(string domain) {
            try {
                var req = WebRequest.Create($"https://jsonwhoisapi.com/api/v1/whois?identifier={domain}");
                req.Method = "GET";
                req.Timeout = 10000;
                req.ContentType = "application/json";
                req.Headers.Add("authorization", "Basic " +
                    Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes("259670159:f6SO4xhUETb40A6SRNCTqw")));
                return FromJSON(new StreamReader(req.GetResponse().GetResponseStream()).ReadToEnd());
            } catch (WebException e) { throw e; }
        }

        public static PesquisaWhois FromJSON(string json) {
            DateTime? DateFromJSON(string date) {
                if (string.IsNullOrEmpty(date)) return null;
                return DateTime.ParseExact(date, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
            }
            dynamic raw = JsonConvert.DeserializeObject(json);
            if (raw.registered.Value) return new PesquisaWhois() {
                DataRegistro = DateFromJSON(raw.created),
                UltimaAlteracao = DateFromJSON(raw.changed),
                ExpiraEm = DateFromJSON(raw.expires),
                Dominio = raw.name,
                JSON = json,
                NameServers = String.Join("\n", raw.nameservers),
                Registrado = true
            };
            else return new PesquisaWhois() {
                Registrado = false,
                Dominio = raw.name
            };
        }

        public override string ToString() {
            return $"{Dominio} pesquisado {TimeAgo.Get(DataPesquisa)}";
        }
    }

    public class WhoisDbContext : DbContext {
        public DbSet<PesquisaWhois> Pesquisas { get; set; }
    }
}
