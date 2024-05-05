# Sparta-Week3
스파르타 코딩클럽 3주차에 진행된 심화 팀프로젝트 과제에 대한 결과물입니다.
## 🤾‍♂️프로젝트 소개
"던전 배틀을 C# 콘솔을 통해 텍스트 게임으로 구현"
## 🕒개발 시간
- 2024.4.29 ~ 2024.5.7
## 💁‍♂️팀원 소개
- **이충민** : 팀장, Item, Inventory, Store, Program(인벤토리, 상점 관련) 클래스 구현
- **이무진** : 팀원, Dungeon, Monster , Program(던전 관련) 클래스 구현
- **곽승우** : 팀원, Quest, QuestManager, Program(퀘스트 관련) 클래스 구현
- **김희환** : 팀원, ConsoleUtility, Player, Skill, Program(플레이어, 데이터 저장을 위한 json 관련) 클래스 구현

✅ Program 클래스는 각자가 구현한 클래스에 관련 필요한 부분을 공동으로 개발, 데이터 저장 관련은 김희환 팀원 분이 구현

## ✍️클래스 및 주요 관련 정보
- **Dungeon** : 전투가 벌어지는 던전과 관련된 클래스
   - **MonsterAppear** : 1~4 마리의 몬스터가 랜덤으로 등장하는 메서드
   - **Battle** : 몬스터와 플레이어의 전투, 등장 몬스터 출력, 플레이어 정보 출력, 행동선택지를 제공하는 다양한 역할을 하는 메서드
     
- **Monster** : 몬스터에 대한 이름(Name), 레벨(Level), 공격력(Atk), 체력(Hp) 등의 정보가 담긴 클래스
   - **Attack** : 몬스터가 플레이어를 공격하는 메서드입니다. (몬스터 공격력 - 플레이어 방어력) * 0.1%(오차) 반올림으로 처리
   - **Dead** : 몬스터 사망과 관련된 메서드
     
- **Quest** : 퀘스트와 관련된 정보가 담긴 클래스
   - **Title(string)** : 퀘스트 제목
   - **Description(string)** : 퀘스트 내용
   - **ClearConditionText(string)** : 클리어 조건이 담긴 문자열
   - **RewardText(string)** : 퀘스트 보상이 담긴 문자열
   - **CheckCondition(Action<Player>)** : 클리어 조건을 체크할 때 쓰이는 반환값이 없는 대리자(Delegate)
   - **Reward(Func<Player, bool>)** : 클리어 보상을 지급하는 데 쓰이는 반환값이 bool 타입인 대리자
   - **QuestStatus(enum)** : 각각의 퀘스트에 대한 진행상태(진행가능/진행중/완료)를 표시하기 위해 열거형으로 선언
     
- **QuestManager** : 퀘스트를 관리하는 퀘스트 매니저 클래스
   - **QuestMenu** : 퀘스트 메뉴를 여는 메서드
   - **SelectQuest** : 플레이어 객체와 index를 받아 퀘스트를 선택하게 하는 메서드
     
- **ConsoleUtility** : 여러 클래스를 제작하면서 공통으로 자주 사용하게되는 메서드들을 하나로 모아둔 유틸리티 클래스
   - **PrintTextHighlightsColor** : 색상 정보, 문자열 3개를 매개변수로 받아 가운데 문자열에 대해 받아온 색상 정보로 출력해주는 메서드
   - **PrintTitle** : 문자열(title)을 받아 색상을 넣어서 출력해주는 메서드
   - **PadRightForMixedText** : 문자열(str)을 출력하고 정수값(totalLength) 만큼 공백을 출력해주는 메서드
   - **PrintLine** : 문자(ch)를 정수값(size)만큼 출력해주고 줄을 개행해주는 메서드
   - **PrintHead** : 게임의 초기화면을 보여주는 메서드
   - **PromptMenuChoice** : 지정한 범위(min, max) 내에 있는 숫자인지 검사하고 숫자를 반환하는 메서드
     
- **Player**
   - **LevelUp** : 정수형 매개변수인 cexp와 멤버 변수 Texp 값이 같아졌다면 Texp를 증가시키고 레벨과 스탯을 증가시키는 메서드
   - **CheckJob(No Parameter)** : 직업명에 대한 입력이 올바른지 확인하고 입력을 반환하는 메서드
   - **InputJob** : 직업 선택에 대한 메서드
   - **InputName** : 이름을 받는 메서드
   - **ChangeJob** : 골드가 충분한지 확인하고 직업을 변경해주는 메서드
   - **StatusMenu** : 플레이어와 인벤토리에 대한 정보를 매개변수로 받아 내 상태를 보여주는 메서드
   - **CheckStat** : 현재 스탯을 최신화하는 메서드
   - **UncheckJob** : 직업마다 존재하는 배율을 꺼주는 메서드
   - **CheckJob** : 직업명에 대한 string 값을 매개변수로 받고 이에 해당하는 직업에 대한 배율을 Multiple 메서드를 통해 켜주는 메서드
     - **Multiple** : 원하는 수(float) 만큼 곱해주고 정수형으로 명시적 형변환하여 반환을 하는 메서드
       
- **Skill**
   - **PrintSkillDescription** : 플레이어의 스탯을 기반으로 스킬설명을 출력하는 메서드
   - **SkillUse** : 매개 변수로 사용할 스킬이 몇 번째 스킬인지 정수형(index)값과 플레이어 객체를 받아 사용할 스킬, 스킬 데미지를 반환하는 메서드
   - **Gainskill** :  변경된 직업의 스킬로 반환값인 Skill 클래스 배열을 할당하는 메서드
     
- **Item** : 아이템에 대한 정보와 아이템 관련 출력 메서드를 포함하고 있는 클래스
   - **ItemType** : 아이템이 어떠한 종류(type)인지에 대한 열거형
   - **MenuType** : 아이템 정보가 출력되는 메뉴가 어떤 메뉴인지에 대한 열거형
   - **Desc** : 아이템에 대한 설명이 담긴 문자열
   - **SellPrice** : 아이템 구매 후 다시 상점에 판매하려 할 때 받을 수 있는 골드 값
   - **IsPurchased** : 아이템을 구매했는 지에 대한 bool값
   - **IsEquipped** : 아이템을 장착했는 지에 대한 bool값
   - **PrintStat** : 아이템에 대한 스탯 정보를 출력
   - **PrintController** : 아이템 정보를 출력하는 메뉴(MenuType)와 아이템의 인덱스에 대한 정보(int)를 매개변수로 받아 이에 맞는 메서드를 수행
     - **PrintIncludeEquipInfo** : 아이템 장착 여부를 포함해 출력하는 메서드, 인덱스나 가격 정보까지 출력할건지 bool값을 매개변수로 받아 처리
     - **PrintExcludeEquipInfo** : 아이템 장착 여부를 제외하고 정보를 출력하는 메서드, 인덱스도 출력한건지 bool값을 매개변수로 받아 처리
     - **PrintPrice** : 아이템 정보를 출력하는 메뉴(MenuType)에 따라 아이템 가격 정보 혹은 구매여부까지 포함하여 출력하는 메서드
   - **ToggleEquipStatus** : 아이템의 장착 여부를 변경하는 메서드
   - **Purchase** : 아이템 구매시 IsPurchased 변수 값을 true로 변경하는 메서드
   - **Sell** : 아이템 판매시에 IsPurchased 변수 값을 false로 변경하는 메서드

- **Inventory** : 인벤토리와 관련된 클래스
   - **ItemList** : 아이템을 리스트 형식으로 관리
   - **RemoveItem** : 인벤토리에서 아이템 제거한다는 것은 판매한다는 것이고 해당 아이템의 Sell 메서드를 호출하고 리스트에서 제거하는 메서드
   - **BonusAtk/BonusDef/BonusHp** : 아이템을 장착하면 생기는 보너스 스탯을 변수로 저장, 부위마다 아이템 장착하기에 값이 누적될 수 있음
     - **ManageBonusValue** : 보너스 스탯을 관리하는 함수, 계산하는 CalBonus 메서드를 호출, 오버로딩으로 되어있어 인덱스로도 아이템 객체로도 관리할 수 있음
       -**CalBonus** : 아이템 해제냐 장착이냐에 따라 아이템에 있는 스탯을 보너스 스탯서 더하거나 빼는 메서드
   - **InventoryMenu** : 인벤토리 메뉴를 출력하는 메서드, 보유 중인 아이템을 출력하고 콘솔서 입력을 받아 장착 관리 페이지로 넘어갈 수 있음
   - **EquipMenu** : 아이템 장착 관리 페이지를 출력하는 메서드, 아스키 아트로 만든 인벤토리와 아이템 목록을 출력하고 콘솔의 입력에 따라 장착 부위 별 아이템 장착 혹은 해제하는 메서드
     -**printInventory** : 보유 아이템 정보를 아스키 아트로 표현하는 메서드, 텍스트 Padding에 대한 이해를 위해 만듬
   - **PrintMyItemList** : 인벤토리 메뉴인지 장착 관리 메뉴인지를 MenuType으로 매개변수를 받아 아이템 객체의 PrintController에 정보를 넘겨 정보 출력

- **Store** : 상점과 관련된 클래스
  - **StoreInventory** : 상점에 존재하는 아이템을 저장하는 아이템 객체 배열
  - **ItemNumByType** : 상점에 아이템 종류(ItemType) 별로 몇 개의 아이템이 존재하는 지 저장하는 정수형 배열
    - **CheckItemNum** : ItemNumByType 배열에 기록하기 위해 0번 인덱스 아이템부터 돌면서 아이템 종류를 확인하여 ItemNumByType 배열에 기록하는 메서드
  - **StoreMenu** : 상점 메뉴를 출력하는 메서드, 콘솔서 입력을 받아 아이템 구매, 아이템 판매 메뉴로 들어갈 수 있음
       
