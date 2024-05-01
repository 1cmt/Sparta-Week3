using Sparta_week3;
using System;

namespace TextGame // Note: actual namespace depends on the project name.
{
    internal class GameManager
    {
        public List<Quest> quests;
        public GameManager()
        {
            InitializeGame();
         }
        public void InitializeGame()
        {
            quests = Quest.GetInitialQuests(); //quest 리스트 가져오기
            //저장한 데이터를 불러오는 과정
        }
        public void StartGame()
        {
            Console.Clear();
            ConsoleUtility.PrintHead();
            string name = Player.InputName();
            string job = Player.InputJob();
            Player player = new Player(name, job);
            MainMenu();
        }

        private void MainMenu()
        {
            //프로그램 진행과정 
            
        }

        public static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            GameManager gamemanager = new GameManager();
            gamemanager.StartGame();
        }
    }
}