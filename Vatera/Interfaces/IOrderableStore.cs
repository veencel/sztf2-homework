namespace Vatera.Interfaces
{
    interface IOrderableStore
    {
        int Count { get; }

        void Insert(IOrderable orderable);

        IOrderable BinarySearch(string Identifier);

        IOrderable SearchByName(string Name);

        void Remove(string identifier);

        IOrderable[] ToArray();
    }
}
