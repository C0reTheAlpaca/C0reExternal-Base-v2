using System;
using System.Drawing;
using SlimDX.Direct3D9;
using System.Threading;
using System.Diagnostics;
using System.Windows.Forms;
using C0reExternalBase_v2.Threads;
using C0reExternalBase_v2.Utility;
using System.Runtime.InteropServices;
using static System.Math;
using static C0reExternalBase_v2.Memory;
using static C0reExternalBase_v2.OverlayHelper;
using static C0reExternalBase_v2.Menu.CheatMenu;
using static C0reExternalBase_v2.Structs.Entitys;
using static C0reExternalBase_v2.Structs.CheatMenu;
using static C0reExternalBase_v2.Variables.Offsets;
using static C0reExternalBase_v2.Structs.Entitys.Entity;

/// <summary>
/// SORRY FOR THE HUNGARIAN NOTATION ;P C0re
/// </summary>

namespace C0reExternalBase_v2
{
    public partial class RenderForm : Form
    {
        public RenderHelper m_Screen;
        public RenderHelper m_Render;

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
            Hotkeys.SetHook();
            InitializeMenu();
            PrepareRender();
            PrepareMemory();
            StartThreads();
        }

        public void RenderFrame()
        {
            // Save Time of Frame Start
            m_dtFrameTimeStart = DateTime.Now;

            // Clears Last Frame & Starts A New
            m_Device.Clear(ClearFlags.Target, Color.FromArgb(0, 0, 0, 0), 1.0f, 0);
            m_Device.BeginScene();

            // Get Handle Of Games Window
            m_pWindowHandle = GetWindowHandle();

            // Get Handle Of ForeGroundWindow
            m_pForeGroundWindow = GetForegroundWindow();

            // Check If The Game Or Our Overlay Are In Front
            if (m_pWindowHandle == m_pForeGroundWindow || Handle == m_pForeGroundWindow)
            {
                // Get Size Of The Games Window
                m_WindowRectangle = GetWindowRect(m_pWindowHandle);

                // Update Menu Shifting
                m_Render.UpdateShift(-Left + m_WindowRectangle.X, -Top + m_WindowRectangle.Y);

                // Render Menu
                MenuRun(m_Render);

                // Iterate Through All Entitys
                for (var i = 0; i < 64; i++)
                {

                    // Check If Our Entity Is Valid
                    if (Arrays.Entity[i].m_iBase == 0)
                        continue;
                    if (Arrays.Entity[i].m_iBase == LocalPlayer.m_iBase)
                        continue;
                    if (Arrays.Entity[i].m_iHealth < 1)
                        continue;
                    if (Arrays.Entity[i].m_iDormant == 1)
                        continue;

                    int test = Arrays.Entity[i].m_iTeam;
                    int test2 = LocalPlayer.m_iTeam;

                    // Sets Team Colors
                    Color color;
                    if (Arrays.Entity[i].m_iTeam != LocalPlayer.m_iTeam) {
                        color = Color.FromArgb(255, 255, 0, 0); } else {
                        color = Color.FromArgb(255, 0, 255, 0); }

                    // Renders An ESP-Box
                    if(Settings.m_bESP)
                        m_Render.DrawESPBox(i, color, m_WindowRectangle);

                    // Renders Snaplines
                    if (Settings.m_bSnaplines)
                        m_Render.DrawSnaplines(i, Color.FromArgb(255, 170, 170, 170), m_WindowRectangle);
                }
            }

            // End Frame
            m_Device.EndScene();
            m_Device.Present();

            // Save Time of Frame End
            m_dtFrameTimeEnd = DateTime.Now;

            // Limit Amount of Rendered FPS to 50
            Thread.Sleep(Max(1000 / 50 - (int)(m_dtFrameTimeEnd - m_dtFrameTimeStart).TotalMilliseconds, 0));
        }

        private void PrepareRender()
        {
            SetWindowLong(Handle, GWL_EXSTYLE, (IntPtr)(GetWindowLong(Handle, GWL_EXSTYLE) ^ WS_EX_LAYERED ^ WS_EX_TRANSPARENT));
            SetLayeredWindowAttributes(Handle, 0, 255, LWA_ALPHA);

            var r = new Rectangle();

            foreach (var s in Screen.AllScreens)
                r = Rectangle.Union(r, s.Bounds);

            Top = r.Top;
            Left = r.Left;
            Width = r.Width;
            Height = r.Height;

            m_iScreenWidth = r.Width;
            m_iScreenHeight = r.Height;

            var PresentParams = new PresentParameters();
            PresentParams.Windowed = true;
            PresentParams.SwapEffect = SwapEffect.Discard;
            PresentParams.BackBufferFormat = Format.A8R8G8B8;
            PresentParams.BackBufferWidth = Width;
            PresentParams.BackBufferHeight = Height;
            PresentParams.Multisample = MultisampleType.EightSamples;

            m_D3D = new Direct3D();
            m_Device = new Device(m_D3D, 0, DeviceType.Hardware, Handle, CreateFlags.HardwareVertexProcessing, PresentParams);
            m_Screen = new RenderHelper(m_Device, -Left, -Top);
            m_Render = new RenderHelper(m_Device);
        }

        private void PrepareMemory()
        {
            ManageMemory.Initialize("csgo");
            m_ClientPointer = ManageMemory.GetModuleAdress("client");
            m_EnginePointer = ManageMemory.GetModuleAdress("engine");

            for (var i = 0; i < 64; i++)
                Arrays.Entity[i] = new Entity();
        }

        private void StartThreads()
        {
            Update Update = new Update();
            Thread Updater = new Thread(Update.Read);
            Updater.Start();

            Bunnyhop Bunnyhop = new Bunnyhop();
            Thread Hopper = new Thread(Bunnyhop.Jump);
            Hopper.Start();
        }

        private IntPtr GetWindowHandle()
        {
            var processes = Process.GetProcessesByName("csgo");
            if (processes.Length > 0)
                return processes[0].MainWindowHandle;
            else
                return (IntPtr)null;
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

        private static Rectangle GetWindowRect(IntPtr handle)
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