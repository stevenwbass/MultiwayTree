namespace KalorAform.MultiwayTree
{
    public class NodeVisitResult<TResult>
    {
        public bool ContinueTraversing { get; set; }
        public TResult Value { get; set; }

        public NodeVisitResult(bool continueTraversing, TResult value)
        {
            ContinueTraversing = continueTraversing;
            Value = value;
        }
    }
}
