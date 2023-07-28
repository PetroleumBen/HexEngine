using HexMapEngine;
using System.Numerics;

namespace HexMapEngine.Test
{

    public class TestTile : IHexTile<TestTile>
    {        
        public float OxygenPPM { get; set; }

        public HexCoords Coords { get; private set; }

        public HexMap<TestTile> Map { get; private set; }


        public TestTile(HexCoords coords, HexMap<TestTile> map, float? oxygenPPM = null)
        {
            OxygenPPM = oxygenPPM ?? 0.0f;
            Coords = coords;
            Map = map;
        }
    }


    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }


        public static HexMap<TestTile> GenerateHexMap()
        {
            var mapRadius = 3;
            var map = new HexMap<TestTile>();

            for (int x = -mapRadius; x <= mapRadius; x++)
            {
                int y1 = Math.Max(-mapRadius, -x - mapRadius);
                int y2 = Math.Min(mapRadius, -x + mapRadius);
                for (int y = y1; y <= y2; y++)
                {
                    HexCoords coords = new HexCoords(x, y);
                    var tile = new TestTile(coords, map, (x * y) + (x + y));

                    map[coords] = tile;
                }
            }

            return map;
        }



        [Test]
        public void HexMap()
        {
            var map = GenerateHexMap();

            Assert.That(map.Tiles.Count(), Is.EqualTo(37), "Map tile count");

            var tI = map[new HexCoords(3, 0)];
            var tJ = map[new HexCoords(-3, 0)];
            var tK = map[new HexCoords(-1, 0)];

            var ijDistance = tI.GetDistanceTo(tJ);
            Assert.That(ijDistance, Is.EqualTo(6), "Tile GetDistanceTo");

            map.TryGetTile(new HexCoords(9, -5), out TestTile tile1);
            map.TryGetTile(new HexCoords(3, 0), out TestTile tile2);
            Assert.Multiple(() =>
            {
                Assert.That(tile1, Is.EqualTo(default), "Safely get default tile");
                Assert.That(tile2, Is.EqualTo(tI), "Retrieve correct tile");
            });
        }


        [Test]
        public void MoveInDirection()
        {
            HexCoords hexCoord = new HexCoords(0, 0);
            Assert.Multiple(() =>
            {
                Assert.That(hexCoord + HexDirection.N, Is.EqualTo(new HexCoords(0, 1)), "Move North");
                Assert.That(hexCoord + HexDirection.NE, Is.EqualTo(new HexCoords(1, 0)), "Move North East");
                Assert.That(hexCoord + HexDirection.SE, Is.EqualTo(new HexCoords(1, -1)), "Move South Eat");
                Assert.That(hexCoord + HexDirection.S, Is.EqualTo(new HexCoords(0, -1)), "Move South");
                Assert.That(hexCoord + HexDirection.SW, Is.EqualTo(new HexCoords(-1, 0)), "Move South West");
                Assert.That(hexCoord + HexDirection.NW, Is.EqualTo(new HexCoords(-1, 1)), "Move North West");
            });
        }

        [Test]
        public void Distances()
        {
            HexCoords hC1 = new HexCoords(3, 0);
            HexCoords hC2 = new HexCoords(2, -3);
            HexCoords hC3 = new HexCoords(-1, 3);

            Assert.Multiple(() =>
            {
                Assert.That(hC1.Magnitude(), Is.EqualTo(3), "hC1 to 0");
                Assert.That(hC2.Magnitude(), Is.EqualTo(3), "hC2 to 0");
                Assert.That(hC3.Magnitude(), Is.EqualTo(3), "hC3 to 0");

                Assert.That(hC1.DistanceTo(hC2), Is.EqualTo(4), "hC1 to hC2");
                Assert.That(hC2.DistanceTo(hC3), Is.EqualTo(6), "hC2 to hC3");
                Assert.That(hC3.DistanceTo(hC1), Is.EqualTo(4), "hC3 to hC1");
            });
        }

    }
}