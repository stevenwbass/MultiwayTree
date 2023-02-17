using System.Collections.Generic;
using NUnit.Framework;
using System.Threading.Tasks;

namespace KalorAform.MultiwayTree.Tests
{
    public class GenericMultiwayTreeTraverserTests
    {
        private MultiwayTree<int> _testTree = new MultiwayTree<int>(100);

        public class TestNodeVisitResult
        {
            public int NodeValue { get; set; }
        }

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
        public async Task Test()
        {
            var myTraverser = new GenericMultiwayTreeTraverser<int, List<TestNodeVisitResult>>(async (data, nodeVisitResults) => {
                if (nodeVisitResults.Value == null)
                {
                    nodeVisitResults.Value = new List<TestNodeVisitResult>();
                }
                nodeVisitResults.Value.Add(new TestNodeVisitResult() { NodeValue = data });
                return nodeVisitResults;
            });
            var result = await myTraverser.TraverseAsync(_testTree, TraversalType.PreOrder);
            Assert.AreEqual(7, result.Count);
        }
    }
}
