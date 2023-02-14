namespace KalorAform.MultiwayTree
{
    /// <summary>
    /// Extend this class with your strongly-typed <typeparamref name="T"/> corresponding to your <see cref="KalorAform.MultiwayTree"/>'s Data property type and your desired node visitation result <typeparamref name="TResult"/>.
    /// </summary>
    public abstract class AbstractMultiwayTreeTraverser<T, TResult> : IMultiwayTreeTraverser<T, TResult> where T : IEquatable<T>
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
        ///     Note: Defaults to your <typeparamref name="TResult"/>'s default value (may be null).
        /// </param>
        /// <returns>
        ///     Boolean indicating whether traversal should continue. 
        ///     Return true to continue traversing the tree or false to stop without visiting any more nodes.
        /// </returns>
        public abstract Task<bool> VisitNodeAsync(T data, TResult nodeVisitsResult);


        public Task<TResult> TraverseAsync(MultiwayTree<T> tree, TraversalType traversalType)
        {
            switch (traversalType)
            {
                case TraversalType.LevelOrder:
                    return DoBreadthFirstTraversal(tree);
                case TraversalType.PreOrder:
                    return DoPreOrderTraversal(tree);
                default:
                    throw new NotImplementedException();
            }
        }

        private async Task<TResult> DoBreadthFirstTraversal(MultiwayTree<T> tree)
        {
            var queue = new Queue<Tuple<int, MultiwayTree<T>>>();

            if (tree != null)
                queue.Enqueue(new Tuple<int, MultiwayTree<T>>(0, tree));

            TResult ret = default(TResult);

            while (queue.Count() > 0)
            {
                var queueItem = queue.Dequeue();
                var iLevel = queueItem.Item1;
                var currentNode = queueItem.Item2;

                if (await VisitNodeAsync(currentNode.Data, ret))
                    break;

                foreach (var item in currentNode.Children)
                    queue.Enqueue(new Tuple<int, MultiwayTree<T>>(iLevel + 1, item));
            }

            return ret;
        }

        private async Task<TResult> DoPreOrderTraversal(MultiwayTree<T> tree)
        {
            var processStack = new Stack<MultiwayTree<T>>();
            processStack.Push(tree);

            TResult ret = default(TResult);

            while (processStack.Count > 0)
            {
                var n = processStack.Pop();

                if (!await VisitNodeAsync(n.Data, ret))
                    break;

                for (int i = n.Children.Count - 1; i >= 0; i--)
                {
                    var child = n.Children.ElementAt(i);
                    processStack.Push(child);
                }
            }

            return ret;
        }
    }
}
