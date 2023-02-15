using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KalorAform.MultiwayTree.Tests
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
            return nodeVisitsResult;
        }
    }

    public class AbstractMultiwayTreeTraverserTests
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

        public class LevelOrderTests: AbstractMultiwayTreeTraverserTests
        {
            [Test]
            public async Task WhenVisitNodeAsyncAlwaysSetsContinueTraversalToTrue_TraversalVisitsAllNodes()
            {
                var myTreeTraverser = new ConcreteMultiwayTreeTraverser();
                var traversalResult = await myTreeTraverser.TraverseAsync(_testTree, TraversalType.LevelOrder);
                Assert.AreEqual(7, traversalResult.Count);
            }

            [Test]
            public async Task WhenVisitNodeAsyncSetsContinueTraversalToFalse_TraversalCeases()
            {
                var myTreeTraverser = new ConcreteMultiwayTreeTraverserWithEarlyExit();
                var traversalResult = await myTreeTraverser.TraverseAsync(_testTree, TraversalType.LevelOrder);
                Assert.AreEqual(3, traversalResult.Count);
            }
        }

        public class PreOrderTests : AbstractMultiwayTreeTraverserTests
        {
            [Test]
            public async Task WhenVisitNodeAsyncAlwaysSetsContinueTraversalToTrue_TraversalVisitsAllNodes()
            {
                var myTreeTraverser = new ConcreteMultiwayTreeTraverser();
                var traversalResult = await myTreeTraverser.TraverseAsync(_testTree, TraversalType.PreOrder);
                Assert.AreEqual(7, traversalResult.Count);
            }

            [Test]
            public async Task WhenVisitNodeAsyncSetsContinueTraversalToFalse_TraversalCeases()
            {
                var myTreeTraverser = new ConcreteMultiwayTreeTraverserWithEarlyExit();
                var traversalResult = await myTreeTraverser.TraverseAsync(_testTree, TraversalType.PreOrder);
                Assert.AreEqual(5, traversalResult.Count);
            }
        }

        public class PostOrderTests: AbstractMultiwayTreeTraverserTests
        {
            // TODO
        }
    }
}
