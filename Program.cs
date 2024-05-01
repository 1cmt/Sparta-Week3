using Sparta_week3;
using System;

namespace TextGame // Note: actual namespace depends on the project name.
{
    public class GameManager
    {
        public static GameManager instance = new GameManager();
        public QuestManager questManager;
        public GameManager()
        {
            InitializeGame();
        }
        public void InitializeGame()
        {
            questManager = new QuestManager();
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
            questManager.QuestMenu();

        }

        public static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8; //콘솔 출력텍스트 깨짐 방지
            instance.StartGame();
        }
    }
}