using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonogameImgui.PrefabConstruct
{
    abstract class Property
    {
        public string Name;

        public Property(string name)
        {
            Name = name;
        }
    }
}
