using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.IO;
using System.Text.Json;
using Newtonsoft.Json;
using TextGame;
using System.ComponentModel;
using System.ComponentModel.Design;



namespace TextGame
{
    //1번째 퀘스트 : 5마리 잡기 + 5마리 중 몇 마리 잡았는지 체크
    //2번째 퀘스트 : 장비를 장착했는지 체크
    //3번째 퀘스트 : 강해졌는지 체크 (지금은 임의로 3레벨 달성했는지로 할게요)

    //퀘스트 진행상태 저장기능 (1.진행 가능 2.진행 중{+진행 중인 세부 내용} 3.완료)
    //퀘스트 저장 목록에 몬스터 킬카운트 필요합니다 <<<<<<퀘스트 조건체크용이라서 진행상태에 몇마리 잡았는지 기록 가능하면 불필요해요

    //public static class QuestService
    //{
    //    // 퀘스트 리스트를 JSON 파일로 저장하는 메서드
    //    public static void SaveQuestsToFile(List<Quest> quests, string filePath)
    //    {
    //        string json = JsonConvert.SerializeObject(quests, Formatting.Indented);
    //        File.WriteAllText(filePath, json);
    //        Console.WriteLine("Quests have been saved to file.");
    //    }

    //    // JSON 파일에서 퀘스트 리스트를 불러오는 메서드
    //    public static List<Quest> LoadQuestsFromFile(string filePath)
    //    {
    //        if (!File.Exists(filePath))
    //        {
    //            Console.WriteLine("File does not exist.");
    //            return new List<Quest>(); // 파일이 존재하지 않으면 빈 리스트 반환
    //        }

    //        string json = File.ReadAllText(filePath);
    //        List<Quest> quests = JsonConvert.DeserializeObject<List<Quest>>(json);
    //        Console.WriteLine("Quests have been loaded from file.");
    //        return quests;
    //    }
    //}


    //몬스터타입
    //public enum MonsterType
    //{
    //    Minion,         //0.미니언
    //    CannonMinion,   //1.대포 미니언
    //    VoidBug        //2.공허충
    //}
    //퀘스트 상태여부
    public enum QuestStatus
    {
        Startable,          //0.진행 가능
        InProgress,         //1.진행 중
        Finished            //2.완료
    }

    public class QuestManager
    {
        public List<Quest> questList;
        Player _player;
        Inventory _inventory;

        public QuestManager()       //퀘스트리스트 
        {
            questList = new List<Quest> //모든 퀘스트를 정의하는 함수
        {   
            //1.미니언 5마리 잡기
            new Quest(
                "마을을 위협하는 미니언 처치",
                "이봐! 마을 근처에 미니언들이 너무 많아졌다고 생각하지 않나?\n마을주민들의 안전을 위해서라도 저것들 수를 좀 줄여야 한다고!\n모험가인 자네가 좀 처치해주게!",
                "1.미니언 5마리 처치하기",
                "쓸만한 방패 x 1\n5G",
                player => //클리어 조건: 미니언 5마리 잡기
                {
                    if(player.KillCount[(int)MonsterType.Minion] >= 5)    //player.KillCount[MonsterType.Minion] >= 5 미니언을 5마리 이상 잡았으면
                    
                    {
                        Console.WriteLine("미니언 (5/5) (완료)");
                        return true;
                    }
                    else //다 못잡았으면
                    {
                        Console.WriteLine($"미니언 ({player.KillCount[(int)MonsterType.Minion]}/5) (진행중)");
                        return false;
                    }
                },
                player => //보상
                {
                    Console.WriteLine("쓸만한 방패 x 1");
                    Console.WriteLine("5골드");
                    _inventory.AddItem(new Item("쓸만한 방패", "쓸만한 방패입니다.", ItemType.Shield, 100, 0, 2, 0));
                    player.Gold += 5;
                }),

            //2.장비 장착하기
            new Quest(
                "장비를 장착해보자",
                "모험을 떠나기 전에 적절한 장비를 갖추는 건 중요하네.\n자네의 인벤토리에서 방패를 장착해보게!",
                "1.아무 방패 장착하기",
                "100G",
                player => //클리어 조건: 방패 장착하기
                {   
                    //쓸만한 방패를 장착했으면 shieldEquipped=true
                    Item shield = _inventory.ItemList.FirstOrDefault(item => item.Type == ItemType.Shield && item.IsEquipped);
                    bool shieldEquipped = (shield != null) ? shield.IsEquipped : false;

                    if(shieldEquipped)    //방패를 장착했으면
                    {
                        Console.WriteLine("방패 장착 (완료)");
                        return true;
                    }
                    else //방패를 장착 안했으면
                    {
                        Console.WriteLine("방패 장착 (진행중)");
                        return false;
                    }
                },
                player => //보상
                {
                    //쓸만한 방패 x 1
                    _inventory.AddItem(new Item("쓸만한 방패", "쓸만한 방패입니다", ItemType.Shield, 0, 2, 0, 500));
                    //100골드 지급
                    player.Gold += 100;
                }),
            //3.강해지기
            new Quest(
                "강해지기",
                "강한 모험가들도 처음에는 볼품없는 풋내기였지!\n당장 위험한 던전부터 들어갈 생각하지 말고 실력을 키워야만 하네.\n자네의 성장을 보여주게!",
                "1.레벨 3 달성",
                "200G",
                player => //클리어 조건: 미니언 5마리 잡기
                {
                    if(player.Level >= 3)    //레벨이 3 이상이면
                    {
                        Console.WriteLine("3레벨 달성 (완료)");
                        return true;
                    }
                    else //레벨이 3 미만이면
                    {
                        Console.WriteLine("3레벨 달성 (진행중)");
                        return false;
                    }
                },
                player => //보상
                {
                    //200골드 지급
                    player.Gold += 200;
                }),
            };
        }

        public void QuestMenu(Player player, Inventory inventory)     //마을 -> 퀘스트 메뉴
        {
            _player = player;
            _inventory = inventory;

            while (true)
            {
                string ProgressStatusText; // 진행가능 or 진행중 (완료는 제외)

                Console.Clear();
                ConsoleUtility.PrintTitle("■ 퀘스트 메뉴 ■");
                Console.WriteLine("진행 가능한 퀘스트들을 볼 수 있습니다.");
                Console.WriteLine("");
                Console.WriteLine("[퀘스트 목록]");

                //questList 리스트 안에 담긴 (완료되지 않은)퀘스트 수만큼 리스트를 보여주고, 선택지 생성
                //퀘스트 제목 끝에 (진행가능 or 진행중) 설정

                List<int> selectableList = new List<int>();
                selectableList.Add(-1); //0번째 배열은 안 쓴다는 의미

                //완료되지 않은, 진행가능 or 진행중인 퀘스트들을 선별하기
                for (int i = 0; i < questList.Count; i++)
                {
                    if (questList[i].ProgressStatus == (int)QuestStatus.Finished) //만약 체크한 퀘스트가 완료됐다면 (완료됨)표시하기
                    {
                        ProgressStatusText = " (완료됨)";
                        selectableList.Add(i); //i 값을 selectableList 리스트에 추가하기
                        Console.Write($"{selectableList.Count - 1}. ");
                        Console.WriteLine(questList[i].Title + $"{ProgressStatusText}");
                    }
                    else
                    {
                        ProgressStatusText = (questList[i].ProgressStatus == (int)QuestStatus.Startable) ? " (진행가능)" : " (진행중)";
                        selectableList.Add(i); //i 값을 selectableList 리스트에 추가하기
                        Console.Write($"{selectableList.Count - 1}. ");
                        Console.WriteLine(questList[i].Title + $"{ProgressStatusText}");
                    }
                }

                ConsoleUtility.PrintTitle("");
                Console.WriteLine("0.나가기");

                int KeyInput = ConsoleUtility.PromptMenuChoice(0, selectableList.Count);

                switch (KeyInput)
                {
                    case 0:
                        //메인메뉴 이동
                        return;
                    default:
                        SelectQuest(_player, KeyInput-1);
                        break;
                }
            }
        }
        public void SelectQuest(Player player, int index)   //퀘스트 선택하기
        {
            Console.Clear();
            while (true)
            {   
                Quest quest = questList[index];

                string QuestTitle = quest.Title;
                ConsoleUtility.PrintTitle(QuestTitle);
                Console.WriteLine("");
                Console.WriteLine(quest.Description);
                Console.WriteLine("");
                Console.WriteLine(" - 완료 조건 -");
                Console.WriteLine($"{quest.ClearConditionText}");
                Console.WriteLine("");
                Console.WriteLine(" - 보상 -");
                Console.WriteLine($"{quest.RewardText}");
                ConsoleUtility.PrintTitle("");


                if (quest.ProgressStatus == (int)QuestStatus.Startable) //고른 퀘스트가 시작 가능할때
                {
                    Console.WriteLine("0.나가기");
                    Console.WriteLine("1.퀘스트 수락");

                    int KeyInput = ConsoleUtility.PromptMenuChoice(0, 1);
                    switch (KeyInput)
                    {
                        case 0:
                            return;
                        case 1:
                            quest.ProgressStatus = (int)QuestStatus.InProgress; //해당 퀘스트의 진행상태를 (진행가능)->(진행중)으로 변경
                            return;
                    }
                }
                else if (quest.ProgressStatus == (int)QuestStatus.InProgress) //진행 중인 퀘스트를 선택했을 때 (진행가능 or 진행중인 퀘스트밖에 없어서 굳이 체크 안해도 됨)
                {
                    Console.WriteLine(" - 진행상황 -");
                    bool _canFinish = quest.CheckCondition(player); // player를 매개변수를 받아서 함수(=클리어조건을 체크하는 내용 + 텍스트출력)를 실행
                    Console.WriteLine("");

                    if (_canFinish) //선택한 퀘스트를 완료 가능할떄
                    {
                        Console.WriteLine("0.나가기");
                        Console.WriteLine("1.퀘스트 완료");

                        int KeyInput = ConsoleUtility.PromptMenuChoice(0, 1);
                        switch (KeyInput)
                        {
                            case 0:
                                return;
                            case 1:
                                //퀘스트상태를 완료로 바꾸고 + 보상 지급
                                quest.ProgressStatus = (int)QuestStatus.Finished;
                                quest.Reward(player);
                                return;
                        }
                    }
                    else //선택한 퀘스트를 완료 불가능할때
                    {
                        Console.WriteLine("0.나가기");

                        int KeyInput = ConsoleUtility.PromptMenuChoice(0, 0);
                        switch (KeyInput)
                        {
                            case 0:
                                return;
                        }
                    }
                }
                else if (quest.ProgressStatus == (int)QuestStatus.Finished) //완료된 퀘스트를 선택했을 때
                {
                    Console.Clear();
                    ConsoleUtility.PrintTitle($"{QuestTitle} (완료됨)");
                    Console.WriteLine("");
                    Console.WriteLine(quest.Description);
                    Console.WriteLine("");
                    Console.WriteLine(" - 완료 조건 -");
                    Console.WriteLine($"{quest.ClearConditionText}");
                    Console.WriteLine("");
                    Console.WriteLine(" - 보상 -");
                    Console.WriteLine($"{quest.RewardText}");
                    ConsoleUtility.PrintTitle("");
                    Console.WriteLine("0.나가기");

                    int KeyInput = ConsoleUtility.PromptMenuChoice(0, 0);
                    switch (KeyInput)
                    {
                        case 0:
                            return;
                    }
                }
                    
                
            }
        }

        //먼저 퀘스트 진행상태 저장리스트를 초기화
        //public static List<int> loadedStatuses = LoadQuestStatusFromFile();
    }

    public class Quest
    {
        public string Title;                    //퀘스트 제목
        public string Description;              //퀘스트 내용
        public string ClearConditionText;       //클리어 조건 텍스트
        public string RewardText;               //퀘스트 보상 텍스트

        public Func<Player, bool> CheckCondition;   //클리어 조건을 체크하는 함수. 조건에 따라 ClearConditionText의 문자열 뒤에 어떻게 추가할지 직접 정의해야 함. (ex "\t미니언 (3 / 5)")
        public Action<Player> Reward;           //클리어 보상 함수

        public int ProgressStatus = (int)QuestStatus.Startable;          //클리어 여부를 저장하는 변수. 기본 진행 가능 상태

        public Quest(string title, string description, string clearConditionText, string rewardText, Func<Player, bool> checkCondition, Action<Player> reward)
        {
            Title = title;
            Description = description;
            ClearConditionText = clearConditionText;
            RewardText = rewardText;
            CheckCondition = checkCondition;
            Reward = reward;
        }
    }
}



/*      퀘스트 선택 & 완료 시
        
        (특정 퀘스트 선택 시)

        Quest!!

        마을을 위협하는 미니언 처치

        이봐! 마을 근처에 미니언들이 너무 많아졌다고 생각하지 않나?
        마을주민들의 안전을 위해서라도 저것들 수를 좀 줄여야 한다고!
        모험가인 자네가 좀 처치해주게!

        - 미니언 5마리 처치 (0/5)

        - 보상- 
	        쓸만한 방패 x 1
	        5G

        1. 수락
        2. 거절
        원하시는 행동을 입력해주세요.
        >>


        (특정 퀘스트 완료 시)

        Quest!!

        마을을 위협하는 미니언 처치

        이봐! 마을 근처에 미니언들이 너무 많아졌다고 생각하지 않나??
        마을주민들의 안전을 위해서라도 저것들 수를 좀 줄여야 한다고!
        자네가 좀 처치해주게!

        - 미니언 5마리 처치 (5/5)

        - 보상- 
	        쓸만한 방패 x 1
	        5G

        1. 보상 받기
        2. 돌아가기

        원하시는 행동을 입력해주세요.
        >>
     */