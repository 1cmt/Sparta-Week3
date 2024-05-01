using Sparta_week3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace TextGame
{
    public enum MonsterType
    {
        Minion = 1,
        CannonMinion,
        Hollowworm
    }

    public enum DungeonChoice
    {
        Run,
        Battle
    }


    public class Dungeon
    {
        private Monster[] _monsters;
        private Monster _minion;
        private Monster _cannonMinion;
        private Monster _hollowworm;                
        private Player _player;

        public Dungeon()
        {
            _minion = new Monster("미니언", 2, 5, 15);
            _cannonMinion = new Monster("대포미니언", 5, 8, 25);
            _hollowworm = new Monster("공허충", 3, 9, 10);
            MonsterAppear();
            _player = GameManager.P1;
        }


        public void EnterDungeon()
        {
            // 구성
            // 0. 화면 정리
            Console.Clear();
            // 1. 전투 선택 멘트를 줌                        
            Console.WriteLine($"{_monsters.Length}마리의 몬스터가 등장 했다!\n");

            foreach (Monster monster in _monsters)
            {
                Console.WriteLine($"Lv.{monster.Level} {monster.Name} Hp {monster.Hp}");
            }
            Console.WriteLine("\n");

            //플레이어 정보 불러오기
            Console.WriteLine(
                $"Lv.{_player.Level}  {_player.Name} ({_player.Job})\n" +
                $"HP {_player.Hp}/{_player.MaxHp}" +
                $"\n");
            
            Console.WriteLine(
                "1. 전투한다\n" +
                "0. 도망친다\n"
                );
            int choice = ConsoleUtility.PromptMenuChoice(0, 1);

            switch (choice)
            {
                case (int)DungeonChoice.Run:
                    return;//메인 선택지로                    

                case (int)DungeonChoice.Battle:
                    Battle();
                    break;            
            }
            
        }

        private void Battle()
        {
            // 구성
            // 0. 화면 정리
            Console.Clear();
            // 1. 전투 선택 멘트를 줌            
            Console.WriteLine("Battle!\n");            

            for (int i = 0; i < _monsters.Length; i++)
            {
                if (_monsters[i].Hp <= 0)
                {
                    Console.WriteLine($"[{i + 1}] Lv.{_monsters[i].Level} {_monsters[i].Name} Dead");
                }
                Console.WriteLine($"[{i + 1}] Lv.{_monsters[i].Level} {_monsters[i].Name} Hp {_monsters[i].Hp}");
            }
            Console.WriteLine("");

            //플레이어 정보 불러오기
            Console.WriteLine(
                $"Lv.{_player.Level}  {_player.Name} ({_player.Job})\n" +
                $"HP {_player.Hp}/{_player.MaxHp}" +
                $"\n");

            Console.WriteLine(
                "번호를 입력해 공격할 몬스터를 선택해 주세요\n" +
                "0. 턴 종료");
            
            int choice = ConsoleUtility.PromptMenuChoice(0, _monsters.Length);

            //턴 종료
            if (choice == 0)
            {
                Console.Clear();
                Console.WriteLine($"{_player.Name} 은(는) 아무것도 하지 않고 턴을 종료 했다.\n");

                MonsterTurn();

            }
            else 
            {
                Console.Clear();
                //플레이어의 공격
                Console.WriteLine($"{_monsters[choice - 1].Name}을/를 공격했다!");




                //몬스터 공격
                MonsterTurn();

            }
        }


        public void MonsterAppear()
        {
            int num = new Random().Next(1, 5);
            _monsters = new Monster[num];

            for (int i = 0; i < num; i++)
            {
                switch (new Random().Next(1,4))
                {
                    case (int)MonsterType.Minion:
                        _monsters[i] = _minion;
                        break;

                    case (int)MonsterType.CannonMinion:
                        _monsters[i] = _cannonMinion;
                        break;

                    case (int)MonsterType.Hollowworm:
                        _monsters[i] = _hollowworm;
                        break;

                }
            }                      
        }

        public void MonsterTurn()
        {
            //몬스터 공격
            for (int i = 0; i < _monsters.Length; i++)
            {
                Console.WriteLine(
                    $"Lv.{_monsters[i].Level} {_monsters[i].Name} 의 공격!\n" +
                    $"{_player.Name} 을(를) 맞췄습니다.    [데미지 : {_monsters[i].Atk}]\n");

                Console.Write(
                    $"Lv.{_player.Level} {_player.Name}\n" +
                    $"Hp {_player.Hp} -> ");

                _monsters[i].Attack(_player);

                Console.WriteLine($"{_player.Hp}\n");

                //플레이어 체력이 0이 되면 패매 메인메뉴로 돌아간다
                if (_player.Hp == 0)
                {
                    Console.WriteLine(
                        "플레이어의 체력이 0이되어 전투를 종료합니다.\n" +
                        "0. 다음");
                    NextButton();

                    BattleResult();
                    return;
                }

            }
            Console.WriteLine("0. 다음");
            NextButton();
        }

        public void BattleResult()
        {
            Console.Clear();
            // 1. 전투 선택 멘트를 줌            
            Console.WriteLine("Battle! - Result\n");

            //패배
            if (_player.Hp == 0)
            {
                Console.WriteLine("You Lose\n");
                Console.WriteLine("당신은 던전에서 패배했습니다.");
            }
            else 
            {
                Console.WriteLine("Victory\n");
                Console.WriteLine($"던전에서 몬스터 {_monsters.Length}마리를 잡았습니다.");
            }



            //플레이어 정보 불러오기
            Console.WriteLine(
                $"Lv.{_player.Level}  {_player.Name} ({_player.Job})\n" +
                $"HP {_player.Hp}/{_player.MaxHp}" +
                $"\n");

            Console.WriteLine("0. 다음");
            NextButton();
        }





        public void NextButton()
        {
            int choice0 = ConsoleUtility.PromptMenuChoice(0, 0);
            if (choice0 == 0) Battle();
        }



    }
}
