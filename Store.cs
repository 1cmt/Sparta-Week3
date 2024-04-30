using System.Runtime.InteropServices.Marshalling;
using Sparta_week3;

namespace Sparta_week3
{
    internal class Store
    {
        public Item[] StoreInventory { get; }

        public Store()
        {
            StoreInventory = new Item[8];
            StoreInventory[0] = new DefItem("수련자 갑옷", "수련에 도움을 주는 갑옷입니다.", 1000, 5);
            StoreInventory[1] = new DefItem("무쇠 갑옷", "무쇠로 만들어져 튼튼한 갑옷입니다.", 2000, 9);
            StoreInventory[2] = new DefItem("스파르타의 갑옷", "스파르타의 전사들이 사용했다는 전설의 갑옷입니다.", 3500, 15);
            StoreInventory[3] = new DefItem("부실한 갑옷", "천으로 만들어진 부실한 갑옷입니다.", 300, 2);
            StoreInventory[4] = new AtkItem("낡은 검", "쉽게 볼 수 있는 낡은 검 입니다.", 600, 2);
            StoreInventory[5] = new AtkItem("청동 도끼", "어디선가 사용됐던거 같은 도끼입니다.", 1500, 5);
            StoreInventory[6] = new AtkItem("스파르타의 창", "스파르타의 전사들이 사용했다는 전설의 창입니다.", 2000, 7);
            StoreInventory[7] = new AtkItem("부실한 창", "낡아보이는 부실한 창입니다.", 200, 1);
        }

        public void StoreMenu(Player player, Inventory inventory)
        {
            Console.Clear();

            ConsoleUtility.PrintTitle("■ 상점 ■");
            PrintCommonText(player);    
            PrintStoreItemInfo(MenuType.StoreMenu, player);

            Console.WriteLine("");
            Console.WriteLine("1. 아이템 구매");
            Console.WriteLine("2. 아이템 판매");
            Console.WriteLine("0. 나가기");
            Console.WriteLine("");

            switch (ConsoleUtility.PromptMenuChoice(0, 2))
            {
                case 0:
                    return;
                case 1:
                    PurchaseMenu(player, inventory);
                    break;
                case 2:
                    SellMenu(player, inventory);
                    break;
            }
        }

        public void PurchaseMenu(Player player, Inventory inventory, string? prompt = null)
        {
            if (prompt != null)
            {
                Console.Clear();
                ConsoleUtility.PrintTitle(prompt);
                Thread.Sleep(1000);
            }

            Console.Clear();

            ConsoleUtility.PrintTitle("■ 상점 - 아이템 구매 ■");
            PrintCommonText(player);
            PrintStoreItemInfo(MenuType.PurchaseMenu, player);

            Console.WriteLine("");
            Console.WriteLine("0. 나가기");
            Console.WriteLine("");

            int keyInput = ConsoleUtility.PromptMenuChoice(0, StoreInventory.Length);
            switch (keyInput)
            {
                case 0:
                    StoreMenu(player, inventory);
                    break;
                default:
                    // 1 : 이미 구매한 경우
                    if (StoreInventory[keyInput - 1].IsPurchased) // index 맞추기
                    {
                        PurchaseMenu(player, inventory, "이미 구매한 아이템입니다.");
                    }
                    // 2 : 돈이 충분해서 살 수 있는 경우
                    else if (player.Gold >= StoreInventory[keyInput - 1].Price)
                    {
                        player.Gold -= StoreInventory[keyInput - 1].Price;
                        StoreInventory[keyInput - 1].Purchase();
                        inventory.AddItem(StoreInventory[keyInput - 1]);
                        PurchaseMenu(player, inventory);
                    }
                    // 3 : 돈이 모자라는 경우
                    else
                    {
                        PurchaseMenu(player, inventory, "Gold가 부족합니다.");
                    }
                    break;
            }
        }

        public void SellMenu(Player player, Inventory inventory, string? prompt = null)
        {
            if (prompt != null)
            {
                Console.Clear();
                ConsoleUtility.PrintTitle(prompt);
                Thread.Sleep(1000);
            }

            Console.Clear();

            ConsoleUtility.PrintTitle("■ 상점 - 아이템 판매 ■");
            PrintCommonText(player);
            inventory.PrintMyItemList(MenuType.SellMenu);

            Console.WriteLine("");
            Console.WriteLine("0. 나가기");
            Console.WriteLine("");

            int keyInput = ConsoleUtility.PromptMenuChoice(0, inventory.Count());

            switch (keyInput)
            {
                case 0:
                    StoreMenu(player, inventory);
                    break;
                default:
                    // 1 : 장착한 경우
                    if (inventory.IsEquipped(keyInput - 1))
                    {
                        Console.WriteLine("장착 중인 아이템입니다. 그래도 판매하시겠습니까? [0: 아니요, 1: 네]");
                        Console.Write(">> ");
                        switch (ConsoleUtility.PromptMenuChoice(0, 1))
                        {
                            case 0:
                                SellMenu(player, inventory);
                                break;
                            case 1:
                                inventory.ToggleEquipStatus(keyInput - 1);
                                inventory.RemoveItem(keyInput - 1);
                                break;
                        }
                    }
                    // 2 : 판매가 가능한 경우
                    else
                    {
                        player.Gold += inventory.GetSellPrice(keyInput - 1);
                        StoreInventory[keyInput - 1].Sell();
                        inventory.RemoveItem(keyInput - 1);
                        SellMenu(player, inventory);
                    }
                    break;
            }
        }

        public void PrintCommonText(Player player)
        {
            Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.");
            Console.WriteLine("");
            Console.WriteLine("[보유 골드]");
            ConsoleUtility.PrintTextHighlightsColor(ConsoleColor.Yellow, "", player.Gold.ToString(), " G");
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("[아이템 목록]");
        }

        public void PrintStoreItemInfo(MenuType menuType, Player player)
        {
            for (int i = 0; i < StoreInventory.Length; i++)
            {
                StoreInventory[i].PrintController(menuType, i + 1);
            }

            Console.WriteLine("");
        }
    }

}