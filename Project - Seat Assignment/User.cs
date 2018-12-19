using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project___Seat_Assignment
{
    class User : Persoon
    {
        public User(int pId, string pUsername, Clan pClan)
        {
            id = pId;
            username = pUsername;
            clan = pClan;
            assignedSeat = 0;
        }
        public User(int pId, string pUsername, Clan pClan, string pVoornaam, string pAchternaam)
            :base(pVoornaam, pAchternaam)
        {
            id = pId;
            username = pUsername;
            clan = pClan;
            assignedSeat = 0;
        }
        public User(int pId, string pUsername, Clan pClan, int pAssignedSeat, string pVoornaam, string pAchternaam)
            :base(pVoornaam, pAchternaam)
        {
            id = pId;
            username = pUsername;
            clan = pClan;
            assignedSeat = pAssignedSeat;
        }

        private int id;
        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        private string username;
        public string Username
        {
            get { return username; }
            set { username = value; }
        }

        private Clan clan;
        public Clan Clan
        {
            get { return clan; }
            set { clan = value; }
        }

        private int assignedSeat;
        public int AssignedSeat
        {
            get { return assignedSeat; }
            set { assignedSeat = value; }
        }
    }
}
