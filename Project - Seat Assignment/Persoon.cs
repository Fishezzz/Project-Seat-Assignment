using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project___Seat_Assignment
{
    class Persoon
    {
        public Persoon()
        {
            voornaam = " ";
            achternaam = " ";
        }
        public Persoon(string pVoornaam, string pAchternaam)
        {
            voornaam = pVoornaam;
            achternaam = pAchternaam;
        }

        private string voornaam;
        public string Voornaam
        {
            get { return voornaam; }
            set { voornaam = value; }
        }

        private string achternaam;
        public string Achternaam
        {
            get { return achternaam; }
            set { achternaam = value; }
        }
    }
}
