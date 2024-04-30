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
            Player P1 = new Player("Test", "Tester");
            Console.WriteLine($"현재레벨:{P1.Level},현재 경험치{P1.Cexp}");
            P1.Cexp = 10;
            P1.Levelup(ref P1.Level, ref P1.Cexp);
            Console.WriteLine($"현재레벨:{P1.Level},현재 경험치{P1.Cexp}");
        }

        public static void Main()
        {
            GameManager gamemanager = new GameManager();
            gamemanager.StartGame();
        }
    }
}