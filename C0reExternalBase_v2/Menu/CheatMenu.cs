using System.Drawing;
using C0reExternalBase_v2.Structs;
using static C0reExternalBase_v2.Utility.Hotkeys;
using static C0reExternalBase_v2.Structs.CheatMenu;
using static C0reExternalBase_v2.Structs.CheatMenu.Menu;
using static C0reExternalBase_v2.Structs.CheatMenu.Item;

namespace C0reExternalBase_v2.Menu
{
    class CheatMenu
    {
        public static void InitializeMenu()
        {
            // Define Item Names Here
            string[] ItemNames = new string[] {
                "Box", "Snaplines", "BunnyHop"
            };

            // Get The Total Number Of Items
            m_iNumberOfItems = ItemNames.Length;

            // Iterate Through All Items And Set Their Names
            for (int i = 0; i < m_iNumberOfItems; i++)
            {
                if (i == 0)
                    Arrays.ItemList[i].m_bIsHovered = true;

                Arrays.ItemList[i].m_sName = ItemNames[i];
            }
        }

        public static void MenuRun(RenderHelper Render)
        {
            RenderMenu(Render);
            Navigation();

            // Set Setting Vars
            Settings.m_bESP = Arrays.ItemList[0].m_bIsActive;
            Settings.m_bSnaplines = Arrays.ItemList[1].m_bIsActive;
            Settings.m_bBunnyhop = Arrays.ItemList[2].m_bIsActive;
        }

        private static void RenderMenu(RenderHelper Render)
        {
            // Set Menu Dimensions
            m_iItemHeight = 20;
            m_Position = new Vector2D(50, 450);
            m_Size = new Vector2D(250, m_iNumberOfItems * m_iItemHeight + 20);

            // Draw Menu Background
            Render.DrawText("C0reBase | External v2", true, m_Position.x + m_Size.x / 2, m_Position.y - 30, Color.LawnGreen, 3);
            Render.DrawFilledBox(m_Position.x, m_Position.y, m_Size.x, m_Size.y, Color.FromArgb(255, 27, 27, 27));
            Render.DrawBox(m_Position.x, m_Position.y, m_Size.x, m_Size.y, 1, Color.Black);

            // Iterate Through All Menu Items
            for (int i = 0; i < m_iNumberOfItems; i++)
            {
                Item CurrentItem = Arrays.ItemList[i];

                Color ColorText;
                if (CurrentItem.m_bIsActive)
                    ColorText = Color.LawnGreen;
                else
                    ColorText = Color.OrangeRed;

                Color ColorBackground;
                if (CurrentItem.m_bIsHovered)
                    ColorBackground = Color.FromArgb(255, 66, 66, 66);
                else
                    ColorBackground = Color.FromArgb(255, 27, 27, 27);

                // Draw The Item
                Render.DrawFilledBox(m_Position.x + 10, m_Position.y + (i * m_iItemHeight) + 10, m_Size.x - 20, m_iItemHeight, ColorBackground);
                Render.DrawText(CurrentItem.m_sName, false, m_Position.x + 20, m_Position.y + (i * m_iItemHeight) + 11, ColorText, 1);
                Render.DrawBox(m_Position.x + 10, m_Position.y + (i * m_iItemHeight) + 10, m_Size.x - 20, m_iItemHeight, 1, Color.Black);
            }
        }

        private static void Navigation()
        {
            // Navigation Bullshit (Yep Its Ugly AF I Was Lazy Here ;p )

            if (KEY_ARROW_UP_STATE && m_LastPressedNavigationKey != "ArrowUp")
            {
                if (m_iSelector - 1 >= 0)
                {
                    m_iSelector -= 1;
                    Arrays.ItemList[m_iSelector].m_bIsHovered = true;
                }
                m_LastPressedNavigationKey = "ArrowUp";
            }
            else if (!KEY_ARROW_UP_STATE && m_LastPressedNavigationKey == "ArrowUp")
                m_LastPressedNavigationKey = "NULL";

            if (KEY_ARROW_DOWN_STATE && m_LastPressedNavigationKey != "ArrowDown") {
                if (m_iSelector + 1 < m_iNumberOfItems) {
                    m_iSelector += 1;
                    Arrays.ItemList[m_iSelector].m_bIsHovered = true;
                }
                m_LastPressedNavigationKey = "ArrowDown";
            }
            else if (!KEY_ARROW_DOWN_STATE && m_LastPressedNavigationKey == "ArrowDown")
                m_LastPressedNavigationKey = "NULL";

            if (KEY_ARROW_LEFT_STATE)
                Arrays.ItemList[m_iSelector].m_bIsActive = false;

            if (KEY_ARROW_RIGHT_STATE)
                Arrays.ItemList[m_iSelector].m_bIsActive = true;

            // Set All Other Items To NotHovered (Todo: FIX THIS!!!)
            for (int i = 0; i < m_iNumberOfItems; i++)
            {
                if (i != m_iSelector)
                    Arrays.ItemList[i].m_bIsHovered = false;
            }
        }
    }
}
