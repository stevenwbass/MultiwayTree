using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KalorAform.MultiwayTree
{
    public interface INodeVisitor
    {
        public Task<TResult> VisitNodeAsync<T, TResult>(T data);
        public Task<TResult> VisitNodeCumulativeAsync<T, TResult>(T data, TResult cumulativeResult);
    }
}
