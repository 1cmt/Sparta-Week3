using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TextGame
{
    public class Player
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
        public int MaxHp;
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
            (MaxHp,Hp) = (hp,hp);
            Texp = exp;
            if (job == "archer") Atk = (int)(atk * 1.2f);
            else Atk = atk;
            Def = job == "warrior" ? (int)(def * 1.2f)  : def;
            (Hp,MaxHp)  = job == "warrior" ? ((int)(hp * 1.2f), (int)(hp * 1.2f)) : (hp,hp);
            Ctl = job == "assassin"? (int)(Ctl * 1.2f)  : Ctl;
            Mana= job == "wizard"  ? (int)(Mana * 1.2f) : Mana;
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
        public static string InputName()
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
            string? Name = Console.ReadLine();
#pragma warning disable CS8603 // 가능한 null 참조 반환입니다.
            return Name;
#pragma warning restore CS8603 // 가능한 null 참조 반환입니다.
        }

        internal static string InputJob()
        {
            Console.WriteLine("직업을 선택해주세요");
            ConsoleUtility.PrintLine('=');
            string @as = "Assassin"; string wa = "Warrior"; string wi = "Wizard"; string ar = "Archer";
            Console.Write("       "+@as.PadRight(27)+ wa.PadRight(27)+wi.PadRight(27)+ ar.PadRight(22)+"\n");
            Console.Write("상대의 급소를 노려         "+"강인한 육체의 소유자로     "+"강력한 스킬을 사용합니다   "+"강력한 팔힘으로            "+"\n");
            Console.Write("치명적인 일격을 가합니다   "+"생존능력이 높습니다        "+"                           "+"높은 데미지를 입힙니다     "+"\n");
            string? job = Console.ReadLine();
#pragma warning disable CS8602 // null 가능 참조에 대한 역참조입니다.
            return job.ToLower();
#pragma warning restore CS8602 // null 가능 참조에 대한 역참조입니다.
        }
    }
}
