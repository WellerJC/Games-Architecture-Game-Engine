using OpenGL_Game.Components;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGL_Game.Objects
{
    class Drone : Entity
    {
        string name;
        public Drone(string name) : base(name)
        {
            this.name = name;
        }
    }
}
