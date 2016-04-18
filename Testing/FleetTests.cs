using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Fleet;

namespace NunitTests
{
    [TestFixture]
    public class FleetUnitTests
    {
        private MainClass mainClass;

        [TestFixtureSetUp]
        public void SetUp()
        {
            mainClass = new MainClass();
        }

        [TestFixtureTearDown]
        public void TearDown()
        {
            mainClass = null;
        }

        [Test]
        public void TestKillAllHumans()
        {
            Assert.GreaterOrEqual(mainClass.killAllHumans(), 0);
        }

        [Test]
        public void ArrayExample()
        {
            int[] array = new int[] { 1, 2, 3 };
            Assert.That(array, Has.Exactly(1).EqualTo(3));
            Assert.That(array, Has.Exactly(2).GreaterThan(1));
            Assert.That(array, Has.Exactly(3).LessThan(100));
        }

        [Test, Combinatorial]
        public void CombinatorialExample(
            [Values(1,2,3)] int x,
            [Values("First String","Second String")] string s)
        {
            /*
             * 1, "First String"
             * 2, "First String"
             * 3, "First String"
             * 1, "Second String"
             * 2, "Second String"
             * 3, "Second String"
             */

            Assert.That(x + x, Is.Not.EqualTo(5));
            Assert.That(s, Is.Not.EqualTo("Third String"));
        }

        [Test]
        public void ConstraintExample()
        {
            Assert.That(2.3, Is.GreaterThan(2.0).And.LessThan(3.0));
        }
        
    }
}

