using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextGame
{
    public class Monster
    {
        public string Name { get; set; }
        public int Level { get; }
        public int Atk { get; }
        public int Hp { get; set; }
        public int MaxHp { get; private set;}
        //public string Type { get; }
        public bool IsLife { get; private set; }
        public int DropGold { get; }
        public int DropExp { get; }

        public Monster(string name, int level, int atk, int hp, int dropGold, int dropExp)
        {
            Name = name;
            Level = level;
            Atk = atk;
            Hp = hp;
            MaxHp = hp;
            IsLife = true;
            //Type = name;
            DropGold = dropGold;
            DropExp = dropExp;
        }

        public void Dead()
        {
            if (Hp <= 0)
            {
                IsLife = false;                
            }
        }

        public void Attack(Player player)
        {

            if (Atk <= player.Def) return;            

            player.Hp -= (Atk - player.Def);

            if(player.Hp < 0) player.Hp = 0;        
        }

        //public void Respawn()
        //{
        //    IsLife = true;
        //    Hp = MaxHp;
        //}



    }
}
