using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KalorAform.MultiwayTree
{
    public class MultiwayTreeNode<T> : MultiwayTree<T>
    {
        public MultiwayTreeNode() : base(new List<MultiwayTree<T>>())
        {
        }

        public void AddChild(MultiwayTreeNode<T> child)
        {
            Children.Add(child);
        }

        public async Task<dynamic> VisitNodeAsync(Func<MultiwayTree<T>, Task<dynamic>> visitActionAsync)
        {
            if (visitActionAsync == null)
                return Task.CompletedTask;

            return await visitActionAsync(this);
        }

        public bool IsLeaf()
        {
            return !this.Children.Any();
        }
    }
}
