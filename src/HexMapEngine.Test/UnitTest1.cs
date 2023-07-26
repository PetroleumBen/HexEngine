using HexMapEngine;
using System.Numerics;

namespace HexMapEngine.Test
{

    public class TestTile : IHexTile<TestTile>
    {        
        public float OxygenPPM { get; set; } = 0.0f;

        public HexCoords Coords { get; set; }

        public HexMap<TestTile> Map { get; set; } = new HexMap<TestTile>();
    }


    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }


        [Test]
        public void MoveInDirection()
        {
            HexCoords hexCoord = new HexCoords(0, 0);

            Assert.That(hexCoord + HexDirection.N == new HexCoords(0, 1), "Move North");
            Assert.That(hexCoord + HexDirection.NE == new HexCoords(1, 0), "Move North East");
            Assert.That(hexCoord + HexDirection.SE == new HexCoords(1, -1), "Move South Eat");
            Assert.That(hexCoord + HexDirection.S == new HexCoords(0, -1), "Move South");
            Assert.That(hexCoord + HexDirection.SW == new HexCoords(-1, 0), "Move South West");
            Assert.That(hexCoord + HexDirection.NW == new HexCoords(-1, 1), "Move North West");
        }


        [Test]
        public void Distances()
        {
            HexCoords hC1 = new HexCoords(3, 0);
            HexCoords hC2 = new HexCoords(2, -3);
            HexCoords hC3 = new HexCoords(-1, 3);

            Assert.That(hC1.Magnitude() == 3, "hC1 to 0");
            Assert.That(hC2.Magnitude() == 3, "hC2 to 0");
            Assert.That(hC3.Magnitude() == 3, "hC3 to 0");


            Assert.That(hC1.DistanceTo(hC2) == 4, "hC1 to hC2");
            Assert.That(hC2.DistanceTo(hC3) == 6, "hC2 to hC3");
            Assert.That(hC3.DistanceTo(hC1) == 4, "hC3 to hC1");
        }

    }
}