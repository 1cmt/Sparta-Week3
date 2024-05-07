using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Reflection.Emit;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Linq;
using Newtonsoft.Json;

namespace TextGame
{
    [Serializable]
    public class Player
    {
        [JsonProperty("_name")]
        private string _name;

        [JsonProperty("_job")]
        private string _job;
        public string Name { get { return _name; } }
        public string Job
        { 
            get { return _job; }
            set
            {
                UncheckJob();
                CheckJob(value);
                _job = value;
            }
        }
        public int Level;
        public float Ctl = 0.15f;
        public int Atk, totalAtk, levelAtk=0;
        public int Def, totalDef, levelDef=0;
        public int Mp,totalMp;
        public int MaxMp, totalMaxMp;
        public int Hp, totalHp;
        public int MaxHp,totalMaxHp;
        public int Gold;
        public int Cexp = 0;

        public List<int> KillCount = new List<int> { 0, 0, 0, 0 };
        public int Texp { get; set; }
        public Skill[] skillbook = new Skill[2];
        public Item?[] EquipItems { get; set; } //배열의 index가 장착 부위를 의미 (ItemType을 int로 형변환 하여 접근)

        public Player(string name, string job, int level = 1, int atk = 10, int def = 10, int gold = 1700,  int exp = 10, int hp = 100, int mp = 50)
        {
            _name = name;
            _job = job;
            skillbook = Skill.Gainskill(job);
            Level = level;
            Gold = gold;
            Texp = exp;
            Hp    = job == "WARRIOR"  ?hp = 120 :hp;
            MaxHp = job == "WARRIOR"  ?hp = 120:hp;
            Mp    = job == "WIZARD"   ?mp = 60:mp; 
            MaxMp = job == "WIZARD"   ?mp=  60:mp;
            Atk   = job == "ARCHER"   ?atk = 12 : atk;
            Def   = job == "WARRIOR"  ?def = 12:def;
            Ctl   = job == "ASSASSIN" ?Ctl = 0.3f : Ctl;
            EquipItems = new Item?[10];
        }

        public void Levelup(ref int level, ref int cexp)
        {
            if (cexp >= Texp)
            {
                cexp -= Texp;
                Texp += 20 + level * 5;
                levelAtk += 1;
                levelDef++;
                level++;
                ;
            }
        }
        public static string InputName()
        {
            Console.Write("B1A4던전에 오신 것을 환영합니다 이름을 적어주세요 : ");
            string Name = Console.ReadLine();
            
            if (string.IsNullOrEmpty(Name)) // 빈 문자열인지 확인
            {
                Console.WriteLine("이름을 입력해주세요.");
                return InputName(); //
            }
            return Name.Trim();
        }
            public void Attack(Monster monster)
        {
            monster.Hp -= totalAtk;
            if (monster.Hp < 0) monster.Hp = 0;
        }
        //summaery//
        /*<스탯관련입니다>*/
        //summaery//
        public static int Multiple(float multimpler, int multiplecand)
        {
            return (int)(multimpler * multiplecand);
        }
        public void CheckStat(Inventory inventory)
        {
            totalAtk = Atk +levelAtk+ inventory.BonusAtk;
            totalDef = Def +levelDef+ inventory.BonusDef;
            totalHp = Hp + inventory.BonusHp;
            totalMaxHp = MaxHp + inventory.BonusHp;
        }
        public void UncheckJob()
        {
            switch (_job)
            {
                case "ARCHER":
                    Atk = Multiple(0.83334f, Atk);
                    break;
                case "WARRIOR":
               
                    Def = Multiple(0.83334f, Def);
                    Hp = Multiple(0.83334f, Hp);
                    MaxHp = Multiple(0.83334f, MaxHp);
                    break;
              
                case "WIZARD":
                    MaxMp = Multiple(0.83334f, MaxMp);
                    Mp = Multiple(0.83334f, Mp);
                    break;
                case "ASSASSIN":
                    Ctl = Multiple(0.5f, (int)Ctl);
                    break;
            }
        }

        public void CheckJob(string name) 
        { 
            switch (name)
            {
                case "ARCHER":
                    Atk = Multiple(1.2f, Atk );
                    break;
                case "WARRIOR":

                    Def = Multiple(1.2f, Def );
                    Hp = Multiple(1.2f, Hp);
                    MaxHp = Multiple(1.2f, MaxHp);
                    break;

                case "WIZARD":
                    MaxMp = Multiple(0.8f, MaxMp);
                    Mp = Multiple(1.2f, Mp);
                    break;
                case "ASSASSIN":
                    Ctl = Multiple(2f, (int)Ctl);
                    break;
            }
        }
        //summaery//
        /*</스탯관련입니다>*/
        //summaery//


        //summaery//
        /* <직업관련 메소드입니다>*/
        //summary//
        public static string InputJob()
        {
            Console.WriteLine("직업을 선택해주세요");

            ConsoleUtility.PrintLine('=');
            string @as = "ASSASSIN"; string wa = "WARRIOR"; string wi = "WIZARD"; string ar = "ARCHER";
            Console.Write("       " + @as.PadRight(27) + wa.PadRight(27) + wi.PadRight(27) + ar.PadRight(22) + "\n");
            Console.Write("상대의 급소를 노려         " + "강인한 육체의 소유자로     " + "강력한 스킬을 사용합니다   " + "강력한 팔힘으로            " + "\n");
            Console.Write("치명적인 일격을 가합니다   " + "생존능력이 높습니다        " + "                           " + "높은 데미지를 입힙니다     " + "\n");
            ConsoleUtility.PrintLine('=');

            string Tempjob = CheckJob();
            return Tempjob;
        }
        public static string CheckJob()
        {
            string @as = "ASSASSIN"; string wa = "WARRIOR"; string wi = "WIZARD"; string ar = "ARCHER";
            string? job = Console.ReadLine().ToUpper();
            while (job != @as & job != wa & job != wi & job != ar)
            {
                Console.WriteLine("올바른 직업을 선택해주세요");
                job = Console.ReadLine().ToUpper();
            }
            return job;
        }

        public void Changejob(ref int gold, string job)
        {
            if (gold >= 1000)
            {
                Console.Write("변경하실 직업을 입력해주세요: ");
                string? TempJob = InputJob();
                if (job == TempJob) { Console.WriteLine("똑같은 직업입니다."); }
                else
                {
                    Job = TempJob;
                    skillbook = Skill.Gainskill(Job);
                    gold -= 1000;
                    Console.WriteLine(TempJob + "직업으로 변경완료");
                }
            }
            else { Console.WriteLine($"골드가 {1000 - gold}만큼 부족합니다"); }

            ConsoleUtility.PrintLine('=');
            Console.WriteLine("아무 키나 눌러서 진행");
            Console.ReadKey();
        }

        /// <summary>
        /* </직업관련 메소드입니다>*/
        /// </summary>
        public void StatusMenu(Inventory inventory)
        {
            
            Console.Clear();

            ConsoleUtility.PrintTitle(((Func<string, string>)(status => status.PadLeft(50)))("상태창".PadLeft(50)));
            ConsoleUtility.PrintTextHighlightsColor(ConsoleColor.Yellow, "레벨 : ", Level.ToString(), "(");
            ConsoleUtility.PrintTextHighlightsColor(ConsoleColor.Blue,Cexp.ToString()+"/", Texp.ToString(), ")\n");
            ConsoleUtility.PrintTextHighlightsColor(ConsoleColor.Yellow, "이름 : ", Name);
            ConsoleUtility.PrintTextHighlightsColor(ConsoleColor.Green, "(", Job, ")\n");
            ConsoleUtility.PrintTextHighlightsColor(ConsoleColor.Green, "공격력 : ", (totalAtk).ToString().PadRight(6), inventory.BonusAtk > 0 ? $" (+{inventory.BonusAtk})\n" : "\n");
            ConsoleUtility.PrintTextHighlightsColor(ConsoleColor.Green, "방어력 : ", (totalDef).ToString().PadRight(6), inventory.BonusDef > 0 ? $" (+{inventory.BonusDef})\n" : "\n");
            ConsoleUtility.PrintTextHighlightsColor(ConsoleColor.Green, "체  력 : ", (totalHp).ToString().PadRight(6), inventory.BonusHp > 0 ? $" (+{inventory.BonusHp})\n" : "\n");
            ConsoleUtility.PrintTextHighlightsColor(ConsoleColor.Green, "마  나 : ", (Mp).ToString().PadRight(6), "\n");
            ConsoleUtility.PrintTextHighlightsColor(ConsoleColor.Yellow, "Gold : ", Gold.ToString(), "\n");

            for (int i = 0; i < skillbook.Length; i++)
            {
                skillbook[i].PrintSkillDescription(this);
            }
            ConsoleUtility.PrintTextHighlightsColor(ConsoleColor.Yellow, "", "0", ". 메뉴로 나가기\n");
            ConsoleUtility.PrintTextHighlightsColor(ConsoleColor.Yellow, "", "1", ". 직업변경 : 1000 G\n");
            ConsoleUtility.PrintLine('=');

            int choice = ConsoleUtility.PromptMenuChoice(0, 1);
            if (choice == 0) return;
            else
            {
                Changejob(ref Gold,Job);
                CheckStat(inventory);
                StatusMenu(inventory);
            }
        }


        
    }
}
