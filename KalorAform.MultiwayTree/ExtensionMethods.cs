using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KalorAform
{
    public static class ExtensionMethods
    {
        /// <summary>
        /// Performs an iterative "PreOrder" traversal of the tree.
        /// As each node is visited, <paramref name="visitNodeAsync"/> is invoked with the <typeparamref name="T"/> Data property of the current node as a parameter.
        /// After each node is visited, <paramref name="exitEarlyAsync"/> is invoked with the result of <paramref name="visitNodeAsync"/> as a parameter.
        /// If <paramref name="exitEarlyAsync"/> returns 'true', the traversal will cease and the last result from <paramref name="visitNodeAsync"/> is returned.
        /// If all nodes are visited and <paramref name="exitEarlyAsync"/> returns 'false' for all nodes, <paramref name="allNodesVisitedResult"/> is returned.
        /// </summary>
        /// <param name="node"></param>
        /// <returns>Task<<typeparamref name="R"/>></returns>
        public static async Task<R> TraverseAsync<T, R>(
            this MultiwayTree<T> tree, 
            Func<T, Task<R>> visitNodeAsync, 
            Func<R, Task<bool>> exitEarlyAsync, 
            R allNodesVisitedResult
            ) where T: IEquatable<T>
        {
            var processStack = new Stack<MultiwayTree<T>>();
            processStack.Push(tree);
            while(processStack.Count > 0)
            {
                var n = processStack.Pop();
                
                var visitResult = await visitNodeAsync(n.Data);

                var exitEarlyResult = await exitEarlyAsync(visitResult);
                if (exitEarlyResult) {
                    return visitResult;
                }

                for(int i=n.Children.Count-1; i>= 0; i--)
                {
                    var child = n.Children.ElementAt(i);
                    processStack.Push(child);
                }
            }

            return allNodesVisitedResult;
        }
    }
}
