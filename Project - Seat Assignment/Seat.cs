using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace Project___Seat_Assignment
{
    class Seat
    {
        public Seat(int pSeatNumber, User pOccupier, TextBox pSeatTextBox)
        {
            seatNumber = pSeatNumber;
            occupier = pOccupier;
            if (pOccupier != null)
                clanColor = occupier.Clan.ClanColor;
            else
                clanColor = Brushes.White;
            seatTextBox = pSeatTextBox;
        }

        private int seatNumber;
        public int SeatNumber
        {
            get { return seatNumber; }
            set { seatNumber = value; }
        }

        private User occupier;
        public User Occupier
        {
            get { return occupier; }
            set { occupier = value; }
        }

        private Brush clanColor;
        public Brush ClanColor
        {
            get { return clanColor; }
            set { clanColor = value; }
        }

        private TextBox seatTextBox;
        public TextBox SeatTextBox
        {
            get { return seatTextBox; }
            set { seatTextBox = value; }
        }
    }
}
