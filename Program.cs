using System;
using static System.Formats.Asn1.AsnWriter;
using System.Numerics;

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
        public GameManager()
        {
            InitializeGame();
        }
        public void InitializeGame()
        {
            inventory = new Inventory();
            store     = new Store();
            questManager = new QuestManager();
            dungeon = new Dungeon();
            //저장한 데이터를 불러오는 과정
        }
        public void StartGame()
        {
            Console.Clear();
            ConsoleUtility.PrintHead();
            string name = Player.InputName();
            string job = Player.InputJob();
            player = new Player(name, job);
            MainMenu();
            
        }

        private void MainMenu()
        {
            while (true)
            {

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
                Console.WriteLine("");
                Console.WriteLine("0. 나가기");
                Console.WriteLine("");

                // 2. 선택한 결과를 검증함
                int choice = ConsoleUtility.PromptMenuChoice(0, 4);

                // 3. 선택한 결과에 따라 보내줌
                switch (choice)
                {
                    case 0:
                        return;
                    case 1:
                        player.StatusMenu(player,inventory);
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
                        dungeon.EnterDungeon(player);
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