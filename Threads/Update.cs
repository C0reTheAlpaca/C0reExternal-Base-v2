using System.Threading;
using C0reExternalBase_v2.Structs;
using C0reExternalBase_v2.Variables;
using static C0reExternalBase_v2.Memory;
using static C0reExternalBase_v2.Structs.Entitys;
using static C0reExternalBase_v2.Structs.Entitys.Entity;

namespace C0reExternalBase_v2.Threads
{
    class Update
    {
        public static void Read()
        {
            while (true)
            {
                //LOCAL
                LocalPlayer.m_iBase = ManageMemory.ReadMemory<int>(Offsets.m_ClientPointer + Offsets.m_dwLocalPlayer);
                LocalPlayer.m_iTeam = ManageMemory.ReadMemory<int>(LocalPlayer.m_iBase + Offsets.m_iTeamNum);
                LocalPlayer.m_iClientState = ManageMemory.ReadMemory<int>(Offsets.m_EnginePointer + Offsets.m_dwClientState);
                LocalPlayer.m_iGlowBase = ManageMemory.ReadMemory<int>(Offsets.m_ClientPointer + Offsets.m_dwGlowObject);
                LocalPlayer.m_iJumpFlags = ManageMemory.ReadMemory<int>(LocalPlayer.m_iBase + Offsets.m_fFlags);
                LocalPlayer.m_angEyeAngles = ManageMemory.ReadMemory<QAngle>(LocalPlayer.m_iClientState + Offsets.m_angEyeAngles);
                LocalPlayer.m_VecOrigin = ManageMemory.ReadMemory<Vector3D>(LocalPlayer.m_iBase + Offsets.m_vecOrigin);
                LocalPlayer.Arrays.ViewMatrix = ManageMemory.ReadMatrix<float>(Offsets.m_ClientPointer + Offsets.m_dwViewMatrix, 16);

                //ENTITY
                for (var i = 0; i < 64; i++)
                {
                    Entity Entity = Arrays.Entity[i];

                    Entity.m_iBase = ManageMemory.ReadMemory<int>(Offsets.m_ClientPointer + Offsets.m_dwEntityList + i * 0x10);

                    if (Entity.m_iBase > 0)
                    {
                        Entity.m_VecOrigin = ManageMemory.ReadMemory<Vector3D>(Entity.m_iBase + Offsets.m_vecOrigin);
                        Entity.m_VecMin = ManageMemory.ReadMemory<Vector3D>(Entity.m_iBase + Offsets.m_vecMins);
                        Entity.m_VecMax = ManageMemory.ReadMemory<Vector3D>(Entity.m_iBase + Offsets.m_vecMaxs);

                        Entity.m_iTeam = ManageMemory.ReadMemory<int>(Entity.m_iBase + Offsets.m_iTeamNum);
                        Entity.m_iHealth = ManageMemory.ReadMemory<int>(Entity.m_iBase + Offsets.m_iHealth);
                        Entity.m_iDormant = ManageMemory.ReadMemory<int>(Entity.m_iBase + Offsets.m_bDormant);
                        Entity.m_iGlowIndex = ManageMemory.ReadMemory<int>(Entity.m_iBase + Offsets.m_iGlowIndex);

                        Arrays.Entity[i] = Entity;
                    }
                }
                Thread.Sleep(1);
            }
        }
    }
}
