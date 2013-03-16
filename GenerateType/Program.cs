using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenerateType
{
    class Program
    {
        static void Main(string[] args)
        {
            var props = typeof(IPerson).GetProperties();

            var nType = ClassBuilder.Build(typeof(IPerson));
            var nObj = Activator.CreateInstance(nType) as IPerson;
            nObj.Id = 1;
            nObj.Amount = 23;
            nObj.LastName = "334asdf";
        }
    }
}
