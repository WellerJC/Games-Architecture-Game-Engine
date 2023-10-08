using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenGL_Game.Components;
using OpenGL_Game.Objects;

namespace OpenGL_Game.Systems
{
    class SystemCollision : ISystem
    {
        const ComponentTypes MASK = (ComponentTypes.COMPONENT_COLLISION_MESH | ComponentTypes.COMPONENT_POSITION);

        public string Name
        {
            get { return "SystemCollision"; }
        }

        public void OnAction(Entity entity)
        {
            if((entity.Mask & MASK) == MASK)
            {

            }
        }
    }
}
