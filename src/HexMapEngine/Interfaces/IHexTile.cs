using System.Collections.Generic;

namespace HexMapEngine 
{
    public interface IHexTile 
    {
        HexCoords Coords { get; }
    }
    

    public interface IHexTile<T> : IHexTile where T : IHexTile<T> 
    {
        HexMap<T> Map { get; }
    }


    public static class HexTileExtension {
        public static T GetNeighbour<T>(this T tile, HexDirection direction) where T : IHexTile<T> => tile.Map[tile.Coords + direction];

        public static HexDirection GetDirectionToNeighbour<T>(this T tile, T neighbour) where T : IHexTile<T> => (neighbour.Coords - tile.Coords).ToDirection();

        public static int GetDistanceTo<T>(this T tile, T toTile) where T : IHexTile<T> => tile.Coords.DistanceTo(toTile.Coords);

        public static IEnumerable<T> AllNeighbours<T>(this T tile) where T : IHexTile<T> 
        {
            foreach (var direction in HexDirection.N.Loop()) 
            {
                var neighbour = tile.GetNeighbour(direction);
                if (neighbour != null) 
                    yield return neighbour;
            }
        }
    }
}