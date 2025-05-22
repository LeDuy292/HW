
namespace HW_OPP
{
    internal class LibraryItemCollection<T>
    {
        List<T> Items;
        public LibraryItemCollection()
        {
            Items = new List<T>();
        }
        public int Count => Items.Count;
        internal void Add(T item)
        {
            Items.Add(item);
        }
    }
}