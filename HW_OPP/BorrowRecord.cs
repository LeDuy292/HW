
namespace HW_OPP
{
    internal record BorrowRecord
    {

        public int Id { get; init; }
        public string Title { get; init; }
        public DateTime BorrowDate { get; init; }
        public DateTime? ReturnDate { get; init; }
        public string BorrowerName { get; init; }
        public BorrowRecord(int id, string title, DateTime borrowdate, DateTime? returndate, string name)
        {
            this.Id = id;
            this.Title = title;
            this.BorrowDate = borrowdate;
            this.ReturnDate = returndate;
            this.BorrowerName = name;
        }

 

        public string? LibraryLocation { get; init; }

    }
}

    