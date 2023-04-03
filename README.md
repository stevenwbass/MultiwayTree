# MultiwayTree

#### Summary
This repository aims to provide a simple, concise, flexible, and well-tested multiway tree nuget package for dotnet core.

#### Background
A **multiway tree** is a tree that can have more than two children. A **multiway tree of order k** (or an **k-way** or **k-ary tree**) is one in which a tree can have k children. [Learn more about multiway trees](https://faculty.cs.niu.edu/~freedman/340/340notes/340multi.htm)

#### Usage
1. Install the [nuget package](https://www.nuget.org/packages/MultiwayTree) from nuget.org with your favorite package manager.
2. Instantiate a `MultiwayTree<T>` where `T` is your desired tree node data Type.
	* e.g. `new MultiwayTree<int>(0)` will have a `Data` property of Type `int` with value `0`.
3. Add child nodes to your tree.
	* If `myTree` is a `MultiwayTree<int>`, `myTree.AddChild(1)` or `myTree.AddChild(new MultiwayTree<int>(1))` will have the same effect.
	* Note: `T` must be the same Type for all nodes of a tree i.e. if `myTree` is a `MultiwayTree<int>`, then `myTree.AddChild("this is a string")` is not possible.
4. Instantiate your tree traverser.
	* Option 1: Instantiate a `GenericMultiwayTreeTraverser`.
		* Best for simple node visitation logic.
		* Expects two type parameters: 
			1) the Type of `Data` in the tree(s) you'll be traversing, and
			2) the Type of the return value of your node visitation function.
		* Expects a single constructor parameter: An async delegate function defining what it means to visit a node. Lambda syntax is convenient for this e.g. 
			```
			var myTraverser = new GenericMultiwayTreeTraverser<int, string>(
				async (dataInt, cumulativeResultString) => {
					return (cumulativeResultString ?? string.Empty) 
						+ dataInt.ToString();
				}
			);
			```
			* Note: The node visitation function expects two parameters: 
				1) the `Data` property value of the current node, and
				2) the return value from the most recent node visitation function invocation. This will be null when you visit the first node if `T` is nullable.
	* Option 2: Implement your own tree traversal class.
		* Best for more complex node visitation logic, and works well with IoC pattern.
		* Extend `AbstractMultiwayTreeTraverser` e.g. 
		```
			public class MyTreeTraverser : AbstractMultiwayTreeTraverser<int, string>
		```
		* Override and implement `VisitNodeAsync` with whatever logic you want.
			* `VisitNodeAsync` must return a `NodeVisitResult` which has 2 properties:
				1) a `TValue`  `Value` where `TValue` is the second Type parameter you set when extending `AbstractMultiwayTreeTraverser`, and
				2) a `bool` `ContinueTraversing` which is only relevant when your traversal type is `TraversalType.LevelOrderSearch` (see below).
5. Traverse your tree.
	* 4 traversal types are provided via an `enum`:
		1. `TraversalType.LevelOrder`
		2. `TraversalType.PostOrder`
		3. `TraversalType.PreOrder`
		4. `TraversalType.LevelOrderSearch`
	* All traversal types *except* `LevelOrderSearch` will visit every node in the tree. `LevelOrderSearch` will stop traversing the tree when the `ContinueTraversing` property of the `VisitNodeResult` returned from visiting the most recent node is `false`.
	* Example:
	```
		var result = await myTraverser.TraverseAsync(myTree, TraversalType.PreOrder)
	```