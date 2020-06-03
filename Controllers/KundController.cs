using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Mvc;
using AdminMVC.Models;
using Newtonsoft.Json;

namespace AdminMVC.Controllers
{
    public class KundController : Controller
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        // GET: Kund
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }
        [Authorize]
        public ActionResult GetKund()
        {         
            List<Kund> KundLista = new List<Kund>();
            HttpClient client = new HttpClient();
            try
            {
                client.BaseAddress = new Uri("http://193.10.202.72/Kundservice/Kunder");
                // Add an Accept header for JSON format.    
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.GetAsync("kunder").Result;  // Blocking call!
                if (response.IsSuccessStatusCode)
                {
                    var products = response.Content.ReadAsStringAsync().Result;
                    KundLista = JsonConvert.DeserializeObject<List<Kund>>(products);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Det gick inte att ansluta till servern! (404)");
                return RedirectToAction("Index");
            }
            // List all Names. 
            

            return View(KundLista);
        }
        [Authorize]
        public ActionResult PutKund(int id) /*Metoden kommer ifrån denna källa: https://www.tutorialsteacher.com/webapi/implement-put-method-in-web-api*/
        {
            
            
                Kund Kunden = null;
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://193.10.202.72/Kundservice/");
                    var responseTask = client.GetAsync("Kunder/" + id.ToString());
                    responseTask.Wait();

                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var readTask = result.Content.ReadAsAsync<Kund>();
                        readTask.Wait();
                        Kunden = readTask.Result;
                    }
                    
                }
                
                return View(Kunden);
            
           
           
        }

        [HttpPost]
        public ActionResult PutKund(Kund Kunden) /*Metoden kommer ifrån denna källa: https://www.tutorialsteacher.com/webapi/implement-put-method-in-web-api*/
        {
           
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://193.10.202.72/Kundservice/");
                    var putTask = client.PutAsJsonAsync<Kund>("Kunder/" + Kunden.InloggningsId, Kunden);
                    putTask.Wait();

                    var result = putTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        return RedirectToAction("GetKund");
                    }

                }
            Logger.Error("Det gick inte att göra förändringen av kunden");
            return View(Kunden);
            
            
        }
        [Authorize]
        public ActionResult DeleteKund(int id)
        {
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri("http://193.10.202.72/Kundservice/");
                var deleteTask = client.DeleteAsync("Kunder/" + id.ToString());

                var result = deleteTask.Result;
                DeleteAnv(id);

                if (result.IsSuccessStatusCode)

                    
                    return RedirectToAction("GetKund");

            }
            Logger.Error("Det gick inte att radera kunden");
            return RedirectToAction("GetKund");
        }
        public void DeleteAnv(int id)
        {
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri("http://193.10.202.74/inlogg/");
                var deleteTask = client.DeleteAsync("anvandares/" + id.ToString());

                var result = deleteTask.Result;
            }
        }
             
        
    }
}