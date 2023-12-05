using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coockie_Clicker.Achievements
{
    internal class Coockies_Clicked
    {
        public int RequiredCookies { get; set; }
        public bool IsUnlocked { get; set; }

        public Coockies_Clicked()
        {
            RequiredCookies = 20;
            IsUnlocked = false;
        }
    }
}
