using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonogameImgui.PrefabConstruct
{
    class ButtsComponent : Component
    {
        public int NumberOfButts;
        public string TextField;

        public ButtsComponent(string displayName)
            : base(displayName)
        {

        }

        public override List<Property> GetProperties()
        {
            return new List<Property>(){
                new IntProperty("#", NumberOfButts),
                new StringProperty("Butts Name", TextField)
                };
        }
    }
}
