using System.Collections.Generic;
using System.Linq;

namespace HexMapEngine {

    public interface IHexMap 
    {
        IEnumerable<IHexTile> Tiles { get; }
        public bool HasTile(HexCoords coords);
        public IHexTile GetTile(HexCoords coords);
    }

    
    /// <summary>
    /// Simple class for storing hexes in dictionary.
    /// </summary>
    public class HexMap<T> : IHexMap where T : IHexTile<T> 
    {

        private Dictionary<HexCoords, T> _tiles = new();

        public IEnumerable<T> Tiles => _tiles.Values;

        IEnumerable<IHexTile> IHexMap.Tiles => Tiles.Cast<IHexTile>();
        

        /// <summary>
        /// Tries to return a tile on given coords, returns null if no tile exists.
        /// </summary>
        public T this[HexCoords coords]
        {
            get => _tiles.TryGetValue(coords, out var tile) ? tile : default;
            set => _tiles[coords] = value;
        }


        public bool HasTile(HexCoords coords) => _tiles.ContainsKey(coords);


        public IHexTile GetTile(HexCoords coords) => this[coords];


        public bool TryGetTile<TI>(HexCoords coords, out TI tile) where TI : T
        {
            if (_tiles.TryGetValue(coords, out T t) && t is TI ti)
            {
                tile = ti;
                return true;
            }
            tile = default;
            return false;
        }
    }
}