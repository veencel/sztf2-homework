namespace Vatera.Interfaces
{
    interface IOrderableStore
    {
        void Insert(IOrderable orderable);

        IOrderable BinarySearch(string Identifier);

        IOrderable SearchByName(string Name);

        void Remove(string identifier);

        IOrderable[] ToArray();
    }
}
