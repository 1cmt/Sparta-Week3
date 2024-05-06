using System.Runtime.InteropServices.Marshalling;

namespace TextGame
{
    [Serializable]
    public class Store
    {
        public Item[] StoreInventory { get; set; }
        
        public Store()
        {
            StoreInventory = new Item[16];
            StoreInventory[0] = new Item("부실한 창", "낡아보이는 부실한 창입니다.", ItemType.Weapon, 200, 1, 0, 0);
            StoreInventory[1] = new Item("낡은 검", "쉽게 볼 수 있는 낡은 검 입니다.", ItemType.Weapon, 600, 2, 0, 0);
            StoreInventory[2] = new Item("청동 도끼", "어디선가 사용됐던거 같은 도끼입니다.", ItemType.Weapon, 1500, 5, 0, 0);
            StoreInventory[3] = new Item("르탄의 창", "위대한 장군인 르탄이 사용했다는 창입니다.", ItemType.Weapon, 2000, 7, 0, 0);
            StoreInventory[4] = new Item("부실한 갑옷", "천으로 만들어진 부실한 갑옷입니다.", ItemType.Armor, 500, 0, 3, 0);
            StoreInventory[5] = new Item("수련자 갑옷", "수련에 도움을 주는 갑옷입니다.", ItemType.Armor, 1000, 0, 5, 0);
            StoreInventory[6] = new Item("무쇠 갑옷", "무쇠로 만들어져 튼튼한 갑옷입니다.", ItemType.Armor, 2000, 0, 9, 0);
            StoreInventory[7] = new Item("르탄의 갑옷", "위대한 장군인 르탄이 사용했다는 갑옷입니다.", ItemType.Armor, 3500, 0, 15, 0);
            StoreInventory[8] = new Item("부실한 투구", "강도가 약한 부실한 투구입니다.", ItemType.Helmet, 300, 0, 1, 0);
            StoreInventory[9] = new Item("낡은 투구", "오래되어 내구성이 손상된 투구입니다.", ItemType.Helmet, 600, 0, 3, 0);
            StoreInventory[10] = new Item("무쇠 투구", "무쇠로 만들어져 튼튼한 투구입니다.", ItemType.Helmet, 1500, 0, 7, 0);
            StoreInventory[11] = new Item("르탄의 투구", "위대한 장군인 르탄이 사용했다는 투구입니다.", ItemType.Helmet, 2500, 0, 10, 0);
            StoreInventory[12] = new Item("부실한 방패", "툭하면 부서질 거 같은 방패입니다.", ItemType.Shield, 400, 0, 2, 0);
            StoreInventory[13] = new Item("낡은 방패", "쉽게 볼 수 있는 낡은 검입니다.", ItemType.Shield, 800, 0, 4, 0);
            StoreInventory[14] = new Item("무쇠 방패", "무쇠로 만들어져 튼튼한 방패입니다.", ItemType.Shield, 1800, 0, 8, 0);
            StoreInventory[15] = new Item("르탄의 방패", "위대한 장군인 르탄이 사용했다는 전설의 방패입니다.", ItemType.Shield, 2800, 0, 12, 0);
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
                    if (inventory.IsItemEquipped(keyInput - 1))
                    {
                        Console.WriteLine("장착 중인 아이템입니다. 그래도 판매하시겠습니까? [0: 아니요, 1: 네]");
                        Console.Write("");
                        switch (ConsoleUtility.PromptMenuChoice(0, 1))
                        {
                            case 0:
                                SellMenu(player, inventory);
                                break;
                            case 1:
                                //장착 중인 아이템 판매 : 판매금 주기/ 플레이어가 보유한 장착 아이템 슬롯 비워주기 / 장착 해제로 아이템 상태 변경/ 보너스 능력치 삭제
                                //판매이기에 인벤토리에서도 삭제 후 다시 판매메뉴를 불러온다.
                                player.Gold += inventory.GetSellPrice(keyInput - 1);
                                player.EquipItems[(int)inventory.GetItemType(keyInput - 1)] = null;
                                inventory.ToggleItemEquipStatus(keyInput - 1);
                                inventory.ManageBonusValue(false, keyInput - 1);
                                inventory.RemoveItem(keyInput - 1);
                                StoreInventory[keyInput - 1].Purchase();
                                SellMenu(player, inventory);
                                break;
                        }
                    }
                    // 2 : 판매가 가능한 경우
                    else
                    {
                        player.Gold += inventory.GetSellPrice(keyInput - 1);
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