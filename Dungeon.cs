using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextGame
{
    internal class Dungeon
    {
        private Monster[] monsters;

        public void Battle()
        {
            
                
        }

        public void MonsterAppear()
        {
            int num = new Random().Next(1, 5);
            monsters = new Monster[num];

            

        }



    }
}
