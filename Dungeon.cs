﻿using Sparta_week3;
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
        //private Monster _minion;
        //private Monster _cannonMinion;
        //private Monster _hollowworm;                
        private Player _player;
        private int _killCount;

        //public Dungeon()
        //{
                     
        //}


        public void EnterDungeon(Player player)
        {
            _player = player;
            MonsterAppear();

            // 구성
            // 0. 화면 정리
            Console.Clear();
            // 1. 전투 선택 멘트를 줌

            Console.WriteLine("던전 탐색중...");
            Thread.Sleep(1000);

            Console.WriteLine($"\n{_monsters.Length}마리의 몬스터가 등장 했다!\n");

            foreach (Monster monster in _monsters)
            {
                Console.WriteLine($"Lv.{monster.Level} {monster.Name} Hp {monster.Hp}/{monster.MaxHp}");
            }
            Console.WriteLine("\n");

            //플레이어 정보 불러오기
            PlayerStatus();

            Console.WriteLine(
                "1. 전투한다\n" +
                "0. 도망친다\n"
                );
            int choice = ConsoleUtility.PromptMenuChoice(0, 1);

            switch (choice)
            {
                case (int)DungeonChoice.Run:
                    _monsters = null;
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
                    Console.WriteLine($"[{i + 1}] Lv.{_monsters[i].Level} {_monsters[i].Name} 공격력 {_monsters[i].Atk} Dead");
                }
                else 
                {
                    Console.WriteLine($"[{i + 1}] Lv.{_monsters[i].Level} {_monsters[i].Name} 공격력 {_monsters[i].Atk} Hp {_monsters[i].Hp}/{_monsters[i].MaxHp}");
                }
            }
            Console.WriteLine("");
            
            PlayerStatus();

            Console.WriteLine(
                "번호를 입력해 공격할 몬스터를 선택해 주세요\n" +
                "0. 턴 종료");
            
            int choice = ConsoleUtility.PromptMenuChoice(0, _monsters.Length);

            //턴 종료
            if (choice == 0)
            {
                Console.Clear();
                Console.WriteLine($"{_player.Name} 은(는) 아무것도 하지 않고 턴을 종료 했다.\n");

                Thread.Sleep(1500);

                MonsterTurn();

            }
            else 
            {
                //사망한 몬스터 선택시
                if (!_monsters[choice - 1].IsLife)
                {
                    Console.WriteLine($"이미 죽은 몬스터입니다. 다시 선택 해주세요\n");
                    Thread.Sleep(1500);

                    Battle();
                    return;
                }


                Console.Clear();
                Console.WriteLine("Battle!\n");
                //플레이어의 공격
                //Console.WriteLine($"{_monsters[choice - 1].Name}을/를 공격했다!");
                Console.Write(
                    $"{_player.Name}의 공격!\n" +
                    $"Lv.{_monsters[choice - 1].Level} {_monsters[choice - 1].Name} 을(를) 맞췄습니다. [{_player.Atk}]\n\n");

                Thread.Sleep(1000);

                Console.Write($"Lv.{_monsters[choice - 1].Level} {_monsters[choice - 1].Name}" +
                    $"Hp {_monsters[choice - 1].Hp} -> ");

                _player.Attack(_monsters[choice - 1]);

                if (_monsters[choice - 1].Hp <= 0)
                {
                    Console.WriteLine("Dead");
                    
                    _killCount++;
                    _monsters[choice - 1].Dead();

                    if (_killCount == _monsters.Length)
                    {
                        Console.WriteLine("\n전투에서 승리했습니다!");
                        Console.WriteLine("0. 다음");
                        ConsoleUtility.PromptMenuChoice(0, 0);
                        BattleResult();
                        return;
                    }
                }
                else 
                {
                    Console.WriteLine($"{_monsters[choice - 1].Hp}");
                }

                Console.WriteLine("\n0. 다음");
                ConsoleUtility.PromptMenuChoice(0, 0);

                
                //몬스터 공격
                MonsterTurn();              
            }
        }


        public void MonsterAppear()
        {
            int num = new Random().Next(1, 5);
            _monsters = new Monster[num];
            _killCount = 0;
                       
            for (int i = 0; i < num; i++)
            {
                switch (new Random().Next(1,4))
                {
                    case (int)MonsterType.Minion:
                        Monster _minion = new Monster("미니언", 2, 15, 15);
                        _monsters[i] = _minion;
                        break;

                    case (int)MonsterType.CannonMinion:
                        Monster _cannonMinion = new Monster("대포미니언", 5, 18, 25);
                        _monsters[i] = _cannonMinion;
                        break;

                    case (int)MonsterType.Hollowworm:
                        Monster _hollowworm = new Monster("공허충", 3, 19, 10);
                        _monsters[i] = _hollowworm;
                        break;
                }
            }                      
        }

        public void MonsterTurn()
        {
            Console.Clear();
            Console.WriteLine("Battle!\n");
            //몬스터 공격
            for (int i = 0; i < _monsters.Length; i++)
            {
                if (!_monsters[i].IsLife) continue;

                if (_monsters[i].Atk < _player.Def)
                {
                    Console.WriteLine($"{_player.Name}의 방어력이 높아 {_monsters[i].Name}의 공격이 실패 합니다!");
                    Thread.Sleep(1000);
                }
                else
                {
                    Console.Write(
                    $"Lv.{_monsters[i].Level} {_monsters[i].Name} 의 공격!\n" +
                    $"{_player.Name} 을(를) 맞췄습니다.    [데미지 : ");
                    if (_player.Def > 0) Console.WriteLine($"{_monsters[i].Atk} - ({_player.Def})]\n");
                    else Console.WriteLine($"{_monsters[i].Atk}]\n");
                    Thread.Sleep(1000);
                }
                
                Console.Write(
                    $"[내정보]\n" +
                    $"Lv.{_player.Level} {_player.Name}\n" +
                    $"Hp {_player.Hp} -> ");

                _monsters[i].Attack(_player);

                Console.WriteLine($"{_player.Hp}\n");

                Thread.Sleep(1000);


                //플레이어 체력이 0이 되면 패매 메인메뉴로 돌아간다
                if (_player.Hp == 0)
                {
                    Console.WriteLine(
                        "플레이어의 체력이 0이되어 전투를 종료합니다.\n" +
                        "0. 다음");
                    int choice0 = ConsoleUtility.PromptMenuChoice(0, 0);
                    if (choice0 == 0)
                    {
                        BattleResult();
                        return;
                    }                    
                }
            }
            Console.WriteLine("0. 다음");
            int choice0_2 = ConsoleUtility.PromptMenuChoice(0, 0);
            if (choice0_2 == 0) Battle();
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
                Console.WriteLine("당신은 던전에서 패배했습니다.\n");
            }
            else 
            {
                Console.WriteLine("Victory\n");
                Console.WriteLine($"던전에서 몬스터 {_monsters.Length}마리를 잡았습니다.\n");
            }

            PlayerStatus();

            //플레이어 체력 회복 (임시)
            _player.Hp = _player.MaxHp;
            //MonsterRespawn();
            Console.WriteLine("0. 다음");
            ConsoleUtility.PromptMenuChoice(0, 0);           
        }


        public void PlayerStatus()
        {            
            Console.WriteLine(
                $"[내정보]\n" +
                $"Lv.{_player.Level}  {_player.Name} ({_player.Job})\n" +
                $"HP {_player.Hp}/{_player.MaxHp}  방어력 {_player.Def}" +
                $"\n");
        }

        //public void MonsterRespawn()
        //{
        //    foreach (Monster monster in _monsters)
        //    {
        //        monster.Respawn();
        //    }
        //}



    }
}
