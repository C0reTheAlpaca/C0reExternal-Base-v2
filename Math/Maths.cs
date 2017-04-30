namespace C0reExternalBase_v2
{
    class Maths
    {
        public static float DEG2RAD(float Yaw)
        {
            return Yaw * ((float)System.Math.PI / 180f);
        }

        public static float RAD2DEG(float Yaw)
        {
            return Yaw * (180f / (float)System.Math.PI);
        }
    }
}
