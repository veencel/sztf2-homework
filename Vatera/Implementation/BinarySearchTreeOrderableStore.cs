using System;
using Vatera.Interfaces;

namespace Vatera.Implementation
{
    class BinarySearchTreeOrderableStore: IOrderableStore
    {
        internal class Node
        {
            public IOrderable Value;
            public Node Left;
            public Node Right;

            public int Count => 1 + (Left?.Count ?? 0) + (Right?.Count ?? 0);
        }

        private Node _root;

        public int Count
        {
            get
            {
                if (_root == null)
                {
                    return 0;
                }

                return _root.Count;
            }
        }

        public void Insert(IOrderable orderable)
        {
            _root = Insert(_root, orderable);
        }

        public IOrderable BinarySearch(string Identifier)
        {
            return BinarySearch(_root, Identifier)?.Value;
        }

        public IOrderable SearchByName(string Name)
        {
            return SearchByName(_root, Name)?.Value;
        }

        public void Remove(string identifier)
        {
            Remove(_root, identifier);
        }

        public IOrderable[] ToArray()
        {
            IOrderable[] orderables = new IOrderable[Count];
            int count = 0;

            ToArray(_root, orderables, ref count);

            return orderables;
        }

        private Node Insert(Node root, IOrderable orderable)
        {
            if (root == null)
            {
                root = new Node();
                root.Value = orderable;
            }
            else if (String.Compare(orderable.Identifier, root.Value.Identifier) < 0)
            {
                root.Left = Insert(root.Left, orderable);
            }
            else
            {
                root.Right = Insert(root.Right, orderable);
            }

            return root;
        }

        private Node BinarySearch(Node root, string Identifier)
        {
            if (root == null)
            {
                return null;
            }

            if (root.Value.Identifier == Identifier)
            {
                return root;
            }

            if (String.Compare(Identifier, root.Value.Identifier) < 0)
            {
                return BinarySearch(root.Left, Identifier);
            }

            return BinarySearch(root.Right, Identifier);
        }

        private Node SearchByName(Node root, string Name)
        {
            if (root == null)
            {
                return null;
            }

            if (root.Value.Name.StartsWith(Name) || root.Value.Name == Name)
            {
                return root;
            }

            return SearchByName(root.Left, Name)
                   ?? SearchByName(root.Right, Name);
        }

        private Node Remove(Node root, String identifier)
        {
            if (root == null)
            {
                return root;
            }

            if (String.Compare(identifier, root.Value.Identifier) < 0)
            {
                root.Left = Remove(root.Left, identifier);
            }
            else if (String.Compare(identifier, root.Value.Identifier) > 0)
            {
                root.Right = Remove(root.Right, identifier);
            }
            else
            {
                if (root.Left == null)
                {
                    return root.Right;
                }
                else if (root.Right == null)
                {
                    return root.Left;
                }

                Node temp = MinValueNode(root.Right);
                root.Value = temp.Value;

                root.Right = Remove(root.Right, temp.Value.Identifier);
            }

            return root;
        }

        private Node MinValueNode(Node root)
        {
            Node current = root;

            while (current != null && current.Left != null)
            {
                current = current.Left;
            }

            return current;
        }

        private void ToArray(Node root, IOrderable[] orderables, ref int count)
        {
            if (root == null)
            {
                return;
            }

            ToArray(root.Left, orderables, ref count);
            orderables[count++] = root.Value;
            ToArray(root.Right, orderables, ref count);
        }
    }
}
