using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
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
        private Player _player;
        private QuestManager _questManager;
        private Skill _skill;

        private int _killCount;
        private int _rewardGold;
        private int _rewardExp;

        private string _minionStr;
        private string _hollowwormStr;


        public Dungeon()
        {
            _minionStr = ConsoleUtility.PadRightForMixedText("미니언", ConsoleUtility.GetPrintableLength("대포미니언"));
            _hollowwormStr = ConsoleUtility.PadRightForMixedText("공허충", ConsoleUtility.GetPrintableLength("대포미니언"));
        }


        public void EnterDungeon(Player player, QuestManager questManager)
        {
            _player = player;
            _questManager = questManager;
            MonsterAppear();

            // 구성
            // 0. 화면 정리
            Console.Clear();
            // 1. 전투 선택 멘트를 줌

            Console.WriteLine("던전 탐색중...");
            Thread.Sleep(1700);

            Console.Clear();
            Console.WriteLine($"{_monsters.Length}마리의 몬스터가 등장 했다!\n");

            Console.WriteLine("[몬스터정보]");
            foreach (Monster monster in _monsters)
            {
                Console.WriteLine($"Lv.{monster.Level} {monster.Name} Hp {monster.Hp}/{monster.MaxHp}");
            }
            Console.WriteLine("");

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
                    _rewardGold = 0;
                    _rewardExp = 0;
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

            Console.WriteLine("[몬스터정보]");
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
                "0. 턴 종료\n");
            
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

                //스킬사용, 일반 공격 선택
                HowAttack(choice - 1);                      
            }
        }


        private void MonsterAppear()
        {
            int num = new Random().Next(1, 5);
            _monsters = new Monster[num];
            _killCount = 0;            
            _rewardExp = 0;
            _rewardGold = 0;
            
            for (int i = 0; i < num; i++)
            {
                switch (new Random().Next(1,4))
                {
                    case (int)MonsterType.Minion:
                        Monster _minion = new Monster(_minionStr, 2, 15, 10, 300, 2);
                        _monsters[i] = _minion;
                        _rewardGold += _minion.DropGold;
                        _rewardExp += _minion.DropExp;
                        break;

                    case (int)MonsterType.CannonMinion:
                        Monster _cannonMinion = new Monster("대포미니언", 5, 18, 25, 1000, 5);
                        _monsters[i] = _cannonMinion;
                        _rewardGold += _cannonMinion.DropGold;
                        _rewardExp += _cannonMinion.DropExp;
                        break;

                    case (int)MonsterType.Hollowworm:
                        Monster _hollowworm = new Monster(_hollowwormStr, 3, 19, 15, 600, 3);
                        _monsters[i] = _hollowworm;
                        _rewardGold += _hollowworm.DropGold;
                        _rewardExp += _hollowworm.DropExp;
                        break;
                }
            }                      
        }

        private void MonsterTurn()
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
                    //Thread.Sleep(1000);
                }
                else
                {
                    Console.Write(
                    $"Lv.{_monsters[i].Level} {_monsters[i].Name} 의 공격!\n" +
                    $"{_player.Name} 을(를) 맞췄습니다.    [데미지 : ");
                    if (_player.Def > 0) Console.WriteLine($"{_monsters[i].Atk} - ({_player.Def})]\n");
                    else Console.WriteLine($"{_monsters[i].Atk}]\n");
                    //Thread.Sleep(1000);
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
                    Console.WriteLine("플레이어의 체력이 0이되어 전투를 종료합니다.\n");
                    Console.WriteLine("\n아무키나 입력해서 넘어가 주세요.");
                    Console.ReadKey();

                    BattleResult();
                    return;
                }
            }

            Console.WriteLine("\n아무키나 입력해서 넘어가 주세요.");
            Console.ReadKey();
            Battle();

            //Console.WriteLine("0. 다음");
            //int choice0_2 = ConsoleUtility.PromptMenuChoice(0, 0);
            //if (choice0_2 == 0) Battle();
        }

        private void BattleResult()
        {
            Console.Clear();
            // 1. 전투 선택 멘트를 줌            
            Console.WriteLine("Battle! - Result\n");

            //패배
            if (_player.Hp == 0)
            {
                Console.WriteLine("You Lose\n");
                Console.WriteLine("" +
                    "당신은 던전에서 패배했습니다." +
                    "메인화면으로 돌아갑니다...\n");
                _player.Hp = 1;

            }
            else 
            {                               
                Console.WriteLine("Victory\n");
                Console.WriteLine(
                    $"던전에서 몬스터 {_monsters.Length}마리를 잡았습니다!\n\n" +
                    $"[보상]\n" +
                    $"Gold {_rewardGold}를 획득했습니다!\n" +
                    $"경험치 {_rewardExp}를 획득했습니다!\n");

                _player.Gold += _rewardGold;
                _player.Cexp += _rewardExp;

                if (_player.Cexp >= _player.Texp)
                {
                    _player.Levelup(ref _player.Level, ref _player.Cexp);
                    Console.WriteLine($"{_player.Name}의 Lv이 올랐습니다!\n");
                }
            }

            PlayerStatus();
            //MonsterRespawn();
            Console.WriteLine("\n아무키나 입력해서 넘어가 주세요.");
            Console.ReadKey();
        }


        private void PlayerStatus()
        {            
            Console.WriteLine(
                $"[내정보]\n" +
                $"Lv.{_player.Level}  {_player.Name} ({_player.Job})\n" +
                $"HP {_player.Hp}/{_player.MaxHp}  Mp {_player.Mp}/{_player.MaxMp}  방어력 {_player.Def}\n" +
                $"EXP {_player.Cexp}/{_player.Texp}  Gold {_player.Gold}\n");
        }

        private void HowAttack(int monsterIndex)
        {
            // 구성
            // 0. 화면 정리
            Console.Clear();
            // 1. 전투 선택 멘트를 줌            
            Console.WriteLine("Battle!\n");

            Console.WriteLine("[몬스터정보]");
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
                "공격 방식을 선택해 주세요\n" +
                "1. 기본 공격\n" +
                "2. 스킬 사용\n");

            int choice = ConsoleUtility.PromptMenuChoice(1, 2);

            if (choice == 1)
            {
                //기본 공격

                Console.Clear();
                Console.WriteLine("Battle!\n");

                //플레이어의 공격
                //Console.WriteLine($"{_monsters[choice - 1].Name}을/를 공격했다!");
                Console.Write(
                    $"{_player.Name}의 공격!\n" +
                    $"Lv.{_monsters[monsterIndex].Level} {_monsters[monsterIndex].Name} 을(를) 맞췄습니다.    [데미지 : {_player.totalAtk}]\n\n");

                Thread.Sleep(1000);

                Console.Write($"Lv.{_monsters[monsterIndex].Level} {_monsters[monsterIndex].Name}" +
                    $"Hp {_monsters[choice - 1].Hp} -> ");

                _player.Attack(_monsters[monsterIndex]);

                if (_monsters[monsterIndex].Hp <= 0)
                {
                    Console.WriteLine("Dead");

                    _killCount++;
                    _monsters[monsterIndex].Dead();

                    //퀘스트 수락시 몬스터 카운트 증가
                    if (_questManager.questList[0].ProgressStatus == 1 && _monsters[monsterIndex].Name == _minionStr && _player.KillCount[(int)MonsterType.Minion] < 6)
                    {
                        _player.KillCount[(int)MonsterType.Minion] += 1;
                        Console.WriteLine(
                        $"\n[퀘스트 진행도]\n" +
                        $"미니언 처치!    [{_player.KillCount[(int)MonsterType.Minion]}/5]\n");

                        if (_player.KillCount[(int)MonsterType.Minion] == 5) Console.WriteLine("퀘스트를 완료 했습니다!\n");

                    }

                    if (_killCount == _monsters.Length)
                    {
                        Console.WriteLine("\n전투에서 승리했습니다!\n");
                        Console.WriteLine("\n아무키나 입력해서 넘어가 주세요.");
                        Console.ReadKey();
                        BattleResult();
                        return;
                    }
                }
                else
                {
                    Console.WriteLine($"{_monsters[monsterIndex].Hp}");
                }

                Console.WriteLine("\n아무키나 입력해서 넘어가 주세요.");
                Console.ReadKey();

                //몬스터 공격
                MonsterTurn();

            }
            else if (choice == 2)
            {
                //스킬 선택지

                // 구성
                // 0. 화면 정리
                Console.Clear();
                // 1. 전투 선택 멘트를 줌            
                Console.WriteLine("Battle!\n");

                Console.WriteLine("[몬스터정보]");
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

                //스킬 출력
                Console.WriteLine("[스킬정보]");
                for (int i = 0; i < _player.skillbook.Length; i++)
                {
                    Console.Write($"{i+1}");
                    _player.skillbook[i].PrintSkillDescription(_player);                
                }
                Console.WriteLine("0. 뒤로가기\n");

                int choiceSkill = ConsoleUtility.PromptMenuChoice(0, _player.skillbook.Length);

                if (choiceSkill == 0)
                {
                    HowAttack(monsterIndex);
                    return;
                }
                else 
                {
                    //마나 부족
                    if (_player.Mp < _player.skillbook[choiceSkill - 1].Mplose)
                    {
                        Console.Clear();
                        Console.WriteLine("Mp가 부족합니다. 다른 공격방식을 선택해 주세요.");
                        Thread.Sleep(1700);

                        HowAttack(monsterIndex);
                    }

                    //스킬 사용

                    Console.Clear();
                    Console.WriteLine("Battle!\n");

                    //플레이어의 공격
                    //Console.WriteLine($"{_monsters[choice - 1].Name}을/를 공격했다!");
                    Console.Write(
                        $"{_player.Name}은(는) {_player.skillbook[choiceSkill - 1].Skillname}을(를) 사용했다!\n" +
                        $"Lv.{_monsters[monsterIndex].Level} {_monsters[monsterIndex].Name}을(를) 맞췄습니다.    [데미지 : {_player.skillbook[choiceSkill - 1].SkillUse(choiceSkill, _player)}]\n\n");

                    Thread.Sleep(1000);
                    
                    Console.Write($"Lv.{_monsters[monsterIndex].Level} {_monsters[monsterIndex].Name}" +
                    $"Hp {_monsters[monsterIndex].Hp} -> ");

                    _monsters[monsterIndex].Hp -= _player.skillbook[choiceSkill - 1].SkillUse(choiceSkill, _player);
                    _player.Mp -= _player.skillbook[choiceSkill - 1].Mplose;
                   
                    if (_monsters[monsterIndex].Hp <= 0)
                    {
                        Console.WriteLine("Dead");

                        _monsters[monsterIndex].Hp = 0;
                        _killCount++;
                        _monsters[monsterIndex].Dead();

                        //퀘스트 수락시 몬스터 카운트 증가
                        if (_questManager.questList[0].ProgressStatus == 1 && _monsters[monsterIndex].Name == _minionStr && _player.KillCount[(int)MonsterType.Minion] < 6)
                        {
                            _player.KillCount[(int)MonsterType.Minion] += 1;
                            Console.WriteLine(
                            $"\n[퀘스트 진행도]\n" +
                            $"미니언 처치!    [{_player.KillCount[(int)MonsterType.Minion]}/5]\n");

                            if (_player.KillCount[(int)MonsterType.Minion] == 5) Console.WriteLine("퀘스트를 완료 했습니다!\n");

                        }

                        if (_killCount == _monsters.Length)
                        {
                            Console.WriteLine("\n전투에서 승리했습니다!\n");
                            Console.WriteLine("\n아무키나 입력해서 넘어가 주세요.");
                            Console.ReadKey();

                            BattleResult();
                            return;
                        }
                    }
                    else
                    {
                        Console.WriteLine($"{_monsters[monsterIndex].Hp}");
                    }

                    Console.WriteLine("\n아무키나 입력해서 넘어가 주세요.");
                    Console.ReadKey();

                    //몬스터 공격
                    MonsterTurn();

                }              
            }
        }
    }
}
