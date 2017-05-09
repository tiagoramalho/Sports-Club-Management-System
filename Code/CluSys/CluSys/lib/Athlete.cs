using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CluSys.lib
{
    [Serializable()]
    class Athlete
    {
        public string CC { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public DateTime Birthdate { get; set; }
        public string Photo { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Job { get; set; }
        public string DominantSide { get; set; }
        public string ModalityId { get; set; }
    }
}
