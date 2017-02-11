using System;
using System.Drawing;
using SlimDX.Direct3D9;
using static C0reExternalBase_v2.RenderForm;

namespace C0reExternalBase_v2
{
    class OverlayHelper
    {
        public static DateTime m_dtFrameTimeStart;
        public static DateTime m_dtFrameTimeEnd;

        public static Direct3D m_D3D;

        public static Rectangle m_WindowRectangle;

        public static Margins m_Margin;

        public static Device m_Device;

        public static IntPtr m_pWindowHandle;
        public static IntPtr m_pForeGroundWindow;

        public static int m_ScreenWidth;
        public static int m_ScreenHeight;
    }
}
