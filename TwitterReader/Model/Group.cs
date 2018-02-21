using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitterReader
{
    public class Group
    {
        public const string DefaultGroup = "Default";

        public Group()
        {
            ScreenNameList = new List<string>();
        }

        public string Name { get; set; }
        public List<string> ScreenNameList { get; set; }
    }
}
