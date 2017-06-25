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
        public static Item[] Items = new Item[] {
            new Item { m_sName = "ESP", m_sControlType = "Switch", m_bIsActive = false, m_bIsHovered = true},
            new Item { m_sName = "Engine Chams", m_sControlType = "Switch", m_bIsActive = false, m_bIsHovered = false},
            new Item { m_sName = "Snaplines", m_sControlType = "Switch", m_bIsActive = false, m_bIsHovered = false},
            new Item { m_sName = "Radar", m_sControlType = "Switch", m_bIsActive = false, m_bIsHovered = false},
            new Item { m_sName = "BunnyHop", m_sControlType = "Switch", m_bIsActive = false, m_bIsHovered = false},
            new Item { m_sName = "TriggerBot", m_sControlType = "Switch", m_bIsActive = false, m_bIsHovered = false},
            new Item { m_sName = "Skinchanger", m_sControlType = "Switch", m_bIsActive = false, m_bIsHovered = false},
        };

        public static void InitializeMenu()
        {
            // Get The Total Number Of Items
            m_iNumberOfItems = Items.Length;
        }

        public static void MenuRun(Renderer Render)
        {
            RenderMenu(Render);
            Navigation();

            // Set Setting Vars
            Settings.m_bESP = Items[0].m_bIsActive;
            Settings.m_bEngineChams = Items[1].m_bIsActive;
            Settings.m_bSnaplines = Items[2].m_bIsActive;
            Settings.m_bRadar = Items[3].m_bIsActive;
            Settings.m_bBunnyhop = Items[4].m_bIsActive;
            Settings.m_bTriggerbot = Items[5].m_bIsActive;
            Settings.m_bSkinchanger = Items[6].m_bIsActive;
        }

        private static void RenderMenu(Renderer Render)
        {
            // Set Menu Dimensions
            m_iItemHeight = 20;
            m_Position = new Vector2D(20, 450);
            m_Size = new Vector2D(300, m_iNumberOfItems * m_iItemHeight + 20);

            // Draw Menu Background
            Render.DrawFilledBox(m_Position.x, m_Position.y - 30, m_Size.x, 30, Color.FromArgb(255, 27, 27, 27));
            Render.DrawText("C0reBase | External v2.4", true, m_Position.x + m_Size.x / 2, m_Position.y - 25, Color.White, 3);
            Render.DrawFilledBox(m_Position.x, m_Position.y, m_Size.x, m_Size.y, Color.FromArgb(255, 27, 27, 27));
            Render.DrawBox(m_Position.x, m_Position.y - 30, m_Size.x, m_Size.y + 30, 1, Color.Black);

            // Iterate Through All Menu Items
            for (int i = 0; i < m_iNumberOfItems; i++)
            {
                Item CurrentItem = Items[i];

                Color ColorText;
                if (CurrentItem.m_bIsActive)
                    ColorText = Color.LawnGreen;
                else
                    ColorText = Color.Red;

                Color ColorBackground;
                if (CurrentItem.m_bIsHovered)
                    ColorBackground = Color.FromArgb(255, 66, 66, 66);
                else
                    ColorBackground = Color.FromArgb(255, 27, 27, 27);

                // Draw The Item
                Render.DrawFilledBox(m_Position.x + 10, m_Position.y + (i * m_iItemHeight) + 10, m_Size.x - 20, m_iItemHeight, ColorBackground);
                Render.DrawFilledBox(m_Size.x - 15, m_Position.y + (i * m_iItemHeight) + 15, 7, 7, ColorText); //btn
                Render.DrawText(CurrentItem.m_sName, false, m_Position.x + 20, m_Position.y + (i * m_iItemHeight) + 11, Color.Gold, 1);
                Render.DrawBox(m_Position.x + 10, m_Position.y + (i * m_iItemHeight) + 10, m_Size.x - 20, m_iItemHeight, 1, Color.Black);
            }
        }

        private static void Navigation()
        {
            // Navigation Bullshit (Yep Its Ugly AF I Was Lazy Here ;p )

            if (KEY_ARROW_UP_STATE && m_LastPressedNavigationKey != KeyCodeConstants.ARROW_UP)
            {
                if (m_iSelector - 1 >= 0)
                {
                    m_iSelector -= 1;
                    Items[m_iSelector].m_bIsHovered = true;
                }
                m_LastPressedNavigationKey = KeyCodeConstants.ARROW_UP;
            }
            else if (!KEY_ARROW_UP_STATE && m_LastPressedNavigationKey == KeyCodeConstants.ARROW_UP)
            {
                m_LastPressedNavigationKey = KeyCodeConstants.NULL;
            }

            if (KEY_ARROW_DOWN_STATE && m_LastPressedNavigationKey != KeyCodeConstants.ARROW_DOWN)
            {
                if (m_iSelector + 1 < m_iNumberOfItems)
                {
                    m_iSelector += 1;
                    Items[m_iSelector].m_bIsHovered = true;
                }
                m_LastPressedNavigationKey = KeyCodeConstants.ARROW_DOWN;
            }
            else if (!KEY_ARROW_DOWN_STATE && m_LastPressedNavigationKey == KeyCodeConstants.ARROW_DOWN)
            {
                m_LastPressedNavigationKey = KeyCodeConstants.NULL;
            }

            if (KEY_ARROW_LEFT_STATE)
            {
                if (Items[m_iSelector].m_sControlType == "Switch")
                {
                    Items[m_iSelector].m_bIsActive = false;
                }
                else if (Items[m_iSelector].m_sControlType == "Select")
                {
                    // TODO
                }
            }

            if (KEY_ARROW_RIGHT_STATE)
            {
                if (Items[m_iSelector].m_sControlType == "Switch")
                {
                    Items[m_iSelector].m_bIsActive = true;
                }
                else if (Items[m_iSelector].m_sControlType == "Select")
                {
                    // TODO
                }
            }

            // Set All Other Items To NotHovered (Todo: FIX THIS!!!)
            for (int i = 0; i < m_iNumberOfItems; i++)
            {
                if (i != m_iSelector)
                    Items[i].m_bIsHovered = false;
            }
        }
    }
}
