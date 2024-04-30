using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Sparta_week3
{
    internal class Player
    {
        private string _name;
        private string _job;
        public int Level;
        public float Atk;
        public int Def;
        //크리티컬 public int Ctl;
        //마나 public int Mana;
        public int Hp;
        public int Gold;
        public int Exp=10;
        public string JobChange
        {
            set => _job = value;
        }
        
        public Player(string name, string job, int level = 1, int atk = 10, int def = 10, int hp = 100, int gold = 1000 /*float ctl=0.15f, int mana = 50*/)  
        {
            _name = name;
            _job = job;
            Level = level;
            Gold = gold;
            if(job == "Archer") Atk = (int)(atk * 1.2f); 
            else Atk = atk;
            Def = job == "Warrior" ? (int)(def * 1.2f) : def;
            //Ctl = job = "assassin" ? ctl * 1.2f : ctl;
            Hp = hp;
            //Mana = job == "Wizard" ? (int)(mana * 1.2f) : mana;
        }
        public void Levelup(int level,int exp)
        {
            if (Exp == exp)
                Exp += 20 + (level) *5;
            Atk += 0.5f;
            Def += 1;
        }
    }
}
