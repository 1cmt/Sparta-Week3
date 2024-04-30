using System.Security.Cryptography.X509Certificates;
using Sparta_week3;

enum MenuType
{
    None,
    StoreMenu,
    PurchaseMenu,
    SellMenu,
    InventoryMenu,
    EquipMenu
}

internal class Item
{
    public string Name { get; }
    public string Desc { get; }
    public int Price { get; }
    public int SellPrice { get; }

    public bool IsPurchased { get; private set; } = false;
    public bool IsEquipped { get; private set; } = false;

    public Item(string name, string desc, int price)
    {
        Name = name;
        Desc = desc;
        Price = price;
        SellPrice = (int)(price * 0.85f);
    }

    public virtual void PrintStat() { }

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
                PrintExcludeEquipInfo(menuType);
                break;
            case MenuType.SellMenu:
                PrintIncludeEquipInfo(true, idx, true);
                break;
        }
    }

    public void PrintIncludeEquipInfo(bool withNumber = false, int idx = 0, bool hasPriceInfo = false)
    {
        if (withNumber) ConsoleUtility.PrintTextHighlightsColor(ConsoleColor.DarkMagenta, "", $"{idx} ");

        if (IsEquipped)
        {
            ConsoleUtility.PrintTextHighlightsColor(ConsoleColor.Cyan, "[", "E", "]");
            Console.Write(ConsoleUtility.PadRightForMixedText(Name, 9));
        }
        else Console.Write(ConsoleUtility.PadRightForMixedText(Name, 12));

        PrintStat();

        if (hasPriceInfo)
        {
            Console.Write(ConsoleUtility.PadRightForMixedText(Desc, 12));
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
            ConsoleUtility.PrintTextHighlightsColor(ConsoleColor.DarkMagenta, "", $"{idx} ");
            Console.Write(ConsoleUtility.PadRightForMixedText(Name, 10));
        }
        else Console.Write(ConsoleUtility.PadRightForMixedText(Name, 12));

        PrintStat();
        Console.Write(ConsoleUtility.PadRightForMixedText(Desc, 12));
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

    internal void ToggleEquipStatus()
    {
        IsEquipped = !IsEquipped;
    }

    internal void Purchase()
    {
        IsPurchased = true;
    }

    internal void Sell()
    {
        IsPurchased = false;
    }
}

internal class AtkItem : Item
{
    public int Atk { get; }

    public AtkItem(string name, string info, int price, int atk) : base(name, info, price)
    {
        Atk = atk;
    }

    public override void PrintStat()
    {
        Console.Write($" | 공격력 +{Atk}  | ");
    }
}

internal class DefItem : Item
{
    public int Def { get; }

    public DefItem(string name, string info, int price, int def) : base(name, info, price)
    {
        Def = def;
    }

    public override void PrintStat()
    {
        Console.Write($" | 공격력 +{Def}  | ");
    }
}

