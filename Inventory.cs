
namespace Sparta_week3
{
    internal class Inventory
    {
        public List<Item> ItemList { get; set; }

        public Inventory()
        {
            ItemList = new List<Item>();
        }

        public int Count()
        {
            return ItemList.Count;
        }

        public void AddItem(Item item)
        {
            ItemList.Add(item);
        }

        public void RemoveItem(int idx)
        {
            ItemList.RemoveAt(idx);
        }

        public bool IsEquipped(int idx)
        {
            return ItemList[idx].IsEquipped;
        }

        public void ToggleEquipStatus(int idx)
        {
            ItemList[idx].ToggleEquipStatus();
        }

        public int GetSellPrice(int idx)
        {
            return ItemList[idx].SellPrice;
        }

        public void InventoryMenu(Player player)
        {
            Console.Clear();

            ConsoleUtility.PrintTitle("■ 인벤토리 ■");
            Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다");
            Console.WriteLine("");
            Console.WriteLine("[아이템 목록]");
            PrintMyItemList(MenuType.InventoryMenu);
            Console.WriteLine("1. 장착관리");
            Console.WriteLine("0. 나가기");
            Console.WriteLine("");

            switch (ConsoleUtility.PromptMenuChoice(0, 1))
            {
                case 0:
                    return;
                case 1:
                    
                    EquipMenu(player);
                    break;
            }
        }

        public void EquipMenu(Player player)
        {
            Console.Clear();

            ConsoleUtility.PrintTitle("■ 인벤토리 - 장착 관리 ■");
            Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다");
            Console.WriteLine("");
            Console.WriteLine("[아이템 목록]");
            PrintMyItemList(MenuType.EquipMenu);
            Console.WriteLine("0. 나가기");
            Console.WriteLine("");
            
            int KeyInput = ConsoleUtility.PromptMenuChoice(0, ItemList.Count);

            switch (KeyInput)
            {
                case 0:
                    InventoryMenu(player);
                    break;
                case 1:
                    ToggleEquipStatus(KeyInput - 1);
                    EquipMenu(player);
                    break;
            }
        }

        public void PrintMyItemList(MenuType menuType)
        {
            for (int i = 0; i < ItemList.Count; i++)
            {
                ItemList[i].PrintController(menuType, i + 1);
            }

            Console.WriteLine("");
        }
    }
}
