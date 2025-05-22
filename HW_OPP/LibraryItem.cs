using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW_OPP
{
    public abstract class LibraryItem
    {
        public int id {  get; set; }
        public string Title {  get; set; }
        public int PubishYear { get; set; }
         protected LibraryItem(int id, string title, int pubishYear)
        {
            this.id = id;
            this.Title = title;
            this.PubishYear = pubishYear;
        }

        public abstract void  DisplayInfo();
        public virtual decimal CalculateLateReturnFee(int dateDays)
        {
            return dateDays * 0.5m ;
        }

    }
}
