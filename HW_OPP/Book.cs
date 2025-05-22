
using HW_OPP;

namespace HW_OPP
{
    public class Book :  LibraryItem , IBorrowable
    {
        public string Author { get; set; }
        public int Pages { get; set; }
        public string Genre { get; set; }
        public DateTime? BorrowDate { get ; set ; }
        public DateTime? ReturnDate { get ; set ; }
        public bool IsAvailable { get; set; } = true;

        public Book(int id, string title, int pubishYear , string author   ) : base(id, title, pubishYear)
        {
            this.Author = author;
            this.Pages = Pages;
        }


        public void Borrow()
        {
            if (IsAvailable)
            {
                BorrowDate = DateTime.Now;
                IsAvailable = false;
                Console.WriteLine($"Book ID :{id} BorrowDate : {BorrowDate}");
            }
            else
            {
                Console.WriteLine($"Book  ID :{id} has been borrowed");
            }
        }

        public override void DisplayInfo()
        {
            Console.WriteLine($"ID : {id} Title : {Title} pubishYear : {PubishYear} Author : {Author} Pages : {Pages} Genre : {Genre}");
        }

        public void Return()
        {
            ReturnDate = DateTime.Now;
            IsAvailable = true;
            Console.WriteLine($"BookID :{id} ReturnDate  : {BorrowDate}");
        }

        public override decimal CalculateLateReturnFee(int dateDays)
        {
            return dateDays * 0.75m;
        }
        public override string? ToString()
        {
            return base.ToString();
        }
    }
}