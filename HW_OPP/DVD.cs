using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW_OPP
{
    internal class DVD : LibraryItem , IBorrowable
    {
      
        public string Director {  get; set; }
        public int Runtime { get; set; }
        public string AgeRating { get; set; }
        public DateTime? BorrowDate { get ; set ; }
        public DateTime? ReturnDate { get ; set; }
        public bool IsAvailable { get; set; } = true;

        public DVD(int id, string title, int pubishYear , string Director) : base(id, title, pubishYear)
        {
            this.Director = Director;
        }

        public override void DisplayInfo()
        {
            Console.WriteLine($"ID : {id} Title : {Title} pubishYear : {PubishYear} Runtime : {Runtime} AgeRating : {AgeRating} Director : {Director}");
        }

        public override string? ToString()
        {
            return base.ToString();
        }

        public override decimal CalculateLateReturnFee(int dateDays)
        {
            return dateDays * 1.00m;
        }

        public void Borrow()
        {
            if (IsAvailable)
            {
                BorrowDate = DateTime.Now;
                IsAvailable = false;
                Console.WriteLine($"DVD ID :{id} BorrowDate : {BorrowDate}");
            }
            else
            {
                Console.WriteLine($"DVD ID :{id} has been borrowed");
            }
        }

        public void Return()
        {
            ReturnDate = DateTime.Now;
            IsAvailable = true;
            Console.WriteLine($"DVD ID :{id} ReturnDate  : {BorrowDate}");
        }
    }
}
