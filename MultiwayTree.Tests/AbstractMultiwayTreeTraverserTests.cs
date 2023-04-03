using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MultiwayTree.Tests
{
    public class TestNodeVisitResult
    {
        public int NodeValue { get; set; }
    }

    public class ConcreteMultiwayTreeTraverser : AbstractMultiwayTreeTraverser<int, List<TestNodeVisitResult>>
    {
        protected async override Task<NodeVisitResult<List<TestNodeVisitResult>>> VisitNodeAsync(int data, NodeVisitResult<List<TestNodeVisitResult>> nodeVisitsResult)
        {
            if (nodeVisitsResult.Value == null)
            {
                nodeVisitsResult.Value = new List<TestNodeVisitResult>();
            }
            nodeVisitsResult.Value.Add(new TestNodeVisitResult { NodeValue = data });
            return nodeVisitsResult;
        }
    }

    public class ConcreteMultiwayTreeTraverserWithEarlyExit : AbstractMultiwayTreeTraverser<int, List<TestNodeVisitResult>>
    {
        protected async override Task<NodeVisitResult<List<TestNodeVisitResult>>> VisitNodeAsync(int data, NodeVisitResult<List<TestNodeVisitResult>> nodeVisitsResult)
        {
            if (nodeVisitsResult.Value == null)
            {
                nodeVisitsResult.Value = new List<TestNodeVisitResult>();
            }
            nodeVisitsResult.Value.Add(new TestNodeVisitResult { NodeValue = data });
            // stop traversing when node containing the value '2' is encountered
            if (data == 2)
            {
                nodeVisitsResult.ContinueTraversing = false;
            }
            else
            {
                nodeVisitsResult.ContinueTraversing = true;
            }
            return nodeVisitsResult;
        }
    }

    public class AbstractMultiwayTreeTraverserTests
    {
        private MultiwayTree<int> _testTree = new MultiwayTree<int>(100);

        [OneTimeSetUp]
        public void BuildTestTree()
        {
            // Builds a 3-ary tree that looks like this:
            //               ____0_____
            //              /    |     \
            //             1     2     _3_
            //            /|\   /|\   / | \
            //           4 5 6 7 8 9 10 11 12
            _testTree = new MultiwayTree<int>(0);
            var firstChild = _testTree.AddChild(1);
            firstChild.AddChild(4);
            firstChild.AddChild(5);
            firstChild.AddChild(6);
            var secondChild = _testTree.AddChild(2);
            secondChild.AddChild(7);
            secondChild.AddChild(8);
            secondChild.AddChild(9);
            var thirdChild = _testTree.AddChild(3);
            thirdChild.AddChild(10);
            thirdChild.AddChild(11);
            thirdChild.AddChild(12);
        }

        private void AssertTraversalOrder(List<TestNodeVisitResult> nodeVisitResults, params int[] expectedNodeValues)
        {
            for(var i = 0; i < expectedNodeValues.Length; i++)
            {
                var nodeVisitNumber = nodeVisitResults.FindIndex(x => x.NodeValue.Equals(expectedNodeValues[i]));
                Assert.That(nodeVisitNumber != -1, $"Node with value {expectedNodeValues[i]} was not visited (this is not expected).");
                Assert.That(nodeVisitNumber == i, $"Node with value {expectedNodeValues[i]} was visited out of order.");
            }
        }

        public class LevelOrderTests: AbstractMultiwayTreeTraverserTests
        {
            private TraversalType _traversalType = TraversalType.LevelOrder;

            [Test]
            public async Task WhenVisitNodeAsyncAlwaysSetsContinueTraversalToTrue_TraversalVisitsAllNodes()
            {
                var myTreeTraverser = new ConcreteMultiwayTreeTraverser();
                var traversalResult = await myTreeTraverser.TraverseAsync(_testTree, _traversalType);
                Assert.AreEqual(13, traversalResult.Count);
                AssertTraversalOrder(traversalResult, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12);
            }

            [Test]
            public async Task WhenVisitNodeAsyncSetsContinueTraversalToFalse_TraversalCeases()
            {
                var myTreeTraverser = new ConcreteMultiwayTreeTraverserWithEarlyExit();
                var traversalResult = await myTreeTraverser.TraverseAsync(_testTree, _traversalType);
                Assert.AreEqual(3, traversalResult.Count);
                AssertTraversalOrder(traversalResult, 0, 1, 2);
            }
        }

        public class PreOrderTests : AbstractMultiwayTreeTraverserTests
        {
            private TraversalType _traversalType = TraversalType.PreOrder;

            [Test]
            public async Task WhenVisitNodeAsyncAlwaysSetsContinueTraversalToTrue_TraversalVisitsAllNodes()
            {
                var myTreeTraverser = new ConcreteMultiwayTreeTraverser();
                var traversalResult = await myTreeTraverser.TraverseAsync(_testTree, _traversalType);
                Assert.AreEqual(13, traversalResult.Count);
                AssertTraversalOrder(traversalResult, 0, 1, 4, 5, 6, 2, 7, 8, 9, 3, 10, 11, 12);
            }

            [Test]
            public async Task WhenVisitNodeAsyncSetsContinueTraversalToFalse_TraversalCeases()
            {
                var myTreeTraverser = new ConcreteMultiwayTreeTraverserWithEarlyExit();
                var traversalResult = await myTreeTraverser.TraverseAsync(_testTree, _traversalType);
                Assert.AreEqual(6, traversalResult.Count);
                AssertTraversalOrder(traversalResult, 0, 1, 4, 5, 6, 2);
            }
        }

        public class PostOrderTests: AbstractMultiwayTreeTraverserTests
        {
            private TraversalType _traversalType = TraversalType.PostOrder;

            [Test]
            public async Task WhenVisitNodeAsyncAlwaysSetsContinueTraversalToTrue_TraversalVisitsAllNodes()
            {
                var myTreeTraverser = new ConcreteMultiwayTreeTraverser();
                var traversalResult = await myTreeTraverser.TraverseAsync(_testTree, _traversalType);
                Assert.AreEqual(13, traversalResult.Count);
                AssertTraversalOrder(traversalResult, 4, 5, 6, 1, 7, 8, 9, 2, 10, 11, 12, 3, 0);
            }

            [Test]
            public async Task WhenVisitNodeAsyncSetsContinueTraversalToFalse_TraversalCeases()
            {
                var myTreeTraverser = new ConcreteMultiwayTreeTraverserWithEarlyExit();
                var traversalResult = await myTreeTraverser.TraverseAsync(_testTree, _traversalType);
                Assert.AreEqual(8, traversalResult.Count);
                AssertTraversalOrder(traversalResult, 4, 5, 6, 1, 7, 8, 9, 2);
            }
        }

        public class LevelOrderSearchTests : AbstractMultiwayTreeTraverserTests
        {
            private TraversalType _traversalType = TraversalType.LevelOrderSearch;

            [Test]
            public async Task WhenVisitNodeAsyncAlwaysSetsContinueTraversalToTrue_TraversalVisitsAllNodes()
            {
                var myTreeTraverser = new ConcreteMultiwayTreeTraverser();
                var traversalResult = await myTreeTraverser.TraverseAsync(_testTree, _traversalType);
                Assert.AreEqual(13, traversalResult.Count);
                AssertTraversalOrder(traversalResult, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12);
            }

            [Test]
            public async Task WhenVisitNodeAsyncSetsContinueTraversalToFalse_TraversalOnlyExploresBranchesForNodesThatReturnTrue()
            {
                var myTreeTraverser = new ConcreteMultiwayTreeTraverserWithEarlyExit();
                var traversalResult = await myTreeTraverser.TraverseAsync(_testTree, _traversalType);
                Assert.AreEqual(10, traversalResult.Count);
                AssertTraversalOrder(traversalResult, 0, 1, 2, 3, 4, 5, 6, 10, 11, 12);
            }
        }
    }
}
