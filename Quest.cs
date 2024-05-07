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

    public enum QuestStatus
    {
        Startable,
        InProgress,
        Finished
    }

    public class QuestManager
    {
        public Quest[] questList;
        Player _player;
        Inventory _inventory;

        public QuestManager()
        {
            questList = new Quest[]
        {   
            new Quest(
                "마을을 위협하는 미니언 처치",
                "이봐! 마을 근처에 미니언들이 너무 많아졌다고 생각하지 않나?\n마을주민들의 안전을 위해서라도 저것들 수를 좀 줄여야 한다고!\n모험가인 자네가 좀 처치해주게!",
                "1.미니언 5마리 처치하기",
                "쓸만한 방패 x 1\n5G",
                player =>
                {
                    if(player.KillCount[(int)MonsterType.Minion] >= 5)
                    
                    {
                        Console.WriteLine("미니언 (5/5) (완료)");
                        return true;
                    }
                    else
                    {
                        Console.WriteLine($"미니언 ({player.KillCount[(int)MonsterType.Minion]}/5) (진행중)");
                        return false;
                    }
                },
                player =>
                {
                    Console.WriteLine("쓸만한 방패 x 1");
                    Console.WriteLine("5골드");
                    _inventory.AddItem(new Item("쓸만한 방패", "쓸만한 방패입니다.", ItemType.Shield, 100, 0, 2, 0));
                    player.Gold += 5;
                }),
            new Quest(
                "장비를 장착해보자",
                "모험을 떠나기 전에 적절한 장비를 갖추는 건 중요하네.\n자네의 인벤토리에서 방패를 장착해보게!",
                "1.아무 방패 장착하기",
                "100G",
                player =>
                {   
                    Item shield = _inventory.ItemList.FirstOrDefault(item => item.Type == ItemType.Shield && item.IsEquipped);
                    bool shieldEquipped = (shield != null) ? shield.IsEquipped : false;

                    if(shieldEquipped)
                    {
                        Console.WriteLine("방패 장착 (완료)");
                        return true;
                    }
                    else
                    {
                        Console.WriteLine("방패 장착 (진행중)");
                        return false;
                    }
                },
                player =>
                {
                    _inventory.AddItem(new Item("쓸만한 방패", "쓸만한 방패입니다", ItemType.Shield, 0, 2, 0, 500));
                    player.Gold += 100;
                }),
            new Quest(
                "강해지기",
                "강한 모험가들도 처음에는 볼품없는 풋내기였지!\n당장 위험한 던전부터 들어갈 생각하지 말고 실력을 키워야만 하네.\n자네의 성장을 보여주게!",
                "1.레벨 3 달성",
                "200G",
                player =>
                {
                    if(player.Level >= 3)
                    {
                        Console.WriteLine("3레벨 달성 (완료)");
                        return true;
                    }
                    else
                    {
                        Console.WriteLine("3레벨 달성 (진행중)");
                        return false;
                    }
                },
                player =>
                {
                    player.Gold += 200;
                }),
            };
        }
        public void QuestMenu(Player player, Inventory inventory)
        {
            _player = player;
            _inventory = inventory;

            while (true)
            {
                string ProgressStatusText;

                Console.Clear();
                ConsoleUtility.PrintTitle("■ 퀘스트 메뉴 ■");
                Console.WriteLine("진행 가능한 퀘스트들을 볼 수 있습니다.");
                Console.WriteLine("");
                Console.WriteLine("[퀘스트 목록]");

                List<int> selectableList = new List<int>();
                selectableList.Add(-1);

                for (int i = 0; i < questList.Length; i++)
                {
                    if (questList[i].ProgressStatus == (int)QuestStatus.Finished)
                    {
                        ProgressStatusText = " (완료됨)";
                        selectableList.Add(i);
                        Console.Write($"{selectableList.Count - 1}. ");
                        Console.WriteLine(questList[i].Title + $"{ProgressStatusText}");
                    }
                    else
                    {
                        ProgressStatusText = (questList[i].ProgressStatus == (int)QuestStatus.Startable) ? " (진행가능)" : " (진행중)";
                        selectableList.Add(i);
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
                        return;
                    default:
                        SelectQuest(_player, KeyInput-1);
                        break;
                }
            }
        }
        public void SelectQuest(Player player, int index)
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


                if (quest.ProgressStatus == (int)QuestStatus.Startable)
                {
                    Console.WriteLine("0.나가기");
                    Console.WriteLine("1.퀘스트 수락");

                    int KeyInput = ConsoleUtility.PromptMenuChoice(0, 1);
                    switch (KeyInput)
                    {
                        case 0:
                            return;
                        case 1:
                            quest.ProgressStatus = (int)QuestStatus.InProgress;
                            return;
                    }
                }
                else if (quest.ProgressStatus == (int)QuestStatus.InProgress)
                {
                    Console.WriteLine(" - 진행상황 -");
                    bool _canFinish = quest.CheckCondition(player);
                    Console.WriteLine("");

                    if (_canFinish)
                    {
                        Console.WriteLine("0.나가기");
                        Console.WriteLine("1.퀘스트 완료");

                        int KeyInput = ConsoleUtility.PromptMenuChoice(0, 1);
                        switch (KeyInput)
                        {
                            case 0:
                                return;
                            case 1:
                                quest.ProgressStatus = (int)QuestStatus.Finished;
                                quest.Reward(player);
                                return;
                        }
                    }
                    else
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
                else if (quest.ProgressStatus == (int)QuestStatus.Finished)
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
    }

    public class Quest
    {
        public string Title;
        public string Description;
        public string ClearConditionText;
        public string RewardText;

        [NonSerialized]
        public Func<Player, bool> CheckCondition;
        [NonSerialized]
        public Action<Player> Reward;


        public int ProgressStatus = (int)QuestStatus.Startable;

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