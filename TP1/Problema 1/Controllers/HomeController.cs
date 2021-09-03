using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Problema_1.Models;
using System;

using System.Net;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.IO;

namespace Problema_1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public string Problema1(int num)
        {
            try
            {
                int cuadrado = num * num;
                return cuadrado.ToString();
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        public float Problema2(float num,float den)
        {
            return num/den;
        }

        public IActionResult Problema3()
        {
            var url = $"https://apis.datos.gob.ar/georef/api/provincias?campos=id,nombre";
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method="GET";
            request.ContentType="application/json";
            request.Accept = "application/json";
            List<Provincia> lista = new();
            using (WebResponse response = request.GetResponse())
            {
                using(Stream strReader = response.GetResponseStream())
                {
                    if (strReader == null) return View();
                    using(StreamReader objReader = new StreamReader(strReader))
                    {
                        string responseBody = objReader.ReadToEnd();
                        ProvinciasArgentinas Provincias = JsonSerializer.Deserialize<ProvinciasArgentinas>(responseBody);
                        
                        foreach (Provincia prov in Provincias.Provincias)
                        {
                            lista.Add(prov);
                        }
                    }
                }
            }
            ViewBag.lista = lista;
            return View();
        }

        public string Problema4(float km, float lt)
        {
            float calculo = km / lt;
            return "Recorrio "+calculo.ToString()+" km por litro";
        }

        // Clases para Problema 3
        //*********************************************
        public class Parametros
        {
            [JsonPropertyName("campos")]
            public List<string> Campos { get; set; }
        }

        public class Provincia
        {
            [JsonPropertyName("id")]
            public string Id { get; set; }

            [JsonPropertyName("nombre")]
            public string Nombre { get; set; }
        }

        public class ProvinciasArgentinas
        {
            [JsonPropertyName("cantidad")]
            public int Cantidad { get; set; }

            [JsonPropertyName("inicio")]
            public int Inicio { get; set; }

            [JsonPropertyName("parametros")]
            public Parametros Parametros { get; set; }

            [JsonPropertyName("provincias")]
            public List<Provincia> Provincias { get; set; }

            [JsonPropertyName("total")]
            public int Total { get; set; }
        }
        //*********************************************



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
