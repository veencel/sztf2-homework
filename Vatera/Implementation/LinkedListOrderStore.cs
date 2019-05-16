using System;
using System.Collections;
using System.Collections.Generic;
using Vatera.Interfaces;
using Vatera.Models;

namespace Vatera.Implementation
{
    public class LinkedListOrderStore: IOrderStore
    {
        internal class Node
        {
            public Order Value;
            public Node Next;
        }

        private Node _head;

        public LinkedListOrderStore()
        {
            //
        }

        public LinkedListOrderStore(IOrderStore orders)
        {
            foreach (var order in orders)
            {
                Insert(order);
            }
        }

        public int Count
        {
            get
            {
                int count = 0;

                foreach (var order in this)
                {
                    ++count;
                }

                return count;
            }
        }

        public bool IsNotEmpty => Count > 0;

        public void Insert(Order order)
        {
            Node newNode = new Node();
            newNode.Value = order;

            if (_head == null)
            {
                _head = newNode;
            }
            else
            {
                Node lastNode = GetLastNode();

                lastNode.Next = newNode;
            }
        }

        public void SortedInsert(Order order, Func<Order, Order, bool> sorter)
        {
            Node newNode = new Node();
            newNode.Value = order;

            if (_head == null)
            {
                _head = newNode;
                return;
            }

            Node previous = null;
            Node current = _head;

            while (current != null && !sorter(newNode.Value, current.Value))
            {
                previous = current;
                current = current.Next;
            }

            if (previous == null)
            {
                newNode.Next = _head;
                _head = newNode;
                return;
            }

            newNode.Next = current;
            previous.Next = newNode;
        }

        public Order Get(int index)
        {
            int i = 0;
            foreach (var order in this)
            {
                if (i++ == index)
                {
                    return order;
                }
            }

            return null;
        }

        public void Remove(Order order)
        {
            Node temp = _head;
            Node slow = null;

            while (temp != null && temp.Value != order)
            {
                slow = temp;
                temp = temp.Next;
            }

            if (temp == null)
            {
                return;
            }

            if (slow == null)
            {
                _head = _head.Next;
            }
            else
            {
                slow.Next = temp.Next;
            }
        }

        public int Sum(Func<Order, int> valueRetriever)
        {
            int sum = 0;

            foreach (var order in this)
            {
                sum += valueRetriever(order);
            }

            return sum;
        }

        public bool Contains(Order searchedOrder)
        {
            foreach (var order in this)
            {
                if (order == searchedOrder)
                {
                    return true;
                }
            }

            return false;
        }

        public LinkedListOrderStore Sorted(Func<Order, Order, bool> sorter)
        {
            LinkedListOrderStore sorted = new LinkedListOrderStore();

            foreach (var order in this)
            {
                sorted.SortedInsert(order, sorter);
            }

            return sorted;
        }

        IOrderStore IOrderStore.Sorted(Func<Order, Order, bool> sorter)
        {
            return this.Sorted(sorter);
        }

        public IEnumerator<Order> GetEnumerator()
        {
            Node temp = _head;

            while (temp != null)
            {
                yield return temp.Value;

                temp = temp.Next;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private Node GetLastNode()
        {
            Node temp = _head;

            while (temp.Next != null)
            {
                temp = temp.Next;
            }

            return temp;
        }
    }
}
