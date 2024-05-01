using Sparta_week3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextGame
{
    public class Monster
    {
        public string Name { get; }
        public int Level { get; }
        public int Atk { get; }
        public int Hp { get; set; }

        public int MaxHp { get; private set;}


        public bool IsLife { get; private set; }

        public Monster(string name, int level, int atk, int hp)
        {
            Name = name;
            Level = level;
            Atk = atk;       
            Hp = hp;
            MaxHp = hp;
            IsLife = true;
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
            player.Hp -= Atk;
        
        }

        public void Respawn(Monster monster)
        {
            monster.IsLife = true;
            monster.Hp = MaxHp;

        }



    }
}
