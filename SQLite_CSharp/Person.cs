using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLite_CSharp
{
    class Person
    {
        public int Id;
        public string Name;
        public int Age;

        public Person()
            : this(0, "", 0)
        { }

        public Person(int aID)
            : this(aID, "", 0)
        { }

        public Person(int aID, string aName, int aAge)
        {
            Id = aID;
            Name = aName;
            Age = aAge;
        }
    }
}
