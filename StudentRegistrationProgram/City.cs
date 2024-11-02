
using Microsoft.EntityFrameworkCore;

namespace StudentRegistrationProgram
{
    //Index: Makes lookup more performant, the database can perform a binary search on the ordered index.
    //Instead of examining all rows, it only needs to look through a fraction of them
    //logarithmic?

    [Index(nameof(Name))]
    public class City
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string County { get; set; } // Län
        public string Municipality { get; set; } // Kommun
    }
}
