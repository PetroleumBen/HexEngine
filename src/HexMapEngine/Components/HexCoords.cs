using System;
using System.Collections.Generic;
using System.Numerics;

namespace HexMapEngine
{
    /// <summary>
    /// Representation of hex position or offset.
    /// Stores a position as axial coordinates (X, Y), additionally exposing Z value.
    /// </summary>
    // For reference look at https://www.redblobgames.com/grids/hexagons/#coordinates
    [Serializable]
    public struct HexCoords : IEquatable<HexCoords> 
    {

        public static readonly HexCoords Zero = new();

        private int _x;
        private int _y;
        

        public int X {
            get => _x;
            set => _x = value;
        }


        public int Y {
            get => _y;
            set => _y = value;
        }


        /// <summary>
        /// Z=-X-Y
        /// </summary>
        public int Z => -X - Y;


        public HexCoords(int x, int y) 
        {
            _x = x;
            _y = y;
        }
        
        public static HexCoords operator +(HexCoords a, HexCoords b) => new(a.X + b.X, a.Y + b.Y);
        public static HexCoords operator +(HexCoords a, HexDirection b) => a + b.Coords();
        public static HexCoords operator +(HexDirection a, HexCoords b) => a.Coords() + b;        
        public static HexCoords operator -(HexCoords a, HexCoords b) => new(a.X - b.X, a.Y - b.Y);
        public static HexCoords operator -(HexCoords a, HexDirection b) => a - b.Coords();
        public static HexCoords operator *(HexCoords a, int b) => new(a.X * b, a.Y * b);
        public static HexCoords operator *(int a, HexCoords b) => b * a;
        

        public IEnumerable<HexCoords> Neighbours() 
        {
            for (int i = 0; i < 6; i++) 
                yield return this + ((HexDirection) i).Coords();
        }


        /// <summary>
        /// The offset from the Vector3.zero to the center of the hex.
        /// </summary>
        public Vector3 Offset() 
        {
            return HexDirection.NE.Direction() * X + HexDirection.N.Direction() * Y;
        }


        /// <summary>
        /// Hex distance to the other coords.
        /// </summary>
        public int DistanceTo(HexCoords other) 
        {
            var delta = this - other;
            return (Math.Abs(delta.X) + Math.Abs(delta.Y) + Math.Abs(delta.Z)) / 2;
        }


        /// <summary>
        /// Hex distance to the zero coords.
        /// </summary>
        /// <returns></returns>
        public int Magnitude() => DistanceTo(Zero);


        public bool Equals(HexCoords other) => other.X == X && other.Y == Y;


        public override bool Equals(object obj) => obj is HexCoords coords && Equals(coords);


        public override int GetHashCode() => Tuple.Create(_x, _y).GetHashCode();


        public override string ToString() => $"({X}, {Y}, {Z})";


        public static bool operator ==(HexCoords a, HexCoords b) => a.Equals(b); 


        public static bool operator !=(HexCoords a, HexCoords b) => !(a == b);

    }
}