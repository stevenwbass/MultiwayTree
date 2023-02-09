using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KalorAform.MultiwayTree
{
    public interface IMultiwayTreeTraverser
    {
        Task<TResult> TraverseAsync<T, TResult>(MultiwayTree<T> tree) where T : IEquatable<T>;
        Task<TResult> TraverseCumulative<T, TResult>(MultiwayTree<T> tree) where T : IEquatable<T>;
    }
}
