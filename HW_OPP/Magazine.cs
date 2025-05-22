using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW_OPP
{
    internal class Magazine : LibraryItem
    {
        public int IssueNumber { get; set; }
        public string Publisher { get; set; }
        public Magazine(int id, string title, int pubishYear , int issueNumber) : base(id, title, pubishYear)
        {
            this.IssueNumber = issueNumber;
        }
        public override void DisplayInfo()
        {
            Console.WriteLine($"ID : {id} Title : {Title} pubishYear : {PubishYear} issueNumber : {IssueNumber} Publisher : {Publisher} ");

        }
    }
}
