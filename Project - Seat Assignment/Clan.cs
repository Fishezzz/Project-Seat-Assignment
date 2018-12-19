using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Project___Seat_Assignment
{
    class Clan
    {
        public Clan(int pId, string pClanName, Brush pColor)
        {
            id = pId;
            clanName = pClanName;
            clanColor = pColor;
            users = new List<User>();
        }
        public Clan(int pId, string pClanName, Brush pColor, List<User> pUsers)
        {
            id = pId;
            clanName = pClanName;
            clanColor = pColor;
            users = pUsers;
        }

        private int id;
        public int ID
        {
            get { return id; }
            set { id = value; }
        }


        private string clanName;
        public string ClanName
        {
            get { return clanName; }
            set { clanName = value; }
        }

        private List<User> users;
        public List<User> Users
        {
            get { return users; }
            set { users = value; }
        }

        private Brush clanColor;
        public Brush ClanColor
        {
            get { return clanColor; }
            set { clanColor = value; }
        }
    }
}
