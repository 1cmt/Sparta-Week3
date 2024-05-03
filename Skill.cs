using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TextGame;

namespace TextGame
{
    [Serializable]
    public class Skill
    {
        public string Skillname;
        public string Skilldes;
        public int Damage;
        public int Mplose;

        public Skill(string name, string des, int damage, int mplose)
        {
            Skillname = name;
            Skilldes = des;
            Damage = damage;
            Mplose = mplose;
        }
        public void PrintSkillDescription(Player player)
        {
            Console.Write("-");
            ConsoleUtility.PrintTextHighlightsColor(ConsoleColor.Yellow, "",ConsoleUtility.PadRightForMixedText(Skillname,12), ConsoleUtility.PadRightForMixedText(Skilldes,43));
            Console.Write("|Damage:"+(int)(Damage * player.Atk/10)+"|MP:"+Mplose+"\n");
        }
        public int SkillUse(int index, Player player) //index번째 스킬 사용할지와 player객체 받는다.
        {
            int totaldamage;
            totaldamage = (int)((player.skillbook[index-1].Damage)*player.Atk / 10);
            return totaldamage;
        }
        public static Skill[] Gainskill(string job)
        {
            //직업마다 List<스킬>형인 스킬북을 가져간다 전투에서 최종 데미지는 player.atk값에 스킬damage를 곱한뒤 10으로 나눈다
            switch (job)
            {
                case "ASSASSIN":
                    Skill[] AssassinBook = new Skill[2] {           
                  new Skill("단검찌르기", "|단검으로 적을 빠르게 찌릅니다", 15, 10),
                  new Skill("달팽이던지기", "|달팽이 껍질을 던집니다", 11, 5)};
                    return AssassinBook;
                case "WIZARD":
                        Skill[] WizardBook = new Skill[2]{
                   new Skill("얼음창", "|얼음창을 소환해 적에게 날립니다", 20, 10),
                   new Skill("파이어볼", "|화염구를 소환해 적에게 날립니다", 20, 10)};
                    return WizardBook;
                case "ARCHER":
                    Skill[] ArcherBook = new Skill[2]{
                        new Skill("연사", "|적에게 화살을 여러번 날립니다", 15, 5),
                        new Skill("달팽이던지기", "|달팽이 껍질을 던집니다", 11, 5) };
                    return ArcherBook;
                case "WARRIOR":
                    Skill[] WarriorBook = new Skill[2] {
                    new Skill("숄더차지", "|어깨로 적을 강하게 밀어붙입니다", 15, 20),
                    new Skill("리프 어택", "|전방으로 높이 점프한후 적을 깔아뭉갭니다", 20, 30) };
                    return WarriorBook;
                default:
                    return null;
            }
        }
    }
}
