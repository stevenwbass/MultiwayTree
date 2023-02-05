using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KalorAform.Tests
{
    public class ExtensionMethodsTestsOld
    {
        private MultiwayTree<int> _testTree = new MultiwayTree<int>(100);

        [OneTimeSetUp]
        public void BuildTestTree()
        {
            // Builds a tree that looks like this:
            //                 0
            //                / \
            //               1   2
            //              /|   |\
            //             3 4   5 6
            _testTree = new MultiwayTree<int>(0);
            var firstChild = _testTree.AddChild(1);
            firstChild.AddChild(3);
            firstChild.AddChild(4);
            var secondChild = _testTree.AddChild(2);
            secondChild.AddChild(5);
            secondChild.AddChild(6);
        }

        [Test]
        public async Task WhenExitEarlyAsyncReturnsFalse_AllNodesVisited()
        {
            var visitedNodes = new List<int>();
            var traversalResult = await _testTree.TraverseAsync(
                (i) => { visitedNodes.Add(i); return Task.FromResult(false); },
                (r) => { return Task.FromResult(r); },
                true);

            Assert.IsTrue(traversalResult);
            Assert.AreEqual(7, visitedNodes.Count);
        }
    }
}