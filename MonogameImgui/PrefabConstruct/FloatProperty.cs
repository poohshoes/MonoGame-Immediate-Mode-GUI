using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonogameImgui.PrefabConstruct
{
    class FloatProperty : Property
    {
        public float Value;

        public FloatProperty(string name, float startingValue)
            : base(name)
        {
            Value = startingValue;
        }
    }
}
