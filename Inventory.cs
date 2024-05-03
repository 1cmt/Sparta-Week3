
namespace TextGame
{
    public class Inventory
    {
        public List<Item> ItemList { get; } = new List<Item>();
        public int BonusAtk { get; private set; } = 0;
        public int BonusDef { get; private set; } = 0;
        public int BonusHp { get; private set; } = 0;

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
            ItemList[idx].Sell();
            ItemList.RemoveAt(idx);
        }

        public bool IsItemEquipped(int idx)
        {
            return ItemList[idx].IsEquipped;
        }

        public Item GetItem(int idx)
        {
            return ItemList[idx];
        }

        public ItemType GetItemType(int idx)
        {
            return ItemList[idx].Type;
        }

        public void ToggleItemEquipStatus(int idx)
        {
            ItemList[idx].ToggleEquipStatus();
        }

        public void ManageBonusValue(bool isAdd, int idx)
        {
            Item item = ItemList[idx];

            if (isAdd)
            {
                if (item.Atk != 0) BonusAtk += item.Atk;
                if (item.Def != 0) BonusDef += item.Def;
                if (item.Hp != 0) BonusHp += item.Hp;
            }
            else
            {
                if (item.Atk != 0) BonusAtk -= item.Atk;
                if (item.Def != 0) BonusDef -= item.Def;
                if (item.Hp != 0) BonusHp -= item.Hp;
            }
        }

        public void ManageBonusValue(bool isAdd, Item item)
        {
            if (isAdd)
            {
                if (item.Atk != 0) BonusAtk += item.Atk;
                if (item.Def != 0) BonusDef += item.Def;
                if (item.Hp != 0) BonusHp += item.Hp;
            }
            else
            {
                if (item.Atk != 0) BonusAtk -= item.Atk;
                if (item.Def != 0) BonusDef -= item.Def;
                if (item.Hp != 0) BonusHp -= item.Hp;
            }
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

            switch (ConsoleUtility.PromptMenuChoice(0, 2))
            {
                case 0:
                    return;
                case 1:
                    EquipMenu(player);
                    break;
            }
        }

        public void EquipMenu(Player player, string? prompt = null)
        {
            if (prompt != null)
            {
                Console.Clear();
                ConsoleUtility.PrintTitle(prompt);
                Thread.Sleep(1000);
            }

            Console.Clear();

            ConsoleUtility.PrintTitle("■ 인벤토리 - 장착 관리 ■");
            Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다");
            Console.WriteLine("");
            Console.WriteLine("[아이템 목록]");
            printInventory(player);
            PrintMyItemList(MenuType.EquipMenu);
            Console.WriteLine("0. 나가기");
            Console.WriteLine("");

            int KeyInput = ConsoleUtility.PromptMenuChoice(0, ItemList.Count);

            switch (KeyInput)
            {
                case 0:
                    InventoryMenu(player);
                    break;
                default:
                    if (IsItemEquipped(KeyInput - 1)) //아이템이 이미 장착 중이라면
                    {
                        //이미 장착중인 아이템이라면? 플레이어가 가진 장착 슬롯서 제거/장착 해제 상태로 아이템 변경/아이템에 달려있던 보너스 능력치 제거
                        player.EquipItems[(int)GetItemType(KeyInput - 1)] = null;
                        ToggleItemEquipStatus(KeyInput - 1);
                        ManageBonusValue(false, KeyInput - 1);
                        EquipMenu(player);
                        break;
                    }
                    else //아이템을 장착하고 있지 않다면
                    {
                        //장착하고자 하는 부위에 다른 아이템이 있다면? 장착되어있던 아이템의 상태를 장착 해제로 변경/기존 아이템에 달려있는 능력치는 제거
                        //굳이 플레이어의 장착 아이템 배열에 접근해 null로 바꾸지 않는 이유는 어차피 아래서 현재 장착하고자 하는 아이템을 대입시켜 참조할 것이기에 안넣음
                        if (player.EquipItems[(int)GetItemType(KeyInput - 1)] != null)
                        {
                            Item equipItem = player.EquipItems[(int)GetItemType(KeyInput - 1)]!;
                            ManageBonusValue(false, equipItem);
                            equipItem.ToggleEquipStatus();
                        }

                        //아이템 장착: 플레이어가 가진 장착 슬롯에 추가/장착 해제 아이템 상태로 변경/아이템에 달려있던 보너스 능력치 제거
                        player.EquipItems[(int)GetItemType(KeyInput - 1)] = GetItem(KeyInput - 1);
                        ToggleItemEquipStatus(KeyInput - 1);
                        ManageBonusValue(true, KeyInput - 1);

                        EquipMenu(player);
                        break;
                    }
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

        public void printInventory(Player player)
        {
            //Pad 함수에 대한 이해를 위해 제작
            ////박스 가로 길이를 12로 잡고 만일 부위에 아이템이 있다면 가운데 정렬, 없다면 그냥 공백 12칸

            string weaponName = player.EquipItems[(int)ItemType.Weapon] != null ? ConsoleUtility.PadCenterForMixedText(player.EquipItems[(int)ItemType.Weapon]!.Name, 12) : new string(' ', 12);
            string armorName = player.EquipItems[(int)ItemType.Armor] != null ? ConsoleUtility.PadCenterForMixedText(player.EquipItems[(int)ItemType.Armor]!.Name, 12) : new string(' ', 12);
            string helemtName = player.EquipItems[(int)ItemType.Helmet] != null ? ConsoleUtility.PadCenterForMixedText(player.EquipItems[(int)ItemType.Helmet]!.Name, 12) : new string(' ', 12);
            string shieldName = player.EquipItems[(int)ItemType.Shield] != null ? ConsoleUtility.PadCenterForMixedText(player.EquipItems[(int)ItemType.Shield]!.Name, 12) : new string(' ', 12);
            
            Console.WriteLine("");
            ConsoleUtility.PrintLongLine('■');
            Console.WriteLine("");
            Console.WriteLine("                                                                         .=**++**+.                                                                     ");
            Console.WriteLine("                                             @============@             **:      .*#.                                         =                         ");
            Console.WriteLine("                                             @            @           .%-          :%.                                       #%*                        ");
            Console.WriteLine("                                             @    투 구   @           *+            =#                                      #+ +#                       ");
            Console.WriteLine("                                             @            @===========%#=======     :%                                     *%***@*                      ");
            Console.WriteLine("                                             @{0}@           =%            *+                                     ++   ++                      ", helemtName);
            Console.WriteLine("                                             @            @            +#.         **                                      ++   ++        @============@");
            Console.WriteLine("                                             @============@             :#*-.  .-+#-                                       ++   ++        @            @");
            Console.WriteLine("                            .-=++++++-.                            .#%++++++++++++++++*@:                                  ++   ++        @   무  기   @");
            Console.WriteLine("                          =#+:::-=--::+#+                         +##*                :@+#:                                ++ ==%%========@            @");
            Console.WriteLine("  @=============@       -%-.+#+-:.:-=**.-#=                     +#- +*                :@ .**.                              ++   ++        @{0}@", weaponName);
            Console.WriteLine("  @             @      +# +#:         .*+ **                  =#-   +*                :@   :**.                            ++   ++        @            @");
            Console.WriteLine("  @    방  패   @     :@ +#             ** %-                 -**=: +*                :@ .-**=.    @=============@         ++   ++        @============@");
            Console.WriteLine("  @             @=====#%=@*=======      .@ +*                    .=*%*                :@*+-        @             @         ++   ++");
            Console.WriteLine("  @{0} @     +#.%=             :@ +*                       +*                :@           @    갑  옷   @         **...**", shieldName);
            Console.WriteLine("  @             @     .@::@.            %- @.                       +*       ==========@===========@             @       #=========#");
            Console.WriteLine("  @=============@      :%::#=         =#:.%-                        +*                :@           @             @       #=========#");
            Console.WriteLine("                        .**::+*++=++++-.+#:                         +*                :@           @{0} @          =# %=", armorName);
            Console.WriteLine("                          .=**=:::::-+*+.                           +*                :@           @             @          =# %-");
            Console.WriteLine("                              :--=--:                               +*                :@           @=============@          =% %-");
            Console.WriteLine("                                                                    +*                :@                                     :::");
            Console.WriteLine("                                                                    +#:::::::::::::::::@");
            Console.WriteLine("");
            ConsoleUtility.PrintLongLine('■');
            Console.WriteLine("");
        }
    }
}
