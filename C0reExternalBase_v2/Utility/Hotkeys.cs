using System;
using System.Runtime.InteropServices;
using static C0reExternalBase_v2.Structs.CheatMenu;

namespace C0reExternalBase_v2.Utility
{
    class Hotkeys
    {
        // Key States (Public Use)
        public static bool KEY_ARROW_UP_STATE;
        public static bool KEY_ARROW_DOWN_STATE;
        public static bool KEY_ARROW_LEFT_STATE;
        public static bool KEY_ARROW_RIGHT_STATE;
        public static bool KEY_SPACEBAR_STATE;

        // Key Codes
        private const int ARROW_UP = 38;
        private const int ARROW_DOWN = 40;
        private const int ARROW_LEFT = 37;
        private const int ARROW_RIGHT = 39;
        private const int SPACEBAR = 32;

        // Key States
        private const int WM_KEYDOWN = 0x100;
        private const int WM_KEYUP = 0x101;
        private const int WM_SYSKEYDOWN = 0x104;
        private const int WM_SYSKEYUP = 0x105;
        private const int WM_KEYBOARD_LL = 0x00D;

        public static IntPtr hookProc(int code, IntPtr wParam, IntPtr lParam)
        {
            int vkCode = Marshal.ReadInt32(lParam);

            switch (vkCode)
            {
                case ARROW_UP:
                    KEY_ARROW_UP_STATE = GetKeystate(wParam);
                    break;
                case ARROW_DOWN:
                    KEY_ARROW_DOWN_STATE = GetKeystate(wParam);
                    break;
                case ARROW_LEFT:
                    KEY_ARROW_LEFT_STATE = GetKeystate(wParam);
                    break;
                case ARROW_RIGHT:
                    KEY_ARROW_RIGHT_STATE = GetKeystate(wParam);
                    break;
                case SPACEBAR:
                    KEY_SPACEBAR_STATE = GetKeystate(wParam);
                    break;
                default:
                    break;

            }
            return CallNextHookEx(hhook, code, (int)wParam, lParam);
        }

        public static bool GetKeystate(IntPtr wParam)
        {
            if (wParam == (IntPtr)WM_KEYDOWN)
                return true;
            else
                return false;
        }

        #region DllImports

        [DllImport("user32.dll")]
        public static extern IntPtr CallNextHookEx(IntPtr idHook, int nCode, int wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc callback, IntPtr hInstance, uint threadId);

        [DllImport("kernel32.dll")]
        static extern IntPtr LoadLibrary(string lpFileName);

        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        private static LowLevelKeyboardProc _proc = hookProc;

        public static IntPtr hhook = IntPtr.Zero;

        public static void SetHook()
        {
            IntPtr hInstance = LoadLibrary("User32");
            hhook = SetWindowsHookEx(WM_KEYBOARD_LL, _proc, hInstance, 0);
        }
        #endregion
    }
}
