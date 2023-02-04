using NUnit.Framework;

namespace KalorAform.Tests
{
    public class MultiwayTreeTests
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
        public void CtorWithNoChildren_Works()
        {
            var multiwayTree = new MultiwayTree<int>(0);
            Assert.AreEqual(0, multiwayTree.Children.Count);
            Assert.AreEqual(0, multiwayTree.Data.CompareTo(0));
            Assert.NotNull(multiwayTree);
        }

        [Test]
        public void WhenTreeHasNoChildren_IsLeafReturnsTrue()
        {
            var multiwayTree = new MultiwayTree<int>(0);
            Assert.IsTrue(multiwayTree.IsLeaf());
        }

        [Test]
        public void WhenTreeHasChildren_IsLeafReturnsFalse()
        {
            var multiwayTree = new MultiwayTree<int>(0);
            multiwayTree.AddChild(new MultiwayTree<int>(0));
            Assert.IsFalse(multiwayTree.IsLeaf());
        }

        [Test]
        public void WhenTreeHasNoParent_IsRootReturnsTrue()
        {
            var multiwayTree = new MultiwayTree<int>(0);
            Assert.IsTrue(multiwayTree.IsRoot());
        }

        [Test]
        public void WhenTreeHasParent_IsRootReturnsFalse()
        {
            var multiwayTree = new MultiwayTree<int>(0);
            var child = new MultiwayTree<int>(0);
            multiwayTree.AddChild(child);
            Assert.IsFalse(child.IsRoot());
        }

        [Test]
        public void WhenChildIsAddedToTree_ParentIsSetOnChild()
        {
            var multiwayTree = new MultiwayTree<int>(0);
            var child = new MultiwayTree<int>(0);
            multiwayTree.AddChild(child);
            Assert.AreEqual(multiwayTree, child.Parent);
        }

        [Test]
        public void WhenTreeDoesNotContainNode_ContainsNodeReturnsFalse()
        {
            var containsNode = _testTree.ContainsNode(7);
            Assert.IsFalse(containsNode);
        }

        [Test]
        public void WhenTreeContainsNode_ContainsNodeReturnsTrueForDataParameter()
        {
            var containsNode = _testTree.ContainsNode(6);
            Assert.IsTrue(containsNode);
        }

        [Test]
        public void WhenTreeContainsNode_ContainsNodeReturnsTrueForTreeParameter()
        {
            var containsNode = _testTree.ContainsNode(new MultiwayTree<int>(6));
            Assert.IsTrue(containsNode);
        }
    }
}