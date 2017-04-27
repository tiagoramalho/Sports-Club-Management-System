using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CluSys
{
    class Athletes
    {
    }

    /// TEMPLATES/EXAMPLES
    public class Customer
    {
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String Address { get; set; }

        public Customer(String firstName, String lastName, String address)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Address = address;
        }

    }

    public class Customers : ObservableCollection<Customer>
    {
        public Customers()
        {
            Add(new Customer("Michael", "Anderberg",
                    "12 North Third Street, Apartment 45"));
            Add(new Customer("Chris", "Ashton",
                    "34 West Fifth Street, Apartment 67"));
            Add(new Customer("Cassie", "Hicks",
                    "56 East Seventh Street, Apartment 89"));
            Add(new Customer("Guido", "Pica",
                    "78 South Ninth Street, Apartment 10"));
        }
    }
}
