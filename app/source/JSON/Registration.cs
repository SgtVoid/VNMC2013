using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VNMC2013.JSON.Registration
{
    public class Result
    {
        public string IkGaMeeOpHetVNMCValue { get; set; }
        public int? EersteKeuzeActiviteitOpZondagWijzigenVanJeUiteindelijkeKeuzeIsMogelijkTotBeginOktoberId { get; set; }
        public int CreatedById { get; set; }
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
