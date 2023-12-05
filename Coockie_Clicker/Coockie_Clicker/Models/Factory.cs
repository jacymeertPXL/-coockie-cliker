﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coockie_Clicker.Classes
{
    internal class Factory
    {
        public int Teller { get; set; }
        public double Prijs { get; set; }
        public double PrijsBonus { get; set; }
        public bool CursorButtonVisible { get; set; }
        public Factory() {
            Teller = 1;
            Prijs = 130000;
            PrijsBonus = 130000;
            CursorButtonVisible = false;
        }

        public void PrijsVerhogenBonus()
        {
            PrijsBonus = Prijs * Math.Pow(10, 2 + 3 * Teller);
        }

        public void PrijsVerhogen()
        {
            Prijs = Prijs * Math.Pow(1.15, Teller);
        }

        public void GekockteBonusFactory()
        {
            Teller++;
        }

    }
}