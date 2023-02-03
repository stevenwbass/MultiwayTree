namespace KalorAform
{
    public class MultiwayTree<T>
    {
        public List<MultiwayTree<T>> Children { get; set; }

        public MultiwayTree(List<MultiwayTree<T>> children)
        {
            Children = children;
        }
    }
}