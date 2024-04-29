using System;
using System.Collections.Generic;
using System.Numerics;

//<><><><>퀘스트 완료 보상 함수를 쓰기 위해 람다 표현식을 쓰느라 아직 Player가 정의되지 않았어요. 에러가 있으니 주의!<><><><>

//1번째 퀘스트 : 5마리 잡기 + 5마리 중 몇 마리 잡았는지 체크
//2번째 퀘스트 : 장비를 장착했는지 체크
//3번째 퀘스트 : 강해졌는지 체크 (임의로 만들라는 뜻 같아요. 제가 거의 터널시야라 3번째 퀘스트에 대한 추가 내용을 못 찾은 걸수도 있어요)

//필요 : 퀘스트 진행상태 저장기능 (1.진행 가능 2.진행 중{+진행 중인 세부 내용} 3.완료)
//필요 : 몬스터 킬카운트 필요합니다 <<<<<<퀘스트 조건체크용이라서 진행상태에 몇마리 잡았는지 기록 가능하면 불필요

//1.퀘스트를 수령 후 

//퀘스트 클래스
public class Quest
{
    public string Title;        //퀘스트 제목
    public string Description;       //퀘스트 내용
    public string ClearConditionText;      //클리어 조건 텍스트
    public string RewardText;       //퀘스트 보상 텍스트

    public Action<Player> CheckCondition;      //클리어 조건을 직접 체크하는 함수. 조건에 따라 ClearConditionText의 텍스트를 어떻게 바꿀지 직접 정의해야 함. (ex 미니언 (3 / 5))
    public Action<Player> Reward;      //클리어 보상 함수 (람다 표현식)

    public Quest(string title, string description, string clearConditionText, string rewardText, Action<Player> checkCondition, Action<Player> reward)
    {
        Title = title;
        Description = description;
        ClearConditionText = clearConditionText;
        RewardText = rewardText;
        CheckCondition = checkCondition;
        Reward = reward;
    }

    public List<Quest> GetInitialQuests()       //퀘스트들을 초기화하는 함수
    {
        List<Quest> quests = new List<Quest>
        {
            new Quest(
                "마을을 위협하는 미니언 처치",
                "이봐! 마을 근처에 미니언들이 너무 많아졌다고 생각하지 않나?\r\n마을주민들의 안전을 위해서라도 저것들 수를 좀 줄여야 한다고!\r\n모험가인 자네가 좀 처치해주게!",
                "미니언 5마리 처치 (0/5)",
                "쓸만한 방패 x 1\r\n\t5G",
                player =>
                {
                    //쓸만한 방패 x 1,
                    player.Gold += 5;
                },
                player =>
                {

                }),
            //new Quest("장비를 장착해보자",1,2,3),
            //new Quest("더욱 더 강해지기!",1,2,3),


        };
        return quests;
    }


    public void OpenQuest()     //퀘스트 보기
    {
        Console.Clear();

    }

    public void SelectQuest()   //퀘스트 선택하기
    {
        //퀘스트 3개 생성
        //1.클리어했는지 체크
        //1-1.클리어를 했으면 "이미 완료한 퀘스트입니다." 출력
        //1-2.클리어를 안했으면 아래 조건 체크
        //2.퀘스트를 받았는지
        //2-1.퀘스트를 받았으면 
        //2-2.퀘스트를 안 받았으면
    }

    //OpenQuestDetails(int num) num번 선택창 열기
}

/*      
        
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