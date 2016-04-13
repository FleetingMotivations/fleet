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
            int result = mainClass.killAllHumans();
            Assert.AreEqual(5, result);
        }
        
    }
}

