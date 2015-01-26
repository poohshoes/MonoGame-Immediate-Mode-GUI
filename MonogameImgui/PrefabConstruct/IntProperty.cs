using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonogameImgui.PrefabConstruct
{
    class IntProperty : Property
    {
        public int Value;

        public IntProperty(string name, int startingValue)
            : base(name)
        {
            Value = startingValue;
        }
    }
}
