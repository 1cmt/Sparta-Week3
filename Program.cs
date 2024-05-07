using System;
using static System.Formats.Asn1.AsnWriter;
using System.Numerics;
using Newtonsoft.Json;

namespace TextGame // Note: actual namespace depends on the project name.
{
    public class GameManager
    {
        public static GameManager instance = new GameManager();
        QuestManager questManager;
        Inventory inventory;
        Store store;
        Player player;
        Dungeon dungeon;
        string folderpath;
        string filePathPlayer;
        string filePathInventory;
        string filePathStore;
        string filePathQuest;
        public GameManager()
        {
            InitializeGame();
        }
        public void InitializeGame()
        {
            dungeon = new Dungeon();            
        }
        public void StartGame()
        {
            Console.Clear();
            ConsoleUtility.PrintHead();
            string name = Player.InputName();
            bool isfirst = LoadGame(name);
            if (isfirst == true)
            { 
                string job = Player.InputJob(); 
                player = new Player(name, job);
            }
            MainMenu();
        }
        public bool LoadGame(string nameinput)
        {
            string name = nameinput;
            SetFilePath(nameinput);
            if (!Directory.Exists(folderpath))
            {
                Directory.CreateDirectory(folderpath);
                Console.WriteLine($"캐릭터 {name}이 생성 되었습니다");
                bool isMake = true;
                inventory = new Inventory();
                store = new Store();
                questManager = new QuestManager();
                questManager.CheckResetConditionReward();
                return isMake;
            }
            else
            {
                Console.WriteLine($"캐릭터 {name}이 로드 되었습니다");

                string statusFile = File.ReadAllText(filePathPlayer);
                player = JsonConvert.DeserializeObject<Player>(statusFile);
                string inventoryFile = File.ReadAllText(filePathInventory);
                inventory = JsonConvert.DeserializeObject<Inventory>(inventoryFile);
                string storeFile = File.ReadAllText(filePathStore);
                store = JsonConvert.DeserializeObject<Store>(storeFile);
                string questFile = File.ReadAllText(filePathQuest);
                questManager = JsonConvert.DeserializeObject<QuestManager>(questFile);
                bool isMake = false;
                return isMake;
            }
        }
        public void SetFilePath(string name)
        {
            folderpath = $@".\{name}";
            filePathPlayer = Path.Combine(folderpath, $"{name}player.json");
            filePathInventory = Path.Combine(folderpath, $"{name}inventory.json");
            filePathStore = Path.Combine(folderpath, $"{name}store.json");
            filePathQuest = Path.Combine(folderpath, $"{name}quest.json");
        }


        private void MainMenu()
        {
            while (true)
            {
                {
                    string statusFile = JsonConvert.SerializeObject(player);
                    File.WriteAllText(filePathPlayer, statusFile);
                    string inventoryFile = JsonConvert.SerializeObject(inventory);
                    File.WriteAllText(filePathInventory, inventoryFile);
                    string storeFile = JsonConvert.SerializeObject(store);
                    File.WriteAllText(filePathStore, storeFile);
                    string questFile = JsonConvert.SerializeObject(questManager);
                    File.WriteAllText(filePathQuest, questFile);
                }
                player.CheckStat(inventory);
                Console.Clear();
                // 1. 선택 멘트를 줌
                ConsoleUtility.PrintLine('■');
                Console.WriteLine("B1A4 마을에 오신 여러분 환영합니다.");
                Console.WriteLine("이곳에서 던전으로 들어가기 전 활동을 할 수 있습니다.");
                ConsoleUtility.PrintLine('■');
                Console.WriteLine("");

                Console.WriteLine("1. 상태보기");
                Console.WriteLine("2. 인벤토리");
                Console.WriteLine("3. 상점");
                Console.WriteLine("4. 퀘스트");
                Console.WriteLine("5. 던전입장");
                Console.WriteLine("6. 여관");
                Console.WriteLine("");
                Console.WriteLine("0. 나가기");
                Console.WriteLine("");

                // 2. 선택한 결과를 검증함
                int choice = ConsoleUtility.PromptMenuChoice(0, 6);

                // 3. 선택한 결과에 따라 보내줌
                switch (choice)
                {
                    case 0:
                        return;
                    case 1:
                        player.StatusMenu(inventory);
                        break;
                    case 2:
                        inventory.InventoryMenu(player);
                        break;
                    case 3:
                        store.StoreMenu(player, inventory);
                        break;
                    case 4:
                        questManager.QuestMenu(player, inventory);
                        break;
                    case 5:
                        dungeon.EnterDungeon(player, questManager);
                        break;
                    case 6:
                        Console.Clear();
                        Console.WriteLine(
                            "여관에서 휴식을 취하면 Hp와 Mp를 회복할 수 있습니다.\n\n" +
                            "1. 휴식을 취한다 (300Gold)\n" +
                            "0. 나간다\n");

                        int choice2 = ConsoleUtility.PromptMenuChoice(0, 1);

                        //if (choice2 == 0) break;
                        if (choice2 == 1)
                        {   
                            if (player.Gold < 300) 
                            {
                                Console.Clear();
                                Console.WriteLine("Gold가 부족합니다. ");
                                Console.WriteLine("\n아무키나 입력해서 넘어가 주세요.");
                                Console.ReadKey();
                                break;
                            }
                            Console.Clear();
                            Console.WriteLine("휴식을 취하는 중...");
                            Thread.Sleep(1700);
                            Console.WriteLine($"{player.Name}은(는) 기운을 차렸다!");
                            Thread.Sleep(1000);

                            player.Hp = player.MaxHp;
                            player.Mp = player.MaxMp;
                            player.Gold -= 300;
                        }
                        break;
                }

            }

        }

        public static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            instance.StartGame();
        }
    }
}