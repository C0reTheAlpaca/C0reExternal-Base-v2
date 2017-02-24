using System.Drawing;
using C0reExternalBase_v2.Structs;
using static C0reExternalBase_v2.Structs.Entitys.LocalPlayer;

namespace C0reExternalBase_v2.Math
{
    class Transformation
    {
        public static bool World2Screen(Vector3D IN, out Vector3D OUT, Rectangle window)
        {
            OUT = new Vector3D();
            var w = 0.0f;

            OUT.x = Arrays.ViewMatrix[0] * IN.x + Arrays.ViewMatrix[1] * IN.y + Arrays.ViewMatrix[2] * IN.z + Arrays.ViewMatrix[3];
            OUT.y = Arrays.ViewMatrix[4] * IN.x + Arrays.ViewMatrix[5] * IN.y + Arrays.ViewMatrix[6] * IN.z + Arrays.ViewMatrix[7];
            w = Arrays.ViewMatrix[12] * IN.x + Arrays.ViewMatrix[13] * IN.y + Arrays.ViewMatrix[14] * IN.z + Arrays.ViewMatrix[15];

            if (w < 0.01f)
                return false;

            var invw = 1.0f / w;
            OUT.x *= invw;
            OUT.y *= invw;

            var width = window.Width;
            var height = window.Height;

            float x = width / 2;
            float y = height / 2;

            x += 0.5f * OUT.x * width + 0.5f;
            y -= 0.5f * OUT.y * height + 0.5f;

            OUT.x = x;
            OUT.y = y;
            return true;
        }
    }
}
