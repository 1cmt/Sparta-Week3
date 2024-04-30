using Sparta_week3;
using System;

namespace TextGame // Note: actual namespace depends on the project name.
{
    internal class GameManager
    {
        public GameManager()
        {
            InitializeGame();
        }
        public void InitializeGame()
        {
            //저장한 데이터를 불러오는 과정
        }
        public void StartGame()
        {
            Console.Clear();
            ConsoleUtility.PrintHead();
            MainMenu();
        }

        private void MainMenu()
        {
            //프로그램 진행과정 
            Player player = new Player("Chad", "전사");
            Store store = new Store();
            Inventory inventory = new Inventory();

            while (true)
            {
                Console.Clear();

                // 1. 선택 멘트를 줌
                Console.WriteLine("■■■■■■■■■■■■■■■■■■■■■■■■■■■■");
                Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다.");
                Console.WriteLine("이곳에서 던전으로 들어가기 전 활동을 할 수 있습니다.");
                Console.WriteLine("■■■■■■■■■■■■■■■■■■■■■■■■■■■■");
                Console.WriteLine("");

                Console.WriteLine("1. 상태보기");
                Console.WriteLine("2. 인벤토리");
                Console.WriteLine("3. 상점");
                Console.WriteLine("");
                Console.WriteLine("0. 나가기");
                Console.WriteLine("");

                // 2. 선택한 결과를 검증함
                int choice = ConsoleUtility.PromptMenuChoice(0, 3);

                // 3. 선택한 결과에 따라 보내줌
                switch (choice)
                {
                    case 0:
                        return;
                    case 1:
                        //StatusMenu();
                        break;
                    case 2:
                        inventory.InventoryMenu(player);
                        break;
                    case 3:
                        store.StoreMenu(player, inventory);
                        break;
                }

            }


        }

        public static void Main()
        {
            GameManager gamemanager = new GameManager();
            gamemanager.StartGame();
        }
    }
}