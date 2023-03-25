namespace MultiwayTree
{
    public enum TraversalType
    {
        /// <summary>
        /// Breadth-first. See unit tests (or wikipedia) for better understanding.
        /// </summary>
        LevelOrder,
        /// <summary>
        /// Depth-first, typically useful for deleting the tree. See unit tests (or wikipedia) for better understanding.
        /// </summary>
        PostOrder,
        /// <summary>
        /// Depth-first, typically useful for copying the tree. See unit tests (or wikipedia) for better understanding.
        /// </summary>
        PreOrder,
        /// <summary>
        /// Breadth-first search. Basically the same as breadth-first search of a BST but on a k-ary tree. 
        /// Will only explore paths through the tree where ContinueTraversing is true on the NodeVisitResult returned by VisitNodeAsync.
        /// Will _not_ 
        /// See unit tests (and/or wikipedia BST) for better understanding.
        /// </summary>
        LevelOrderSearch
    }
}
