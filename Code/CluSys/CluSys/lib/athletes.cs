using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CluSys
{
    /// TEMPLATES/EXAMPLES
    public class Athelete
    {
        public Athelete(String firstName, String lastName, String modality)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Modality = modality;
        }

        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String Modality { get; set; }
    }

    public class Modality
    {
        public Modality(String name)
        {
            this.Name = name;
        }

        public String Name { get; set; }
    }

    public class OpenAthletes : ObservableCollection<Athelete>
    {
        public OpenAthletes()
        {
            Add(new Athelete("Michael", "Anderberg", "12 North Third Street"));
            Add(new Athelete("Chris", "Ashton", "34 West Fifth Street"));
            Add(new Athelete("Cassie", "Hicks", "56 East Seventh Street"));
            Add(new Athelete("Guido", "Pica", "78 South Ninth Street"));
        }
    }

    public class Favorites : ObservableCollection<Athelete>
    {
        public Favorites()
        {
            Add(new Athelete("Michael", "Anderberg", "12 North Third Street"));
            Add(new Athelete("Chris", "Ashton", "34 West Fifth Street"));
            Add(new Athelete("Cassie", "Hicks", "56 East Seventh Street"));
            Add(new Athelete("Guido", "Pica", "78 South Ninth Street"));
        }
    }

    public class Modalities : ObservableCollection<Modality>
    {
        public Modalities()
        {
            Add(new Modality("Futeball"));
            Add(new Modality("Basketball"));
            Add(new Modality("Volleyball"));
            Add(new Modality("Swimming"));
        }
    }
}
