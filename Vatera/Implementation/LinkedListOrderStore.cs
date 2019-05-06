using Vatera.Interfaces;
using Vatera.Models;

namespace Vatera.Implementation
{
    class LinkedListOrderStore: IOrderStore
    {
        internal class Node
        {
            public Order Value;
            public Node Previous;
            public Node Next;
        }

        private Node _head;

        public int Count
        {
            get
            {
                int count = 0;

                Node temp = _head;

                while (temp != null)
                {
                    count++;

                    temp = temp.Next;
                }

                return count;
            }
        }

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
                newNode.Previous = lastNode;
            }
        }

        public Order[][] GroupByRating()
        {
            Order[][] orders = new Order[6][];

            for (int i = 1; i <= 5; i++)
            {
                orders[i] = FilterByRating(i);
            }

            return orders;
        }

        private Order[] FilterByRating(int rating)
        {
            int count = 0;
            Node temp = _head;

            while (temp != null)
            {
                if (temp.Value.Customer.Rating == rating)
                {
                    count++;
                }

                temp = temp.Next;
            }

            Order[] orders = new Order[count];

            count = 0;
            temp = _head;

            while (temp != null)
            {
                if (temp.Value.Customer.Rating == rating)
                {
                    orders[count++] = temp.Value;
                }

                temp = temp.Next;
            }

            return orders;
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
