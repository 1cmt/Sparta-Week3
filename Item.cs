using System.Security.Cryptography.X509Certificates;

namespace TextGame
{

    public enum ItemType
    {
        Weapon,
        Armor,
        Helmet,
        Shield,
        Potion
    }

    public enum MenuType
    {
        StoreMenu,
        PurchaseMenu,
        SellMenu,
        InventoryMenu,
        EquipMenu
    }

    public class Item
    {
        public string Name { get; }
        public string Desc { get; }
        public int Price { get; }
        public int SellPrice { get; }

        public ItemType Type { get; }

        public int Atk { get; }
        public int Def { get; }
        public int Hp { get; }

        public bool IsPurchased { get; set; } = false;
        public bool IsEquipped { get; set; } = false;

        public Item(string name, string desc, ItemType type, int price, int atk, int def, int hp)
        {
            Name = name;
            Desc = desc;
            Type = type;
            Price = price;
            Atk = atk;
            Def = def;
            Hp = hp;
            SellPrice = (int)(price * 0.85f);
        }

        public virtual void PrintStat()
        {
            Console.Write(" | ");
            if (Atk != 0) Console.Write(ConsoleUtility.PadRightForMixedText($"공격력 {(Atk > 0 ? "+" : "")}{Atk} ", 13));
            if (Def != 0) Console.Write(ConsoleUtility.PadRightForMixedText($"방어력 {(Def > 0 ? "+" : "")}{Def} ", 13));
            if (Hp != 0) Console.Write(ConsoleUtility.PadRightForMixedText($"체  력 {(Hp > 0 ? "+" : "")}{Hp} ", 13));
            Console.Write("| ");
        }

        public void PrintController(MenuType menuType, int idx)
        {
            Console.Write("- ");

            switch (menuType)
            {
                case MenuType.InventoryMenu:
                    PrintIncludeEquipInfo();
                    break;
                case MenuType.EquipMenu:
                    PrintIncludeEquipInfo(true, idx, false);
                    break;
                case MenuType.StoreMenu:
                    PrintExcludeEquipInfo(menuType);
                    break;
                case MenuType.PurchaseMenu:
                    PrintExcludeEquipInfo(menuType, true, idx);
                    break;
                case MenuType.SellMenu:
                    PrintIncludeEquipInfo(true, idx, true);
                    break;
            }
        }

        public void PrintIncludeEquipInfo(bool withNumber = false, int idx = 0, bool hasPriceInfo = false)
        {
            if (withNumber)
            {
                string indexStr = idx < 10 ? $"{idx}  " : $"{idx} ";
                ConsoleUtility.PrintTextHighlightsColor(ConsoleColor.DarkMagenta, "", indexStr);
            }

            if (IsEquipped)
            {
                ConsoleUtility.PrintTextHighlightsColor(ConsoleColor.Cyan, "[", "E", "]");
                Console.Write(ConsoleUtility.PadRightForMixedText(Name, 17));
            }
            else Console.Write(ConsoleUtility.PadRightForMixedText(Name, 20));

            PrintStat();

            if (hasPriceInfo)
            {
                Console.Write(ConsoleUtility.PadRightForMixedText(Desc, 50));
                Console.Write(" | ");

                PrintPrice(MenuType.SellMenu); //착용 정보까지 보여주는 것중에 가격을 보여주는 것은 판매메뉴 밖에 없음
                Console.WriteLine("");
            }
            else
            {
                Console.WriteLine(Desc);
            }
        }

        public void PrintExcludeEquipInfo(MenuType menuType, bool withNumber = false, int idx = 0)
        {
            if (withNumber)
            {
                string indexStr = idx < 10 ? $"{idx}  " : $"{idx} ";
                ConsoleUtility.PrintTextHighlightsColor(ConsoleColor.DarkMagenta, "", indexStr);
                Console.Write(ConsoleUtility.PadRightForMixedText(Name, 15));
            }
            else Console.Write(ConsoleUtility.PadRightForMixedText(Name, 15));

            PrintStat();
            Console.Write(ConsoleUtility.PadRightForMixedText(Desc, 50));
            Console.Write(" | ");
            PrintPrice(menuType);
            Console.WriteLine("");
        }

        public void PrintPrice(MenuType menuType)
        {
            switch (menuType)
            {
                case MenuType.StoreMenu:
                case MenuType.PurchaseMenu:
                    if (IsPurchased) ConsoleUtility.PrintTextHighlightsColor(ConsoleColor.Green, "", "구매완료");
                    else ConsoleUtility.PrintTextHighlightsColor(ConsoleColor.Yellow, "", Price.ToString(), " G");
                    break;
                case MenuType.SellMenu:
                    ConsoleColor color = IsEquipped ? ConsoleColor.Red : ConsoleColor.Yellow;
                    ConsoleUtility.PrintTextHighlightsColor(color, "", SellPrice.ToString(), " G");
                    break;
            }

        }

        public void ToggleEquipStatus()
        {
            IsEquipped = !IsEquipped;
        }

        public void Purchase()
        {
            IsPurchased = true;
        }

        public void Sell()
        {
            IsPurchased = false;
        }
    }
}
