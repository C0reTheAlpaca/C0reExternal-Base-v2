using System.Windows.Forms;

namespace C0reExternalBase_v2.Structs
{
    class CheatMenu
    {
        public struct Menu
        {
            public static string m_LastPressedNavigationKey;
            public static int m_iItemHeight;
            public static int m_iSelector = 0;
            public static int m_iNumberOfItems;
            public static Vector2D m_Position;
            public static Vector2D m_Size;
        };

        public struct Settings
        {
            public static bool m_bESP;
            public static bool m_bSnaplines;
            public static bool m_bBunnyhop;
        };

        public struct Item
        {
            public int m_ControlType; // For Later Use
            public bool m_bIsActive;
            public bool m_bIsHovered;
            public string m_sName;

            internal class Arrays
            {
                public static Item[] ItemList = new Item[50];
            }
        };
    }
}
