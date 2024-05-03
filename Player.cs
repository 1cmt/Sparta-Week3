﻿using System;
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
        public int Atk;
        public int Def;
        public float Ctl = 0.15f;
        public int Mp;
        public int MaxHp;
        public int Hp;
        public int MaxMp;
        public int Gold;
        public int Cexp = 0;
        public List<int> KillCount = new List<int> { 0, 0, 0, 0 };
        public int Texp { get; private set; }
        public List<Skill> skillbook = new List<Skill>();
        public Item?[] EquipItems { get; set; } //배열의 index가 장착 부위를 의미 (ItemType을 int로 형변환 하여 접근)

        public Player(string name, string job, int level = 1, int atk = 10, int def = 10, int hp = 100, int gold = 1000, int exp = 10, int mp = 50, int maxmp = 50)
        {
            _name = name;
            _job = job;
            skillbook = Skill.Gainskill(job);
            Level = level;
            Gold = gold;
            (MaxHp, Hp) = (hp, hp);
            Texp = exp;
            Mp = mp; MaxMp = maxmp;
            if (job == "archer") Atk = (int)(atk * 1.2f);
            else Atk = atk;
            Def = job == "warrior" ? (int)(def * 1.2f) : def;
            (Hp, MaxHp) = job == "warrior" ? ((int)(hp * 1.2f), (int)(hp * 1.2f)) : (hp, hp);
            Ctl = job == "assassin" ? (int)(Ctl * 1.2f) : Ctl;
            (Mp, MaxMp) = job == "wizard" ? ((int)(Mp * 1.2f), (int)(Mp * 1.2f)) : (Mp, Mp);
            EquipItems = new Item?[10];
        }

        public void Levelup(ref int level, ref int cexp)
        {
            if (cexp >= Texp)
            {
                cexp -= Texp;
                Texp += 20 + level * 5;
                Atk += 1;
                Def++;
                level++;
                ;
            }
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

        public static string InputName()
        {
            Console.Write("B1A4던전에 오신 것을 환영합니다 이름을 적어주세요 : ");
            string? Name = Console.ReadLine();
            return Name;
        }

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
        public void Changejob(ref int gold, ref string job)
        {
            gold = 34000;
            if (gold >= 32747)
            {
                Console.Write("변경하실 직업을 입력해주세요: ");
                string? TempJob = InputJob();
                if (job == TempJob) { Console.WriteLine("똑같은 직업입니다."); }
                else
                {
                    job = TempJob;
                    gold -= 32747;
                    Console.WriteLine(TempJob + "직업으로 변경완료");
                }
            }
            else { Console.WriteLine($"골드가 {32747 - gold}만큼 부족합니다"); }

            ConsoleUtility.PrintLine('=');
            Console.WriteLine("아무 키나 눌러서 진행");
            Console.ReadKey();
        }

        public void StatusMenu(Inventory inventory)
        {
            Console.Clear();

            ConsoleUtility.PrintTitle(((Func<string, string>)(status => status.PadLeft(50)))("상태창".PadLeft(50)));
            ConsoleUtility.PrintTextHighlightsColor(ConsoleColor.Yellow, "레벨 : ", Level.ToString(), "\n");
            ConsoleUtility.PrintTextHighlightsColor(ConsoleColor.Yellow, "이름 : ", Name);
            ConsoleUtility.PrintTextHighlightsColor(ConsoleColor.Yellow, "(", Job, ")\n");
            ConsoleUtility.PrintTextHighlightsColor(ConsoleColor.Green, "공격력 : ", (Atk + inventory.BonusAtk).ToString().PadRight(6), inventory.BonusAtk > 0 ? $" (+{inventory.BonusAtk})\n" : "\n");
            ConsoleUtility.PrintTextHighlightsColor(ConsoleColor.Green, "방어력 : ", (Def + inventory.BonusDef).ToString().PadRight(6), inventory.BonusDef > 0 ? $" (+{inventory.BonusDef})\n" : "\n");
            ConsoleUtility.PrintTextHighlightsColor(ConsoleColor.Green, "체  력 : ", (Hp + inventory.BonusHp).ToString().PadRight(6), inventory.BonusHp > 0 ? $" (+{inventory.BonusHp})\n" : "\n");
            ConsoleUtility.PrintTextHighlightsColor(ConsoleColor.Yellow, "Gold : ", Gold.ToString(), "\n");

            for (int i = 0; i < skillbook.Count; i++)
            {
                skillbook[i].PrintSkillDescription(this);
            }
            ConsoleUtility.PrintTextHighlightsColor(ConsoleColor.Yellow, "", "0", ". 메뉴로 나가기\n");
            ConsoleUtility.PrintTextHighlightsColor(ConsoleColor.Yellow, "", "1", ". 직업변경 : 32767 G\n");
            ConsoleUtility.PrintLine('=');
            int choice = ConsoleUtility.PromptMenuChoice(0, 1);
            if (choice == 0) return;
            else
            {
                Changejob(ref Gold, ref _job);
                StatusMenu(inventory);
            }
        }

        public void Attack(Monster monster)
        {
            monster.Hp -= Atk;
            if (monster.Hp < 0) monster.Hp = 0;
        }
    }
}
