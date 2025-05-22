using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW_OPP
{
    internal class Library
    {
        public List<LibraryItem> Items = new List<LibraryItem>();
        public Library() {
        }
        public void AddItem(LibraryItem item)
        {
            Items.Add(item);
        }

        public void DisplayAllItems()
        {
            foreach (LibraryItem item in Items)
            {
                item.DisplayInfo();
            }
        }

        internal LibraryItem SearchByTitle(string v)
        {
            Console.WriteLine($"Search By Title : {v}");
            foreach (LibraryItem item in Items) { 
                if(item.Title.Equals(v, StringComparison.OrdinalIgnoreCase)) return item;
            }
            return null; 
        }
        public LibraryItem item ;

        public ref LibraryItem GetItemReference(int id)
        {
            for (int i = 0; i < Items.Count; i++)
            {
                if (Items[i].id == id)
                {
                    item = Items[i];
                    return ref item;
                }
            }
            throw new KeyNotFoundException($"Item with id {id} not found.");
        }

        internal bool UpdateItemTitle(int v, ref string title)
        {
            throw new NotImplementedException();
        }
    }
}
