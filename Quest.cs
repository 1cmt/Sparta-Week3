﻿using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.IO;
using Newtonsoft.Json;
using Sparta_week3;
using System.Security.Cryptography.X509Certificates;



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
    Minion,         //ex)0.미니언
    Monster2,
    Monster3,
    Monster4,
    Monster5,
    Monster6
}
//퀘스트 상태여부
public enum QuestStatus
{
    Prograssable,       //0.진행 가능
    InPrograss,         //1.진행 중
    Finished            //2.완료
}

internal class Quest
{   
    public string Title;                    //퀘스트 제목
    public string Description;              //퀘스트 내용
    public string ClearConditionText;       //클리어 조건 텍스트
    public string RewardText;               //퀘스트 보상 텍스트

    public Action<Player> CheckCondition;   //클리어 조건을 체크하는 함수. 조건에 따라 ClearConditionText의 텍스트 뒤에 어떻게 추가할지 직접 정의해야 함. (ex "\t미니언 (3 / 5)")
    public Action<Player> Reward;           //클리어 보상 함수

    public bool isCleared = false;

    public Quest(string title, string description, string clearConditionText, string rewardText, Action<Player> checkCondition, Action<Player> reward)
    {
        Title = title;
        Description = description;
        ClearConditionText = clearConditionText;
        RewardText = rewardText;
        CheckCondition = checkCondition;
        Reward = reward;
    }

    public List<Quest> GetInitialQuests()       //모든 퀘스트를 정의하는 함수
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
                    if(player.KillCount[MonsterType.Minion] >= 5)    //미니언을 5마리 이상 잡았으면
                    {
                        //미니언 (5/5) (완료)
                        //Console.Write(미니언 (5/5) (완료)")
                    }
                    else //못잡았으면
                    {   
                        //미니언 (n/5) (진행중)
                        //Console.Write($"미니언 ( {player.KillCount[MonsterType.Minion]} / 5 ) (진행중)")
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
                "장비 장착하기",
                "모험을 떠나기 전에 적절한 장비를 갖추는 건 중요하네.\n자네의 인벤토리에 방패를 장착해보게!",
                "쓸만한 방패 장착",
                "100G",
                player => //클리어 조건: 방패 장착하기
                {

                    if(true)    //방패를 장착했으면 (SslmanhanBangpae.isEquipped)
                    {
                        Console.WriteLine("쓸만한 방패 장착 (진행중)");
                    }
                    else //방패를 장착 안했으면
                    {
                        Console.WriteLine("쓸만한 방패 장착 (완료)");
                    }
                },
                player => //보상
                {
                    //쓸만한 방패 x 1
                    //inventory.Add(new item("쓸만한 방패", "쓸만한 방패다", ItemType.Shield, 0, 1, 0, 500));
                    //5골드 지급
                    player.Gold += 5;
                }),
            //"장비를 장착해보자",
            new Quest(
                "강해지기",
                "강한 모험가들도 처음에는 볼품없는 풋내기였지!\n당장 위험한 던전부터 들어갈 생각하지 말고 실력을 키워야만 해.\n자네의 성장을 보여주게!",
                "레벨 2 달성",
                "쓸만한 방패 x 1\n5G",
                player => //클리어 조건: 미니언 5마리 잡기
                {

                    if(player.Level >= 3)    //레벨이 3 이상이면
                    {

                    }
                    else //레벨이 3 미만이면
                    {

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
            //new Quest("더욱 더 강해지기!",1,2,3),


        };
        return quests;
    }


    public void QuestMenu()     //퀘스트 메뉴
    {
        Console.Clear();
        ConsoleUtility.PrintTitle("■ 퀘스트 보기 ■");
        Console.WriteLine("현재 퀘스트들의 진행상태를 볼 수 있습니다.");
        Console.WriteLine("");
        Console.WriteLine("[퀘스트 목록]");

        //quests 리스트 안에 담긴 퀘스트 수만큼 리스트를 보여주고, 선택지 생성
        //퀘스트마다 각각 완료했는지
        //for (int i = 0, i < quests.Count; i++) {}
    }

    public void SelectQuest(int num)   //퀘스트 선택하기
    {
        //1.클리어했으면 "이미 완료한 퀘스트입니다." 출력하고 재입력받기
        //2.클리어를 안했으면 -> 퀘스트를 받았는지 체크
        //2-1.퀘스트를 받았으면 퀘스트 제목,내용,조건,보상 출력 + (진행중) 상태 표시
        //2-2.퀘스트를 안 받았으면 퀘스트 제목,내용,조건,보상 출력
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