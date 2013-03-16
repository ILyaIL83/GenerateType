using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenerateType
{
    public interface IPerson : IBaseData
    {
        string FirstName { get; set; }
        string LastName { get; set; }
        decimal Amount { get; set; }
    }

    public interface IBaseData
    {
        int Id { get; set; }
    }

    public class BaseDataClass : IBaseData
    {
        public int Id { get; set; }
    }
}
