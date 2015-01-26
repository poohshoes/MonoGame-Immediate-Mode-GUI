using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonogameImgui.PrefabConstruct
{
    class Prefab
    {
        public string Name;
        public string Category;
        public List<Component> Components = new List<Component>();

        public Prefab(string name)
        {
            Name = name;
        }
    }
}
