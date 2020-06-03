using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminMVC.Models
{
    public class PersonalModel
    {
       public int Id { get; set; }

        public string AnvandarNamn { get; set; }

        public string Losenord { get; set; }

        public string Namn { get; set; }

        public string Efternamn { get; set; }

        public int BehorighetsNiva { get; set; }

        public int RefID { get; set; }

        public string Roll { get; set; }
    }
}