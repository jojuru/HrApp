using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ohj2_Projekti
{
    public class Employment
    {
        public string name { get; set; }
        public string unit { get; set; }
        public DateTime begin { get; set; }
        public DateTime end { get; set; }
        public Boolean endset { get; set; }


        public Employment() { }
        public Employment(string name, string unit, DateTime begin, DateTime end, Boolean endset)
        {
            this.name = name;
            this.begin = begin;
            this.end = end;
            this.unit = unit;
            this.endset = endset;
        }
    }
}
