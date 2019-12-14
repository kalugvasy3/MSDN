using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//https://gist.github.com/caseykramer/3161704

namespace PerformanceOfCollectionCsharp
{
    public static class Set

    {

        public static class Tree

        {

            public static EmptyTree<T> Empty<T>() where T : IComparable

            {

                return new EmptyTree<T>();

            }

        }



        public class EmptyTree<T> : Tree<T> where T : IComparable

        {

            public override bool IsEmpty { get { return true; } }

        }



        public class Tree<T> where T : IComparable

        {

            public Tree<T> LeftSubtree { get; internal set; }

            public Tree<T> RigetSubtree { get; internal set; }

            public T Element { get; internal set; }

            public virtual bool IsEmpty

            {

                get { return false; }

            }

        }


        public static bool IsMember<T>(T element, Tree<T> tree) where T : IComparable

        {

            if (tree.IsEmpty)

                return false;

            var currentElement = tree.Element;

            var currentTree = tree;

            while (!currentTree.IsEmpty)

            {

                if (element.CompareTo(currentElement) == 0)

                    return true;

                if (element.CompareTo(currentElement) == 1)

                {

                    currentTree = currentTree.RigetSubtree;

                }

                else

                {

                    currentTree = currentTree.LeftSubtree;

                }

                currentElement = currentTree.Element;

            }

            return false;

        }



        public static Tree<T> Insert<T>(T element, Tree<T> tree) where T : IComparable

        {

            if (tree.IsEmpty)

                return new Tree<T> { LeftSubtree = Tree.Empty<T>(), Element = element, RigetSubtree = Tree.Empty<T>() };

            switch (element.CompareTo(tree.Element))

            {

                case 0:

                    return tree;

                case 1:

                    return new Tree<T> { RigetSubtree = tree.RigetSubtree, Element = tree.Element, LeftSubtree = Set.Insert<T>(element, tree.LeftSubtree) };

                default:

                    return new Tree<T> { LeftSubtree = tree.LeftSubtree, Element = tree.Element, RigetSubtree = Set.Insert<T>(element, tree.RigetSubtree) };

            }

        }

    }
}
