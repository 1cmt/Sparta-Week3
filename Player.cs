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
        public string Job { get { return _job; } set => _job = value; }
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
        public void Changejob(ref int gold, string job)
        {
            if (gold >= 32747)
            { 
                gold -= 32747;
                Console.Write("변경하실 직업을 입력해주세요: ");
                string TempJob = Console.ReadLine();
                if(job == TempJob) { Console.Write("똑같은 직업입니다."); }
                else { 
                        job = TempJob.ToUpper();
                         
                     }
                Console.Read();
            }
            else
            {
                Console.WriteLine($"골드가 {32747-gold}만큼 부족합니다");
                Console.ReadKey();
            }
        }
        public static string InputName()
        {
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
            string @as = "ASSASSIN"; string wa = "WARRIOR"; string wi = "WIZARD"; string ar = "ARCHER";
            Console.Write("       "+@as.PadRight(27)+ wa.PadRight(27)+wi.PadRight(27)+ ar.PadRight(22)+"\n");
            Console.Write("상대의 급소를 노려         "+"강인한 육체의 소유자로     "+"강력한 스킬을 사용합니다   "+"강력한 팔힘으로            "+"\n");
            Console.Write("치명적인 일격을 가합니다   "+"생존능력이 높습니다        "+"                           "+"높은 데미지를 입힙니다     "+"\n");
            ConsoleUtility.PrintLine('=');

            string? job = Console.ReadLine();
            job = job.ToUpper();
            while(job != @as& job!= wa& job != wi & job != ar) 
            {
                Console.WriteLine("올바른 직업을 선택해주세요");
                job = Console.ReadLine();
                job = job.ToUpper();
            }
            return job;

        }
        internal void StatusMenu(Player player,Inventory inventory)
        {
            Console.Clear();
            ConsoleUtility.PrintTitle(((Func<string, string>)(status => status.PadLeft(50)))("상태창"));
            ConsoleUtility.PrintTextHighlightsColor(ConsoleColor.Yellow, "레벨 : ", player.Level.ToString(),"\n");
            ConsoleUtility.PrintTextHighlightsColor(ConsoleColor.Yellow, "이름 : ", player.Name);
            ConsoleUtility.PrintTextHighlightsColor(ConsoleColor.Yellow, "(", player.Job,")\n");
            ConsoleUtility.PrintTextHighlightsColor(ConsoleColor.Yellow, "공격력 : ",player.Atk.ToString(),"\n");
            ConsoleUtility.PrintTextHighlightsColor(ConsoleColor.Yellow, "방어력 : ", player.Def.ToString(),"\n");
            ConsoleUtility.PrintTextHighlightsColor(ConsoleColor.Yellow, "체력 : ", player.Hp.ToString(),"\n");
            ConsoleUtility.PrintTextHighlightsColor(ConsoleColor.Yellow, "Gold : ", player.Gold.ToString(),"\n");

            ConsoleUtility.PrintTextHighlightsColor(ConsoleColor.Yellow, "", "0", ". 메뉴로 나가기\n");
            ConsoleUtility.PrintTextHighlightsColor(ConsoleColor.Yellow, "", "1", ". 직업변경 : 32767 G");

            int choice = ConsoleUtility.PromptMenuChoice(0, 1);
            if (choice == 0) return;
            else
            {
                Changejob(ref player.Gold, player.Job);
                StatusMenu(player,inventory);
            }
           

            
           



        }
    }
}
