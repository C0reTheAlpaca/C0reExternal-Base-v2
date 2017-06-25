using System;
using System.Runtime.InteropServices;

namespace C0reExternalBase_v2.Structs
{
    [StructLayout(LayoutKind.Explicit)]
    public struct GlowObject
    {
        [FieldOffset(0x00)]
        public int EntityPointer;
        [FieldOffset(0x04)]
        public float r;
        [FieldOffset(0x08)]
        public float g;
        [FieldOffset(0x0C)]
        public float b;
        [FieldOffset(0x10)]
        public float a;
        [FieldOffset(0x14)]
        public int jnk1;
        [FieldOffset(0x18)]
        public int jnk2;
        [FieldOffset(0x1C)]
        public float BloomAmount;
        [FieldOffset(0x20)]
        public int jnk3;

        [FieldOffset(0x24)]
        public bool m_bRenderWhenOccluded;
        [FieldOffset(0x25)]
        public bool m_bRenderWhenUnoccluded;
        [FieldOffset(0x26)]
        public bool m_bFullBloom;

        public static int GetSize()
        {
            return 0x26;
        }
    };
}
