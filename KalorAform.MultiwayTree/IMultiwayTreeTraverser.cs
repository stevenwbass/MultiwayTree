namespace KalorAform.MultiwayTree
{
    public interface IMultiwayTreeTraverser<T, TResult> where T : IEquatable<T>
    {
        Task<TResult> TraverseAsync(MultiwayTree<T> tree, TraversalType traversalType);
    }
}
