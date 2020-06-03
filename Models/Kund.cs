using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminMVC.Models
{
    public class Kund
    {
        public int InloggningsId { get; set; }
        public string Email { get; set; }

        public string Losenord { get; set; }

        public string Fornamn { get; set; }

        public string Efternamn { get; set; }

        public string PersonNr { get; set; }

        public string TelefonNr { get; set; }

        public int Bonuspoang { get; set; }

        
    }
}