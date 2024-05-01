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
        public float Ctl = 0.15f;
        public int Mana=50;
        public int Hp;
        public int Gold;
        public int Cexp=0;
        public int Texp { get; private set; }
        public string JobChange
        {
            set => _job = value;
        }

        public Player(string name, string job, int level = 1, int atk = 10, int def = 10, int hp = 100, int gold = 1000, int exp = 10)
        {
            _name = name;
            _job = job;
            Level = level;
            Gold = gold;
            Hp = hp;
            Texp = exp;
            if (job == "Archer") Atk = (int)(atk * 1.2f);
            else Atk = atk;
            Def = job == "Warrior" ? (int)(def * 1.2f) : def;
            Ctl = job == "assassin" ? (int)(Ctl * 1.2f) : Ctl;
            Mana = job == "Wizard" ? (int)(Mana * 1.2f) : Mana;
        }
        public void Levelup(ref int level, ref int cexp)
        {
            if (cexp == Texp)
            {
                Texp += 20 + level * 5;
                Atk += 0.5f;
                Def ++;
                level++;
                cexp = 0;
            }
        }
        public string InputName()
        {
            Console.Clear();
            ConsoleUtility.PrintLine('=');
            Console.WriteLine("스파르타 던전 생성중...");
            Thread.Sleep(1000);
            Console.WriteLine("몬스터 생성중...");
            Thread.Sleep(1000);
            Console.WriteLine("장비 제작중...");
            Thread.Sleep(1000);
            ConsoleUtility.PrintLine('=');
            Console.Write("B1A4던전에 오신 것을 환영합니다 이름을 적어주세요 : ");
            string Name = Console.ReadLine();
            return Name;
        }
    }
}
