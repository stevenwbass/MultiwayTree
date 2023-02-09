using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KalorAform.MultiwayTree
{
    public class BreadthFirstTraverser : IMultiwayTreeTraverser
    {
        private INodeVisitor _nodeVisitor;

        public BreadthFirstTraverser(INodeVisitor nodeVisitor)
        {
            _nodeVisitor = nodeVisitor ?? throw new ArgumentNullException(nameof(nodeVisitor));
        }

        public async Task<TResult> TraverseAsync<T, TResult>(MultiwayTree<T> tree) where T : IEquatable<T>
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

                ret = await _nodeVisitor.VisitNodeAsync<T, TResult>(currentNode.Data);

                foreach (var item in currentNode.Children)
                    queue.Enqueue(new Tuple<int, MultiwayTree<T>>(iLevel + 1, item));
            }

            return ret;
        }

        public Task<TResult> TraverseCumulative<T, TResult>(MultiwayTree<T> tree) where T : IEquatable<T>
        {
            throw new NotImplementedException();
        }
    }
}
