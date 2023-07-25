using System.Numerics;


namespace HexMapEngine.Utilities
{
    public static class HexMath
    {
        public const float OutToInRadius = 0.866025404f;
        public const float InToOutRadius = 1.15470052f;

        private const float Sqrt3 = 1.73205080757f;
        private const float A = -Sqrt3 / 3f;
        private const float B = 2f * Sqrt3 / 3f;

        /// <summary>
        /// Returns HexCoords of hex on given point (ignoring point.y value).
        /// </summary>
        public static HexCoords PointToHexCoords(Vector3 point, float gridScale = 1f)
        {
            return RoundPositionToCoords(PointToHexPosition(point, gridScale));
        }

        /// <summary>
        /// Converts 3D point to Vector2 containing axial position.
        /// </summary>
        public static HexPosition PointToHexPosition(Vector3 point, float gridScale = 1f)
        {
            var x = B * point.X;
            var y = A * point.X + point.Z;
            return new HexPosition(x / gridScale, y / gridScale);
        }

        /// <summary>
        /// Rounds axial coordinates and converts them to the HexCoords.
        /// </summary>
        public static HexCoords RoundPositionToCoords(HexPosition position)
        {
            var x = position.X;
            var y = position.Y;
            var z = position.Z;
            var xr = (int)Math.Round(x);
            var yr = (int)Math.Round(y);
            var zr = (int)Math.Round(z);

            var diffX = Math.Abs(x - xr);
            var diffY = Math.Abs(y - yr);
            var diffZ = Math.Abs(z - zr);

            if (diffX > diffY && diffX > diffZ)
                xr = -yr - zr;
            else if (diffY > diffZ)
                yr = -xr - zr;

            return new HexCoords(xr, yr);
        }


        public static Quaternion ToQuaternion(float x, float y, float z) => ToQuaternion(new Vector3(x, y, z));


        /// <summary>
        /// Vector3 to Quanternion
        /// 
        /// https://stackoverflow.com/a/70462919
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static Quaternion ToQuaternion(Vector3 v)
        {

            float cy = (float)Math.Cos(v.Z * 0.5);
            float sy = (float)Math.Sin(v.Z * 0.5);
            float cp = (float)Math.Cos(v.Y * 0.5);
            float sp = (float)Math.Sin(v.Y * 0.5);
            float cr = (float)Math.Cos(v.X * 0.5);
            float sr = (float)Math.Sin(v.X * 0.5);

            return new Quaternion
            {
                W = (cr * cp * cy + sr * sp * sy),
                X = (sr * cp * cy - cr * sp * sy),
                Y = (cr * sp * cy + sr * cp * sy),
                Z = (cr * cp * sy - sr * sp * cy)
            };

        }


        /// <summary>
        /// Quanternion to Vector3 Euler Angle
        /// 
        /// https://stackoverflow.com/a/70462919
        /// </summary>
        /// <param name="q"></param>
        /// <returns></returns>
        public static Vector3 ToEulerAngles(Quaternion q)
        {
            Vector3 angles = new();

            // roll / x
            double sinr_cosp = 2 * (q.W * q.X + q.Y * q.Z);
            double cosr_cosp = 1 - 2 * (q.X * q.X + q.Y * q.Y);
            angles.X = (float)Math.Atan2(sinr_cosp, cosr_cosp);

            // pitch / y
            double sinp = 2 * (q.W * q.Y - q.Z * q.X);
            if (Math.Abs(sinp) >= 1)
            {
                angles.Y = (float)Math.CopySign(Math.PI / 2, sinp);
            }
            else
            {
                angles.Y = (float)Math.Asin(sinp);
            }

            // yaw / z
            double siny_cosp = 2 * (q.W * q.Z + q.X * q.Y);
            double cosy_cosp = 1 - 2 * (q.Y * q.Y + q.Z * q.Z);
            angles.Z = (float)Math.Atan2(siny_cosp, cosy_cosp);

            return angles;
        }


        public static Vector3 LeftChainTransform(Vector3 value, Quaternion rotation)
        {
            float x2 = rotation.X + rotation.X;
            float y2 = rotation.Y + rotation.Y;
            float z2 = rotation.Z + rotation.Z;

            float wx2 = rotation.W * x2;
            float wy2 = rotation.W * y2;
            float wz2 = rotation.W * z2;
            float xx2 = rotation.X * x2;
            float xy2 = rotation.X * y2;
            float xz2 = rotation.X * z2;
            float yy2 = rotation.Y * y2;
            float yz2 = rotation.Y * z2;
            float zz2 = rotation.Z * z2;

            return new Vector3(
                value.X * (1.0f - yy2 - zz2) + value.Y * (xy2 - wz2) + value.Z * (xz2 + wy2),
                value.X * (xy2 + wz2) + value.Y * (1.0f - xx2 - zz2) + value.Z * (yz2 - wx2),
                value.X * (xz2 - wy2) + value.Y * (yz2 + wx2) + value.Z * (1.0f - xx2 - yy2));
        }


        // Modified as see https://stackoverflow.com/q/68180194
        public static Vector3 RightChainTransform(this Vector3 value, Quaternion rotation)
        {
            var q = new Quaternion(value.X, value.Y, value.Z, 0.0f);
            var conjugate = Quaternion.Conjugate(rotation);
            var res = conjugate.Multiply(q).Multiply(rotation);
            return new Vector3(res.X, res.Y, res.Z);
        }


        // As see https://stackoverflow.com/q/68180194
        public static Quaternion Multiply(this Quaternion value1, Quaternion value2)
        {
            var tmp_00 = (value1.Z - value1.Y) * (value2.Y - value2.Z);
            var tmp_01 = (value1.W + value1.X) * (value2.W + value2.X);
            var tmp_02 = (value1.W - value1.X) * (value2.Y + value2.Z);
            var tmp_03 = (value1.Y + value1.Z) * (value2.W - value2.X);
            var tmp_04 = (value1.Z - value1.X) * (value2.X - value2.Y);
            var tmp_05 = (value1.Z + value1.X) * (value2.X + value2.Y);
            var tmp_06 = (value1.W + value1.Y) * (value2.W - value2.Z);
            var tmp_07 = (value1.W - value1.Y) * (value2.W + value2.Z);
            var tmp_08 = tmp_05 + tmp_06 + tmp_07;
            var tmp_09 = (tmp_04 + tmp_08) * 0.5f;

            return new Quaternion(tmp_01 + tmp_09 - tmp_08,
                                  tmp_02 + tmp_09 - tmp_07,
                                  tmp_03 + tmp_09 - tmp_06,
                                  tmp_00 + tmp_09 - tmp_05);
        }

    }
}