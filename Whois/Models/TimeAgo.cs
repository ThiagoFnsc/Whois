using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Whois.Models {
    public class TimeAgo {
        public static string Get(DateTime? date) {
            if (date.HasValue)
                return Get(date.Value);
            else return "não definido";
        }

        public static string Get(DateTime date) {
            string text = string.Empty;
            void Format(double numero, string singular, string plural) {
                numero = Math.Round(numero);
                text += $"{numero} {(numero == 1 ? singular : plural)}";
            }
            bool passado = new TimeSpan(DateTime.Now.Ticks - date.Ticks).TotalSeconds > 0;
            TimeSpan diff;
            if (passado)
                diff = new TimeSpan(DateTime.Now.Ticks - date.Ticks);
            else
                diff = new TimeSpan(date.Ticks - DateTime.Now.Ticks);

            if (!passado) text = "daqui ";

            if (Math.Round(diff.TotalMinutes) < 1.0)
                Format(diff.TotalSeconds, "segundo", "segundos");
            else if (diff.TotalHours < 1.0)
                Format(diff.TotalMinutes, "minuto", "minutos");
            else if (diff.TotalDays < 1.0)
                Format(diff.TotalHours, "hora", "horas");
            else if (diff.TotalDays < 30.0)
                Format(diff.TotalDays, "dia", "dias");
            else if (diff.TotalDays < 365.25)
                Format(diff.TotalDays / 30.4375, "mês", "meses");
            else
                Format(diff.TotalDays / 365.25, "ano", "anos");

            if (passado) text += " atrás";
            return text;
        }
    }
}