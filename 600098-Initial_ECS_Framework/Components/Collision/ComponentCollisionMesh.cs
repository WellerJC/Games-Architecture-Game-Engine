using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenGL_Game.Level;
namespace OpenGL_Game.Components
{
    class ComponentCollisionMesh : IComponent
    {
        List<ComponentCollisionLine> collisions = new List<ComponentCollisionLine>();

        public ComponentCollisionMesh(cubeMap map)
        {
            collisions = map.collisionMesh;
        }
        public List<ComponentCollisionLine> Collisions
        {
            get { return collisions; }
            set { collisions = value; }
        }
        public ComponentTypes ComponentType
        {
            get { return ComponentTypes.COMPONENT_COLLISION_MESH; }
        }
    }
}
