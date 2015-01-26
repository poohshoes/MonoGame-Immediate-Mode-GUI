using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonogameImgui.PrefabConstruct
{
    class PositionComponent : Component
    {
        public float X;
        public float Y;

        public PositionComponent(string displayName, float x, float y)
            : base(displayName)
        {
            X = x;
            Y = y;
        }

        public override List<Property> GetProperties()
        {
            return new List<Property>(){
                new FloatProperty("X", X),
                new FloatProperty("Y", Y)
                };
        }
    }
}
