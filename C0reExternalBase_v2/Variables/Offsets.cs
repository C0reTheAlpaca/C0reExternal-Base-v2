using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C0reExternalBase_v2.Variables
{
    class Offsets
    {
        public static int m_ClientPointer;
        public static int m_EnginePointer;

        public static int m_dwLocalPlayer = 0x00AA66D4;
        public static int m_dwClientState = 0x005CA514;
        public static int m_dwGlowObject = 0x04FE39FC;
        public static int m_dwEntityList = 0x04AC9154;
        public static int m_dwViewMatrix = 0x04ABACF4;
        public static int m_iGlowIndex = 0x0000A320;
        public static int m_vecOrigin = 0x00000134;
        public static int m_lifeState = 0x0000025B;
        public static int m_bDormant = 0x000000E9;
        public static int m_iTeamNum = 0x000000F0;
        public static int m_iHealth = 0x000000FC;
        public static int m_fFlags = 0x00000100;

        // NETVARS
        public static int m_vecMins = 0x0320;
        public static int m_vecMaxs = 0x032C;
    }
}
