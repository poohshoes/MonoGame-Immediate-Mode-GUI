using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonogameImgui.PrefabConstruct
{
    class StringProperty : Property
    {
        public string Value;

        public StringProperty(string name, string startingValue)
            : base(name)
        {
            Value = startingValue;
        }
    }
}
