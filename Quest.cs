using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.IO;
using System.Text.Json;
using Newtonsoft.Json;
using System.Security.Cryptography.X509Certificates;
using TextGame;



namespace TextGame
{
    //<><><><>퀘스트 완료 보상 함수를 쓰기 위해 람다 표현식을 쓰느라 아직 Player가 정의되지 않았어요. 에러가 있으니 주의!<><><><>

    //1번째 퀘스트 : 5마리 잡기 + 5마리 중 몇 마리 잡았는지 체크
    //2번째 퀘스트 : 장비를 장착했는지 체크
    //3번째 퀘스트 : 강해졌는지 체크 (지금은 임의로 3레벨 달성했는지로 할게요)

    //퀘스트 진행상태 저장기능 (1.진행 가능 2.진행 중{+진행 중인 세부 내용} 3.완료)
    //퀘스트 저장 목록에 몬스터 킬카운트 필요합니다 <<<<<<퀘스트 조건체크용이라서 진행상태에 몇마리 잡았는지 기록 가능하면 불필요해요

    //처음 보는 변수들은 넣을지 말지 미정

    //몬스터타입
    public enum MonsterType
    {
        Minion,         //0.미니언
        CannonMinion,   //1.대포 미니언
        VoidBug        //2.공허충
    }
    //퀘스트 상태여부

    public class QuestManager
    {
        public List<Quest> questList;
        public QuestManager()       //모든 퀘스트를 정의하는 함수
        {
            List<Quest> quests = new List<Quest>
        {   
            //1.미니언 5마리 잡기
            new Quest(
                "마을을 위협하는 미니언 처치",
                "이봐! 마을 근처에 미니언들이 너무 많아졌다고 생각하지 않나?\n마을주민들의 안전을 위해서라도 저것들 수를 좀 줄여야 한다고!\\n모험가인 자네가 좀 처치해주게!",
                "미니언 5마리 처치",
                "쓸만한 방패 x 1\n5G",
                player => //클리어 조건: 미니언 5마리 잡기
                {
                    if(true)    //player.KillCount[MonsterType.Minion] >= 5 미니언을 5마리 이상 잡았으면
                    {
                        //미니언 (5/5) (완료)
                        //Console.Write(미니언 (5/5) (완료)")
                    }
                    else //못잡았으면
                    {   
                        //미니언 (n/5) (진행중)
                        //Console.Write($"미니언 ( {player.KillCount[MonsterType.Minion]} / 5 ) (진행중)") //변수를 이렇게 쓸지는 미정
                    }
                },
                player => //보상
                {
                    //쓸만한 방패 x 1
                    //inventory.Add(new item("쓸만한 방패", "쓸만한 방패다", ItemType.Shield, 0, 1, 0, 500));
                    //5골드 지급
                    player.Gold += 5;
                }),

            //2.장비 장착하기
            new Quest(
                "장비를 장착해보자",
                "모험을 떠나기 전에 적절한 장비를 갖추는 건 중요하네.\n자네의 인벤토리에 방패를 장착해보게!",
                "쓸만한 방패 장착",
                "100G",
                player => //클리어 조건: 방패 장착하기
                {

                    if(true)    //방패를 장착했으면 (SslmanhanBangpae.isEquipped)
                    {
                        Console.WriteLine("쓸만한 방패 장착 (완료)");
                    }
                    else //방패를 장착 안했으면
                    {
                        Console.WriteLine("쓸만한 방패 장착 (진행중)");
                    }
                },
                player => //보상
                {
                    //쓸만한 방패 x 1
                    //inventory.Add(new item("쓸만한 방패", "쓸만한 방패다", ItemType.Shield, 0, 1, 0, 500));
                    //5골드 지급
                    player.Gold += 5;
                }),
            //3.강해지기
            new Quest(
                "강해지기",
                "강한 모험가들도 처음에는 볼품없는 풋내기였지!\n당장 위험한 던전부터 들어갈 생각하지 말고 실력을 키워야만 하네.\n자네의 성장을 보여주게!",
                "레벨 3 달성",
                "쓸만한 방패 x 1\n5G",
                player => //클리어 조건: 미니언 5마리 잡기
                {

                    if(player.Level >= 3)    //레벨이 3 이상이면
                    {
                        Console.WriteLine("3레벨 달성 (완료)");
                    }
                    else //레벨이 3 미만이면
                    {
                        Console.WriteLine("3레벨 달성 (진행중)");
                    }
                },
                player => //보상
                {
                    //쓸만한 방패 x 1
                    //inventory.Add(new item("쓸만한 방패", "쓸만한 방패다", ItemType.Shield, 0, 1, 0, 500));
                    //5골드 지급
                    player.Gold += 5;
                }),
        };
        }

        public void SelectQuest(int index)   //퀘스트 선택하기
        {
            Console.Clear();
            //1.클리어했으면 "이미 완료한 퀘스트입니다." 출력하고 재입력받기
            //2.클리어를 안했으면 -> 퀘스트를 받았는지 체크
            //2-1.퀘스트를 받았으면 퀘스트 제목,내용,조건,보상 출력 + (진행중) 상태 표시
            //2-2.퀘스트를 안 받았으면 퀘스트 제목,내용,조건,보상 출력&보상 지급

            //private Quest quest = quests[index];

        }

        public void QuestMenu()     //마을 -> 퀘스트 메뉴
        {
            Console.Clear();
            ConsoleUtility.PrintTitle("■ 퀘스트 메뉴 ■");
            Console.WriteLine("진행 가능한 퀘스트들을 볼 수 있습니다.");
            Console.WriteLine("");
            Console.WriteLine("[퀘스트 목록]");

            //quests 리스트 안에 담긴 퀘스트 수만큼 리스트를 보여주고, 선택지 생성
            //퀘스트마다 각각 완료했는지
            for (int i = 0; i < questList.Count; i++) //QuestManager는 나중에 없애고 프로그램 클래스에서 quests를 가져갈 예정
            {
                Console.Write($"{i + 1}. ");
                Console.WriteLine(questList[i].Title + $"{questList[i + 1].ProgressStatus}");
                break;
            }
            ConsoleUtility.PrintTitle("");
            Console.WriteLine("0.나가기");

            int KeyInput = ConsoleUtility.PromptMenuChoice(0, questList.Count);

            switch (KeyInput)
            {
                case 0:
                    //MainMenu(); //메인메뉴 이동
                    break;
                default:
                    SelectQuest(KeyInput - 1);
                    break;
            }

        }

        //먼저 퀘스트 진행상태 저장리스트를 초기화
        //public static List<int> loadedStatuses = LoadQuestStatusFromFile();

    }

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


    public enum QuestStatus
    {
        Startable,          //0.진행 가능
        InPrograss,         //1.진행 중
        Finished            //2.완료
    }

    public class Quest
    {
        public string Title;                    //퀘스트 제목
        public string Description;              //퀘스트 내용
        public string ClearConditionText;       //클리어 조건 텍스트
        public string RewardText;               //퀘스트 보상 텍스트

        internal Action<Player> CheckCondition;   //클리어 조건을 체크하는 함수. 조건에 따라 ClearConditionText의 텍스트 뒤에 어떻게 추가할지 직접 정의해야 함. (ex "\t미니언 (3 / 5)")
        internal Action<Player> Reward;           //클리어 보상 함수

        public int ProgressStatus = (int)QuestStatus.Startable;          //클리어 여부를 저장하는 변수. 기본 진행 가능 상태

        internal Quest(string title, string description, string clearConditionText, string rewardText, Action<Player> checkCondition, Action<Player> reward)
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