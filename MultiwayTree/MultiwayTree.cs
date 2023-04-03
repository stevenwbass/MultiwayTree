namespace MultiwayTree
{
    public class MultiwayTree<T> where T : IEquatable<T>
    {
        public T Data { get; private set; }

        public MultiwayTree<T>? Parent { get; set; }
        public LinkedList<MultiwayTree<T>> Children { get; set; }

        public MultiwayTree(T data, LinkedList<MultiwayTree<T>>? children = null)
        {
            Data = data;
            Parent = null;
            Children = children ?? new LinkedList<MultiwayTree<T>>();
        }

        public void AddChild(MultiwayTree<T> child)
        {
            child.Parent = this;
            Children.AddLast(child);
        }

        public MultiwayTree<T> AddChild(T data)
        {
            var child = new MultiwayTree<T>(data);
            child.Parent = this;
            Children.AddLast(child);
            return child;
        }

        public bool IsRoot()
        {
            return Parent == null;
        }

        public bool IsLeaf()
        {
            return !Children.Any();
        }

        /// <summary>
        /// Performs a "PreOrder" traversal of the tree searching for a node with <see cref="Data"/> matching <paramref name="nodeData"/>.
        /// Returns true as soon as a match is found or false when all nodes have been searched.
        /// Uses the <see cref="IComparable"/> implementation associated with <typeparamref name="T"/> to evaluate whether nodes match.
        /// </summary>
        /// <param name="node"></param>
        /// <returns>bool</returns>
        public bool ContainsNode(T nodeData)
        {
            if (Data.Equals(nodeData))
                return true;

            foreach (var child in Children)
            {
                if (child.ContainsNode(nodeData))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Convenience method. Invokes <see cref="ContainsNode(T)"/> using <paramref name="node"/>'s <see cref="Data"/> property.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public bool ContainsNode(MultiwayTree<T> node)
        {
            return ContainsNode(node.Data);
        }
    }
}