using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGL_Game.Components
{
    class ComponentCollisionSphere : IComponent
    {
        bool collision;
        float radius;

        public ComponentCollisionSphere(bool isCollidable, float collisionRadius) 
        {
            collision = isCollidable;
            radius = collisionRadius;
        }
        public ComponentTypes ComponentType
        {
            get { return ComponentTypes.COMPONENT_COLLISION_SPHERE; }
        }

        public bool Collision
        {
            get { return collision; }
            set { collision = value; }
        }

        public float Radius
        {
            get { return radius; }
            set { radius = value; }
        }
    }
}
