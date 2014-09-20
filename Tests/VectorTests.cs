using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using NUnit.Framework;
using TeamRNA;

namespace Tests
{
    [TestFixture]
    public class VectorTests
    {
        [Test]
        public void ShouldUnderstandVectorAdd()
        {
            var input = new Vector(100, 100);
            var change = new Vector(50, 30);

            Assert.That(input + change, Is.EqualTo(new Vector(150, 130)));
            Assert.That(change + input, Is.EqualTo(new Vector(150, 130)));
        }

        [Test]
        public void ShouldUnderstandVectorSub()
        {
            var input = new Vector(100, 100);
            var change = new Vector(50, 30);

            Assert.That(input - change, Is.EqualTo(new Vector(50, 70)));
            Assert.That(change - input, Is.EqualTo(new Vector(-50, -70)));
        }

        [Test]
        public void ShouldKnowIfVectorIsInMyDirection()
        {
            var position = new Vector(100, 100);
            var ballPos = new Vector(50, 50);

            Assert.That(InDirection(position, ballPos, new Vector(10, 10)));
            Assert.That(InDirection(position, ballPos, new Vector(0, 10)));
            Assert.That(InDirection(position, ballPos, new Vector(10, 0)));
            Assert.That(InDirection(position, ballPos, new Vector(-10, 10)));
            Assert.That(InDirection(position, ballPos, new Vector(10, -10)));
            Assert.That(!InDirection(position, ballPos, new Vector(-10, -10)));

            ballPos = new Vector(150, 50);

            Assert.That(InDirection(position, ballPos, new Vector(10, 10)));
            Assert.That(InDirection(position, ballPos, new Vector(-10, 10)));
            Assert.That(!InDirection(position, ballPos, new Vector(10, -10)));
            Assert.That(InDirection(position, ballPos, new Vector(-10, -10)));
        }

        public bool InDirection(Vector pos, Vector pos1, Vector dir)
        {
            var sub = (pos - pos1);

            return (sub.Unit() - dir.Unit()).Length < 1.414214;
        }

        [Test]
        public void ShouldCalcNormalToVector()
        {
            var pos = new Vector(0, 0);
            var ballPos = new Vector(50, 0);
            var direction = new Vector(-2, 2);

            Assert.That(InDirection(pos, ballPos, direction));

            var result = DistanceUtils.GetNormalToVector(pos, ballPos, direction);
            Assert.That(result, Is.EqualTo(new Vector(25, 25)));

            Assert.That(DistanceUtils.GetNormalToVector(pos, ballPos, new Vector(0, 2)), Is.EqualTo(new Vector(50, 0)));
            Assert.That(DistanceUtils.GetNormalToVector(pos, ballPos, new Vector(2, 0)), Is.EqualTo(new Vector(0, 0)));
            Assert.That(DistanceUtils.GetNormalToVector(pos, ballPos, new Vector(-2, -2)), Is.EqualTo(new Vector(25, -25)));
        }

    }
}
