using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace WpfXmlClrNameSpaces2015
{
    public class XmlClrNameSpaceItem
    {
        public string assemName { get; set; }
        public string xmlNs { get; set; }
        public string clrNs { get; set; }

        private Assembly _assem;
        public Assembly assem
        {
            get { return _assem; }
            set
            {
                _assem = value;
                assemName = _assem.GetName().Name;
            }
        }

    }
}
