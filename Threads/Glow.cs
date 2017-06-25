using System.Threading;
using System.Drawing;
using C0reExternalBase_v2.Structs;
using C0reExternalBase_v2.Variables;
using static C0reExternalBase_v2.Memory;
using static C0reExternalBase_v2.Utility.Hotkeys;
using static C0reExternalBase_v2.Structs.Entitys;
using static C0reExternalBase_v2.Structs.CheatMenu;
using static C0reExternalBase_v2.Structs.Entitys.Entity;

namespace C0reExternalBase_v2.Threads
{
    class Glow
    {
        public static void EngineChams()
        {
            while (true)
            {
                // Check If Glow is Enabled
                if (Settings.m_bEngineChams)
                {
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

                        Color Color = Arrays.Entity[i].m_iTeam != LocalPlayer.m_iTeam ? Color.FromArgb(255, 255, 0, 0) : Color.FromArgb(180, 0, 255, 0);

                        
                        GlowObject GlowObj = new GlowObject();

                        GlowObj = ManageMemory.ReadMemory<GlowObject>(LocalPlayer.m_iGlowBase + Arrays.Entity[i].m_iGlowIndex * 0x38);

                        GlowObj.r = Color.R / 255;
                        GlowObj.g = Color.G / 255;
                        GlowObj.b = Color.B / 255;
                        GlowObj.a = Color.A / 255;
                        GlowObj.m_bRenderWhenOccluded = true;
                        GlowObj.m_bRenderWhenUnoccluded = false;
                        GlowObj.m_bFullBloom = false;

                        //GlowObj.SplitScreenSlot = 0xFFFFFFFF;

                        ManageMemory.WriteMemory<GlowObject>(LocalPlayer.m_iGlowBase + Arrays.Entity[i].m_iGlowIndex * 0x38, GlowObj);


                        /*
                        ManageMemory.WriteMemory<float>(LocalPlayer.m_iGlowBase + Arrays.Entity[i].m_iGlowIndex * 0x38 + 0x4, (float)Color.R / 255);
                        ManageMemory.WriteMemory<float>(LocalPlayer.m_iGlowBase + Arrays.Entity[i].m_iGlowIndex * 0x38 + 0x8, (float)Color.G / 255);
                        ManageMemory.WriteMemory<float>(LocalPlayer.m_iGlowBase + Arrays.Entity[i].m_iGlowIndex * 0x38 + 0xC, (float)Color.B / 255);
                        ManageMemory.WriteMemory<float>(LocalPlayer.m_iGlowBase + Arrays.Entity[i].m_iGlowIndex * 0x38 + 0x10, 1.0f);
                        ManageMemory.WriteMemory<byte>(LocalPlayer.m_iGlowBase + Arrays.Entity[i].m_iGlowIndex * 0x38 + 0x24, 1);

                        //GlowObj = ManageMemory.ReadMemory<GlowObject>(LocalPlayer.m_iGlowBase + Arrays.Entity[i].m_iGlowIndex * 0x38);
                        */
                    }
                }
                Thread.Sleep(5);
            }
        }
    }
}
