using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using AdminMVC.Models;
using Newtonsoft.Json;

namespace AdminMVC.Controllers
{
    public class LoginController : Controller
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        // GET: Login
        public ActionResult Index()
        {            
           
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
        

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(PersonalModel login)
        {
            
            if (login.AnvandarNamn == null || login.Losenord == null)
                {
                    ModelState.AddModelError("", "Du måste fylla i både användarnamn och lösenord");
                    return View();
                }
                bool validUser = false;

                //Kontrollera mot webbservice
                validUser = CheckUser(login.AnvandarNamn, login.Losenord);

            if (validUser == true)
                {
                    
                    System.Web.Security.FormsAuthentication.RedirectFromLoginPage(login.AnvandarNamn, false);
                    return RedirectToAction("Index", "Login");
                }
                ModelState.AddModelError("", "Inloggningen ej godkänd, Försök Igen!");
                Logger.Error("Inloggningen ej godkänd");
                return View();                                           
        }

        private bool CheckUser(string username, string password)
        {
            try
            {
                PersonalModel ResponseAnv = new PersonalModel();
                ResponseAnv.AnvandarNamn = username;
                ResponseAnv.Losenord = password;
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://193.10.202.74/inlogg/personals");
                    var response = client.PostAsJsonAsync("Login", ResponseAnv).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        var PersonalResponse = response.Content.ReadAsStringAsync().Result;
                        ResponseAnv = JsonConvert.DeserializeObject<PersonalModel>(PersonalResponse);
                    }
                }

                if (ResponseAnv != null)
                {
                    if (ResponseAnv.BehorighetsNiva == 3)
                        return true;
                    else
                        return false;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {

                Logger.Error(ex);
            }
            return false;
        }
    }
}