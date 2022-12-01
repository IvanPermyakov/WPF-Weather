using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface_Weather.Model
{
    public class Temperature
    {
        public float morn { get; set; }
        public float day { get; set; }
        public float eve { get; set; }
        public float night { get; set; }
    }
    public class fallout
    {
        public int id { get; set; }
    }
    public class Day
    {
        public int dt { get; set; }
        public Temperature temp { get; set; }
        public List<fallout> weather { get; set; }
    }
    public class Weather
    {
        public virtual List<Day> daily { get; set; }
    }
}
