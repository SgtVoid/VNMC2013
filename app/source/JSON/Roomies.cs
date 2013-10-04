using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VNMC2013.JSON.Roomies
{
    public class Result
    {
        public int? WieId { get; set; }
        public string Kamer { get; set; }
    }

    public class D
    {
        public List<Result> results { get; set; }
    }

    public class RootObject
    {
        public D d { get; set; }
    }
}
