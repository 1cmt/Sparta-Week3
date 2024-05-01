using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sparta_week3
{
    internal class ConsoleUtility
    {
        public static void PrintHead()
        {
            Console.WriteLine("===============================================================");
            Console.WriteLine("            ██████╗  ██╗ █████╗ ██╗  ██╗                       ");
            Console.WriteLine("            ██╔══██╗███║██╔══██╗██║  ██║                       ");
            Console.WriteLine("            ██████╔╝╚██║███████║███████║                       ");
            Console.WriteLine("            ██╔══██╗ ██║██╔══██║╚════██║                       ");
            Console.WriteLine("            ██████╔╝ ██║██║  ██║     ██║                       ");
            Console.WriteLine("            ╚═════╝  ╚═╝╚═╝  ╚═╝     ╚═╝                       ");
            Console.WriteLine("                                                               ");
            Console.WriteLine("██████╗ ██╗   ██╗███╗   ██╗ ██████╗ ███████╗ ██████╗ ███╗   ██╗");
            Console.WriteLine("██╔══██╗██║   ██║████╗  ██║██╔════╝ ██╔════╝██╔═══██╗████╗  ██║");
            Console.WriteLine("██║  ██║██║   ██║██╔██╗ ██║██║  ███╗█████╗  ██║   ██║██╔██╗ ██║");
            Console.WriteLine("██║  ██║██║   ██║██║╚██╗██║██║   ██║██╔══╝  ██║   ██║██║╚██╗██║");
            Console.WriteLine("██████╔╝╚██████╔╝██║ ╚████║╚██████╔╝███████╗╚██████╔╝██║ ╚████║");
            Console.WriteLine("╚═════╝  ╚═════╝ ╚═╝  ╚═══╝ ╚═════╝ ╚══════╝ ╚═════╝ ╚═╝  ╚═══╝");
            Console.WriteLine("===============================================================");
            Console.WriteLine("                    press any key to start                     ");
            Console.WriteLine("===============================================================");
            Console.ReadKey();
            Console.Clear();
            ConsoleUtility.PrintLine('=');
            Console.WriteLine("스파르타 던전 생성중...");
            Thread.Sleep(1000);
            Console.WriteLine("몬스터 생성중...");
            Thread.Sleep(1000);
            Console.WriteLine("장비 제작중...");
            Thread.Sleep(1000);
            ConsoleUtility.PrintLine('=');
        }
        public static int PromptMenuChoice(int min, int max)
        {
            while (true)
            {
                Console.Write("원하시는 번호를 입력해주세요: ");
                if (int.TryParse(Console.ReadLine(), out int choice) && choice >= min && choice <= max)
                {
                    return choice;
                }
                Console.WriteLine("잘못된 입력입니다. 다시 시도해주세요.");
            }
        }
        public static void PrintTitle(string title)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(title);
            Console.ResetColor();
        }
        public static void PrintTextHighlightsColor(ConsoleColor color, string s1, string s2, string s3 = "")
        {
            Console.Write(s1);
            Console.ForegroundColor = color ;
            Console.Write(s2);
            Console.ResetColor();
            Console.Write(s3);
        }
        public static int GetPrintableLength(string str)
        {
            int length = 0;
            foreach (char c in str)
            {
                if (char.GetUnicodeCategory(c) == System.Globalization.UnicodeCategory.OtherLetter)
                {
                    length += 2; // 한글과 같은 넓은 문자에 대해 길이를 2로 취급
                }
                else
                {
                    length += 1; // 나머지 문자에 대해 길이를 1로 취급
                }
            }

            return length;
        }

        public static string PadRightForMixedText(string str, int totalLength)
        {
            // 가나다
            // 111111
            int currentLength = GetPrintableLength(str);
            int padding = totalLength - currentLength;
            return str.PadRight(str.Length + padding);
        }

        public static void PrintLine(char ch,int i = 110)
        {
            for (i = 0; i < 110; i++)
            {
                Console.Write(ch);
            }
            Console.Write("\n");
        }
    }
}

