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
        public static bool KEY_ALT_STATE;

        // Key Codes
        public enum KeyCodeConstants
        {
            NULL = 0,
            ALT = 164,
            SPACEBAR = 32,
            ARROW_LEFT = 37,
            ARROW_UP = 38,
            ARROW_RIGHT = 39,
            ARROW_DOWN = 40,
        };

        // Key States
        private const int WM_KEYDOWN = 0x100;
        private const int WM_KEYUP = 0x101;
        private const int WM_SYSKEYDOWN = 0x104;
        private const int WM_SYSKEYUP = 0x105;
        private const int WM_KEYBOARD_LL = 0x00D;
        public const uint MouseLeftDown = 0x0002;
        public const uint MouseLeftUp = 0x0004;

        public static IntPtr hookProc(int code, IntPtr wParam, IntPtr lParam)
        {
            int vkCode = Marshal.ReadInt32(lParam);

            switch (vkCode)
            {
                case (int)KeyCodeConstants.ARROW_UP:
                    KEY_ARROW_UP_STATE = GetKeystate(wParam);
                    break;
                case (int)KeyCodeConstants.ARROW_DOWN:
                    KEY_ARROW_DOWN_STATE = GetKeystate(wParam);
                    break;
                case (int)KeyCodeConstants.ARROW_LEFT:
                    KEY_ARROW_LEFT_STATE = GetKeystate(wParam);
                    break;
                case (int)KeyCodeConstants.ARROW_RIGHT:
                    KEY_ARROW_RIGHT_STATE = GetKeystate(wParam);
                    break;
                case (int)KeyCodeConstants.SPACEBAR:
                    KEY_SPACEBAR_STATE = GetKeystate(wParam);
                    break;
                case (int)KeyCodeConstants.ALT:
                    KEY_ALT_STATE = GetSysKeystate(wParam);
                    break;
                default:
                    break;

            }
            return CallNextHookEx(hhook, code, (int)wParam, lParam);
        }

        public static bool GetKeystate(IntPtr wParam)
        {
            return (wParam == (IntPtr)WM_KEYDOWN);
        }

        public static bool GetSysKeystate(IntPtr wParam)
        {
            return (wParam == (IntPtr)WM_SYSKEYDOWN);
        }

        #region DllImports

        [DllImport("user32.dll")]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, UIntPtr dwExtraInfo);

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
