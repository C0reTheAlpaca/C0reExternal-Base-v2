using static C0reExternalBase_v2.Utility.Hotkeys;

namespace C0reExternalBase_v2.Structs
{
    class CheatMenu
    {
        public struct Menu
        {
            public static int m_iItemHeight;
            public static int m_iSelector = 0;
            public static int m_iNumberOfItems;
            public static Vector2D m_Position;
            public static Vector2D m_Size;
            public static KeyCodeConstants m_LastPressedNavigationKey;
        };

        public struct Item
        {
            public string m_sName;
            public string m_sControlType;
            public bool m_bIsActive;
            public bool m_bIsHovered;
            public Select[] ItemList;
        };

        public struct Select
        {
            public string m_sName;
            public int m_iValue;
        };

        public struct Settings
        {
            public static bool m_bESP;
            public static bool m_bSnaplines;
            public static bool m_bRadar;
            public static bool m_bBunnyhop;
            public static bool m_bTriggerbot;
            public static bool m_bSkinchanger;

            internal class Radar
            {
                public static int m_iRadarSize = 300;
                public static Vector2D m_VecRadarPosition = new Vector2D(20, 20);
            }
        };
    }
}
