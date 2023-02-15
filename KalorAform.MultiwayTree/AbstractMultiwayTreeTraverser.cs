using System.ComponentModel.Design.Serialization;
using System.Data.SqlTypes;

namespace KalorAform.MultiwayTree
{
    /// <summary>
    /// Extend this class with your strongly-typed <typeparamref name="T"/> corresponding to your <see cref="KalorAform.MultiwayTree"/>'s Data property type and your desired node visitation result <typeparamref name="TResult"/>.
    /// </summary>
    public abstract class AbstractMultiwayTreeTraverser<T, TResult> where T : IEquatable<T>
    {
        /// <summary>
        ///     Override this method in your implementation to define what happens when tree nodes are visited during traversal.
        /// </summary>
        /// <typeparam name="T">
        ///     Your MultiwayTree<typeparamref name="T"/> node's Data property
        /// </typeparam>
        /// <typeparam name="TResult">
        ///     Your strongly-typed node visitation result.
        /// </typeparam>
        /// <param name="data">
        ///     The current tree node's Data property.
        /// </param>
        /// <param name="nodeVisitsResult">
        ///     The result of visiting the current node.
        ///     Will be passed to the next invocation of VisitNodeAsync along with the next tree node's Data property.
        ///     Note: Value property (your <typeparamref name="TResult"/>) may be null if TResult is nullable.
        /// </param>
        /// <returns>
        ///     Boolean indicating whether traversal should continue. 
        ///     Return true to continue traversing the tree or false to stop without visiting any more nodes.
        /// </returns>
        protected abstract Task<NodeVisitResult<TResult>> VisitNodeAsync(T data, NodeVisitResult<TResult> nodeVisitsResult);

        public Task<TResult> TraverseAsync(MultiwayTree<T> tree, TraversalType traversalType)
        {
            switch (traversalType)
            {
                case TraversalType.LevelOrder:
                    return DoLevelOrderTraversalAsync(tree);
                case TraversalType.PreOrder:
                    return DoPreOrderTraversalAsync(tree);
                case TraversalType.PostOrder:
                    return DoPostOrderTraversalAsync(tree);
                default:
                    throw new NotImplementedException();
            }
        }

        private async Task<TResult> DoLevelOrderTraversalAsync(MultiwayTree<T> tree)
        {
            var nodeVisitResult = new NodeVisitResult<TResult>(true, default(TResult));

            var queue = new Queue<Tuple<int, MultiwayTree<T>>>();

            if (tree != null)
                queue.Enqueue(new Tuple<int, MultiwayTree<T>>(0, tree));

            while (queue.Count() > 0)
            {
                var queueItem = queue.Dequeue();
                var iLevel = queueItem.Item1;
                var currentNode = queueItem.Item2;

                nodeVisitResult = await VisitNodeAsync(currentNode.Data, nodeVisitResult);

                if (!nodeVisitResult.ContinueTraversing)
                    break;

                foreach (var item in currentNode.Children)
                    queue.Enqueue(new Tuple<int, MultiwayTree<T>>(iLevel + 1, item));
            }

            return nodeVisitResult.Value;
        }

        private async Task<TResult> DoPreOrderTraversalAsync(MultiwayTree<T> tree)
        {
            var nodeVisitsResult = new NodeVisitResult<TResult>(true, default(TResult));

            var processStack = new Stack<MultiwayTree<T>>();
            processStack.Push(tree);

            while (processStack.Count > 0)
            {
                var n = processStack.Pop();

                nodeVisitsResult = await (VisitNodeAsync(n.Data, nodeVisitsResult));

                if (!nodeVisitsResult.ContinueTraversing)
                    break;

                for (int i = n.Children.Count - 1; i >= 0; i--)
                {
                    var child = n.Children.ElementAt(i);
                    processStack.Push(child);
                }
            }

            return nodeVisitsResult.Value;
        }

        private async Task<TResult> DoPostOrderTraversalAsync(MultiwayTree<T> tree)
        {
            var nodeVisitsResult = new NodeVisitResult<TResult>(true, default(TResult));

            var stack = new Stack<(MultiwayTree<T>, int)>();
            var lastVisited = (tree, 0);
            var currentNode = tree;
            var index = 0;

            while (stack.Count > 0 || currentNode != null)
            {
                if (currentNode != null)
                {
                    stack.Push((currentNode, index));
                    index = 0;
                    currentNode = (currentNode.Children.Count > 0 ? currentNode.Children.First() : null);
                }
                else
                {
                    var peeked = stack.Peek();
                    var node = peeked.Item1;
                    index = peeked.Item2;
                    if (node.Children.Count > 1 && lastVisited.Item1 != node.Children.Last())
                    {
                        index = lastVisited.Item2 + 1;
                        currentNode = node.Children.ElementAt(index);
                    }
                    else
                    {
                        nodeVisitsResult = await VisitNodeAsync(node.Data, nodeVisitsResult);

                        if (!nodeVisitsResult.ContinueTraversing)
                            break;

                        lastVisited = (node, index);
                        stack.Pop();
                    }
                }
            }
            return nodeVisitsResult.Value;
        }
    }
}
