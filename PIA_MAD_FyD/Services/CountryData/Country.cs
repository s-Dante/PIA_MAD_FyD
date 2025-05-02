using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIA_MAD_FyD.Services.CountryData
{
    public class Country
    {
        public int id { get; set; }
        public string name { get; set; }
        public List<State> states { get; set; }
    }

    public class State
    {
        public int id { get; set; }
        public string name { get; set; }
        public List<City> cities { get; set; }
    }

    public class City
    {
        public int id { get; set; }
        public string name { get; set; }
    }

}
