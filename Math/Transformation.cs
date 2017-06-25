using System;
using System.Drawing;
using C0reExternalBase_v2.Structs;
using static C0reExternalBase_v2.Structs.Entitys;
using static C0reExternalBase_v2.Structs.Entitys.Entity;

namespace C0reExternalBase_v2
{
    class Transformation
    {
        public static bool World2Screen(Vector3D IN, out Vector3D OUT, Rectangle window)
        {
            OUT = new Vector3D();
            var w = 0.0f;

            OUT.x = LocalPlayer.Arrays.ViewMatrix[0] * IN.x + LocalPlayer.Arrays.ViewMatrix[1] * IN.y + LocalPlayer.Arrays.ViewMatrix[2] * IN.z + LocalPlayer.Arrays.ViewMatrix[3];
            OUT.y = LocalPlayer.Arrays.ViewMatrix[4] * IN.x + LocalPlayer.Arrays.ViewMatrix[5] * IN.y + LocalPlayer.Arrays.ViewMatrix[6] * IN.z + LocalPlayer.Arrays.ViewMatrix[7];
            w = LocalPlayer.Arrays.ViewMatrix[12] * IN.x + LocalPlayer.Arrays.ViewMatrix[13] * IN.y + LocalPlayer.Arrays.ViewMatrix[14] * IN.z + LocalPlayer.Arrays.ViewMatrix[15];

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

        public static Vector2D EntityToRadar(int EntityID, Vector2D RadarPosition, int RadarSize)
        {
            Vector2D DotPos;
            Vector2D Direction;

            // Get Origin Of Entity
            Vector3D EntityPosition = Arrays.Entity[EntityID].m_VecOrigin;

            // Get Origin Of LocalPlayer
            Vector3D LocalPosition = LocalPlayer.m_VecOrigin;

            // Calculate Direction
            Direction.x = -(EntityPosition.y - LocalPosition.y);
            Direction.y = EntityPosition.x - LocalPosition.x;

            // Get Rotation
            QAngle LocalAngles = LocalPlayer.m_angEyeAngles;

            float Radian = Maths.DEG2RAD(LocalAngles.y - 90.0f);

            // Calculate Raw DotPos
            DotPos.x = (Direction.y * (float)System.Math.Cos(Radian) - Direction.x * (float)System.Math.Sin(Radian)) / 20.0f;
            DotPos.y = (Direction.y * (float)System.Math.Sin(Radian) + Direction.x * (float)System.Math.Cos(Radian)) / 20.0f;

            // Add RadarPos To Calculated DotPos
            DotPos.x += RadarPosition.x + RadarSize / 2;
            DotPos.y += RadarPosition.y + RadarSize / 2;

            // Clamp Dots To RadarSize ( Where 5 = Width/Height of the Dot)
            if (DotPos.x < RadarPosition.x)
                DotPos.x = RadarPosition.x;

            if (DotPos.x > RadarPosition.x + RadarSize - 5)
                DotPos.x = RadarPosition.x + RadarSize - 5;

            if (DotPos.y < RadarPosition.y)
                DotPos.y = RadarPosition.y;

            if (DotPos.y > RadarPosition.y + RadarSize - 5)
                DotPos.y = RadarPosition.y + RadarSize - 5;

            return DotPos;
        }
    }
}
