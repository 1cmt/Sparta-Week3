using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Sparta_week3
{
    internal class Player
    {
        [JsonProperty("privateField")]
        private string _name;
        public string Name { get { return _name; } }
        private string _job;
        public string Job { get { return _job; } }
        public int Level;
        public float Atk;
        public int Def;
        public int Ctl;
        public int Mana;
        public int Hp;
        public int Gold;
        public int Exp = 10;
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
            Hp = hp;
            if(job == "Archer") Atk = (int)(atk * 1.2f); 
            else Atk = atk;
            Def = job == "Warrior" ? (int)(def * 1.2f) : def;
            Ctl = job == "assassin" ? (int)(Ctl * 1.2f) : Ctl;
            Mana = job == "Wizard" ? (int)(Mana * 1.2f) : Mana;
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
