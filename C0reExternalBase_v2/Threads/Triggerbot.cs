using System;
using System.Threading;
using C0reExternalBase_v2.Variables;
using static C0reExternalBase_v2.Memory;
using static C0reExternalBase_v2.Utility.Hotkeys;
using static C0reExternalBase_v2.Structs.Entitys;
using static C0reExternalBase_v2.Structs.CheatMenu;

namespace C0reExternalBase_v2.Threads
{
    class Triggerbot
    {
        public static void Trigger()
        {
            // Infinite Loop
            while (true)
            {
                // Check If Triggerbot Is Active
                if (Settings.m_bTriggerbot)
                {
                    // Check If The Alt Key Is Pressed
                    if (KEY_ALT_STATE)
                    {
                        // Create A New Entity Object
                        Entity InCrossEntity = new Entity();

                        // Get The Id Of The Entity That Crosses Our Crosshair
                        InCrossEntity.m_iID = ManageMemory.ReadMemory<int>(LocalPlayer.m_iBase + Offsets.m_iCrossHairID);

                        // Check If The Entity Can Be A Player
                        if (InCrossEntity.m_iID > 0 && InCrossEntity.m_iID <= 64)
                        {
                            // Get The Entitys Basedress From Memory
                            InCrossEntity.m_iBase = ManageMemory.ReadMemory<int>(Offsets.m_ClientPointer + Offsets.m_dwEntityList + (InCrossEntity.m_iID - 1) * 0x10);

                            // Get The Entitys TeamID
                            InCrossEntity.m_iTeam = ManageMemory.ReadMemory<int>(InCrossEntity.m_iBase + Offsets.m_iTeamNum);

                            // Check If Our Entity Is An Enemy
                            if (InCrossEntity.m_iTeam != LocalPlayer.m_iTeam)
                            {
                                // Simulate Mouse-press/release
                                mouse_event(MouseLeftDown, 0, 0, 0, new UIntPtr());
                                mouse_event(MouseLeftUp, 0, 0, 0, new UIntPtr());
                                Thread.Sleep(25);
                            }
                        }
                    }
                }
                Thread.Sleep(1);
            }
        }
    }
}