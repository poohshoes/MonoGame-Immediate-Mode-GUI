using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonogameImgui.PrefabConstruct
{
    abstract class Component
    {
        public string DisplayName;

        public Component(string displayName)
        {
            DisplayName = displayName;
        }

        public abstract List<Property> GetProperties();
    }
}
