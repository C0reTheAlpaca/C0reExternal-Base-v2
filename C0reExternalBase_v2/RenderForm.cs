using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using SlimDX.Direct3D9;
using static System.Math;
using static C0reExternalBase_v2.OverlayHelper;
using static C0reExternalBase_v2.RenderHelper;

namespace C0reExternalBase_v2
{
    public partial class RenderForm : Form
    {
        // Retarded TOPMOST fix
        protected override bool ShowWithoutActivation
        {
            get
            {
                return true;
            }
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams createParams = base.CreateParams;
                createParams.ExStyle |= 0x00000008;
                return createParams;
            }
        }

        public RenderForm()
        {
            InitializeComponent();
            SetWindowLong(Handle, GWL_EXSTYLE, (IntPtr)(GetWindowLong(Handle, GWL_EXSTYLE) ^ WS_EX_LAYERED ^ WS_EX_TRANSPARENT));
            SetLayeredWindowAttributes(Handle, 0, 255, LWA_ALPHA);

            var r = new Rectangle();

            foreach (var s in Screen.AllScreens)
                r = Rectangle.Union(r, s.Bounds);

            Top = r.Top;
            Left = r.Left;
            Width = r.Width;
            Height = r.Height;

            m_ScreenWidth = r.Width;
            m_ScreenHeight = r.Height;

            var PresentParams = new PresentParameters();
            PresentParams.Windowed = true;
            PresentParams.SwapEffect = SwapEffect.Discard;
            PresentParams.BackBufferFormat = Format.A8R8G8B8;
            PresentParams.BackBufferWidth = Width;
            PresentParams.BackBufferHeight = Height;
            PresentParams.Multisample = MultisampleType.EightSamples;

            m_D3D = new Direct3D();
            m_Device = new Device(m_D3D, 0, DeviceType.Hardware, Handle, CreateFlags.HardwareVertexProcessing, PresentParams);
            m_Screen = new Render(m_Device, -Left, -Top);
            m_Render = new Render(m_Device);
        }

        public void RenderFrame()
        {
            m_dtFrameTimeStart = DateTime.Now;

            // Clears Last Frame & Starts A New
            m_Device.Clear(ClearFlags.Target, Color.FromArgb(0, 0, 0, 0), 1.0f, 0);
            m_Device.BeginScene();

            // Get Handle Of CS's Window
            m_pWindowHandle = GetWindowHandle();

            // Get Handle Of ForeGroundWindow
            m_pForeGroundWindow = GetForegroundWindow();

            // Check If CS:GO Or Our Overlay Are In Front
            if (m_pWindowHandle == m_pForeGroundWindow || Handle == m_pForeGroundWindow)
            {
                m_WindowRectangle = GetWindowRect(m_pWindowHandle);

                m_Render.DrawShadowText("C0re - ExternalBase v2", m_WindowRectangle.Width / 2 - m_Screen.MeasureString("C0re - ExternalBase v2").Width / 2, 0, Color.Gold, 2);
                m_Screen.DrawFilledCircle(420, 420, 200, Color.Fuchsia);
                m_Screen.DrawCircle(420, 420, 200, 70, Color.Gold);
            }

            // End Frame
            m_Device.EndScene();
            m_Device.Present();

            m_dtFrameTimeEnd = DateTime.Now;

            // Limit Amount of Rendered FPS to 50
            Thread.Sleep(Max(1000 / 50 - (int)(m_dtFrameTimeEnd - m_dtFrameTimeStart).TotalMilliseconds, 0));
        }

        private IntPtr GetWindowHandle()
        {
            var processes = Process.GetProcessesByName("csgo");
            if (processes.Length > 0)
            {
                return processes[0].MainWindowHandle;
            }
            else
            {
                return (IntPtr)null;
            }
        }

        public void OverlayForm_Paint(object sender, PaintEventArgs e)
        {
            if (!DwmIsCompositionEnabled())
            {
                MessageBox.Show("Please enable Windows-Aero & start again.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }

            m_Margin.Left = 0;
            m_Margin.Top = 0;
            m_Margin.Right = Width;
            m_Margin.Bottom = Height;

            DwmExtendFrameIntoClientArea(Handle, ref m_Margin);
        }

        public static Rectangle GetWindowRect(IntPtr handle)
        {
            Rect rect;
            GetClientRect(handle, out rect);
            var p = new Point(0, 0);
            ClientToScreen(handle, ref p);
            rect.Left = p.X;
            rect.Top = p.Y;

            return new Rectangle(p.X, p.Y, rect.Right, rect.Bottom);
        }

        #region DllImports

        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        [DllImport("user32.dll")]
        private static extern bool SetLayeredWindowAttributes(IntPtr hwnd, uint crKey, byte bAlpha, uint dwFlags);

        [DllImport("dwmapi.dll")]
        private static extern void DwmExtendFrameIntoClientArea(IntPtr hWnd, ref Margins pMargins);

        [DllImport("user32.dll")]
        public static extern bool GetClientRect(IntPtr hwnd, out Rect rectangle);

        [DllImport("user32.dll")]
        private static extern bool ClientToScreen(IntPtr hWnd, ref Point lpPoint);

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("dwmapi.dll", PreserveSig = false)]
        public static extern bool DwmIsCompositionEnabled();

        [DllImport("user32.dll")]
        internal static extern IntPtr SetForegroundWindow(IntPtr hWnd);

        #endregion

        #region Structs

        public struct Margins
        {
            public int Left, Right, Top, Bottom;
        }

        public struct Rect
        {
            public int Left, Top, Right, Bottom;
        }

        #endregion

        #region Constants

        public const int GWL_EXSTYLE = -20;
        public const int WS_EX_LAYERED = 0x80000;
        public const int WS_EX_TRANSPARENT = 0x20;
        public const int LWA_ALPHA = 0x2;
        public const int LWA_COLORKEY = 0x1;

        #endregion
    }
}