using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TextGame
{
    public enum MonsterType
    {
        Minion,
        CannonMinion,
        Hollowworm
    }


    internal class Dungeon
    {
        private Monster[] monsters;
        private Monster minion;
        private Monster cannonMinion;
        private Monster hollowworm;

        


        public Dungeon()
        {
            minion = new Monster("미니언", 2, 5, 15);
            cannonMinion = new Monster("대포미니언", 5, 8, 25);
            hollowworm = new Monster("공허충", 3, 9, 10);
            MonsterAppear();
        }


        public void EnterDungeon()
        {
            // 구성
            // 0. 화면 정리
            Console.Clear();
            // 1. 전투 선택 멘트를 줌
            //Console.WriteLine("■■■■■■■■■■■■■■■■■■■■■■■■■■■■");
            Console.WriteLine("Battle!\n");
            Console.WriteLine($"{monsters.Length}마리의 몬스터가 등장 했다!\n");

            foreach (Monster monster in monsters)
            {
                Console.WriteLine($"Lv.{monster.Level} {monster.Name} Hp {monster.Hp}");
            }
            Console.WriteLine("\n\n");
            //플레이어 정보 불러오기

            Console.WriteLine(
                "1. 공격\n" +
                "0. 도망친다\n"
                );
            int choice = ConsoleUtility.PromptMenuChoice(0, 1);
            //Console.WriteLine("■■■■■■■■■■■■■■■■■■■■■■■■■■■■");





        }

        public void MonsterAppear()
        {
            int num = new Random().Next(1, 5);
            monsters = new Monster[num];

            for (int i = 0; i < num; i++)
            {
                switch (new Random().Next(0,3))
                {
                    case (int)MonsterType.Minion:
                        monsters[i] = minion;
                        break;

                    case (int)MonsterType.CannonMinion:
                        monsters[i] = cannonMinion;
                        break;

                    case (int)MonsterType.Hollowworm:
                        monsters[i] = hollowworm;
                        break;

                }
            }                      
        }



    }
}
