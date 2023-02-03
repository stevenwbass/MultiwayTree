using NUnit.Framework;

namespace KalorAform.Tests
{
    public class MultiwayTreeTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            new MultiwayTree<int>(null);
        }
    }
}