using System.Threading;
using C0reExternalBase_v2.Variables;
using static C0reExternalBase_v2.Memory;
using static C0reExternalBase_v2.Utility.Hotkeys;
using static C0reExternalBase_v2.Structs.Entitys;
using static C0reExternalBase_v2.Structs.CheatMenu;

namespace C0reExternalBase_v2.Threads
{
    class Bunnyhop
    {
        public static void Jump()
        {
            while (true)
            {
                // Check If Bhop is Active & Spacebar is Pressed
                if (Settings.m_bBunnyhop && KEY_SPACEBAR_STATE)
                {
                    if (LocalPlayer.m_iJumpFlags == 257 || LocalPlayer.m_iJumpFlags == 263)
                        ManageMemory.WriteMemory<int>(Offsets.m_ClientPointer + Offsets.m_dwForceJump, 5);
                    else
                        ManageMemory.WriteMemory<int>(Offsets.m_ClientPointer + Offsets.m_dwForceJump, 4);
                }
                Thread.Sleep(2);
            }
        }
    }
}
