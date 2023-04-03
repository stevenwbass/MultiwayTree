namespace MultiwayTree
{
    public class GenericMultiwayTreeTraverser<T, TResult> : AbstractMultiwayTreeTraverser<T, TResult> where T : IEquatable<T>
    {
        private Func<T, NodeVisitResult<TResult>, Task<NodeVisitResult<TResult>>> _visitNodeAsync;

        /// <summary>
        ///     Convenience class in case you don't want/need to add a whole class to traverse your tree, you can use a function parameter to implement your node visitation logic.
        ///     Recommended only for trees with simple node visitation logic.
        /// </summary>
        /// <param name="visitNodeAsync">
        ///     function parameter equivalent of AbstractMultiwayTreeTraverser.VisitNodeAsync
        ///     Your function should:
        ///         * Accept 2 parameters: T (current node's Data property) and NodeVisitResult<TResult> (result returned from visiting most recent node)
        ///         * Return a NodeVisitResult<TResult>
        ///             * If ContinueTraversing is false, tree traversal will cease and NodeVisitResult.Value will be returned from TraverseAsync
        ///             * NodeVisitResult.Value (TResult) will be passed to the next invocation of your visitNodeAsync function along with the next node's Data property (T)
        /// </param>
        public GenericMultiwayTreeTraverser(Func<T, NodeVisitResult<TResult>, Task<NodeVisitResult<TResult>>> visitNodeAsync)
        {
            _visitNodeAsync = visitNodeAsync;
        }

        protected async override Task<NodeVisitResult<TResult>> VisitNodeAsync(T data, NodeVisitResult<TResult> nodeVisitsResult)
        {
            return await _visitNodeAsync(data, nodeVisitsResult);
        }
    }
}
