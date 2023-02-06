using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KalorAform
{
    public static class ExtensionMethods
    {
        public enum TraversalType
        {
            InOrder, // depth-first
            LevelOrder, // breadth-first
            PostOrder, // depth-first
            PreOrder // depth-first
        }

        /// <summary>
        /// Performs an iterative traversal of the tree.
        /// As each node is visited, <paramref name="visitNodeAsync"/> is invoked with the <typeparamref name="T"/> Data property of the current node as a parameter.
        /// After each node is visited, <paramref name="exitEarlyAsync"/> is invoked with the result of <paramref name="visitNodeAsync"/> as a parameter.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="R"></typeparam>
        /// <param name="tree"></param>
        /// <param name="visitNodeAsync"></param>
        /// <param name="exitEarlyAsync"></param>
        /// <param name="allNodesVisitedResult"></param>
        /// <returns>Either:
        /// The result of the <paramref name="visitNodeAsync"/> invocation that resulted in <paramref name="exitEarlyAsync"/> returning true
        /// OR <paramref name="allNodesVisitedResult"/></returns>
        public static async Task<R> TraverseAsync<T, R>(
            this MultiwayTree<T> tree,
            Func<T, Task<R>> visitNodeAsync, 
            Func<R, Task<bool>> exitEarlyAsync, 
            R allNodesVisitedResult,
            TraversalType traversalType = TraversalType.LevelOrder
            ) where T: IEquatable<T>
        {
            // TODO: this is preorder traversal, so refactor to reflect that and implement other traversal types
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
