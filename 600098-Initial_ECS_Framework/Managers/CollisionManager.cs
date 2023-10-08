using OpenGL_Game.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenGL_Game.Objects;
using OpenGL_Game.Scenes;
using OpenTK;

namespace OpenGL_Game.Managers
{
    class CollisionManager
    {
        const ComponentTypes MASK = (ComponentTypes.COMPONENT_COLLISION_SPHERE | ComponentTypes.COMPONENT_POSITION);

        public CollisionManager() { }

        

        public bool SphereCollision(ComponentCollisionSphere object1C, ComponentCollisionSphere object2C, ComponentPosition object1P, ComponentPosition object2P)
        {
            if (object1C.Collision && object2C.Collision)
            {
                if ((object1P.Position - object2P.Position).Length < object1C.Radius + object2C.Radius)
                {
                    return true;
                }
            }
            
            return false;
        }

        public bool LineCollision(ComponentPosition position, ComponentCollisionLine collision, Vector3 previousPos)
        {
            Tuple<Vector2, Vector2> playerPosition = new Tuple<Vector2, Vector2>(position.Position.Xz, previousPos.Xz);
            float V1DotN1 = Vector2.Dot(playerPosition.Item1, collision.Points.Item1);
            float V2DotN1 = Vector2.Dot(playerPosition.Item2, collision.Points.Item1);
            
            if(V1DotN1 * V2DotN1 < 0)
            {
                Vector2 playerMovementVector = Vector2.Subtract(playerPosition.Item2, playerPosition.Item1);
                Vector2 playerNormal = new Vector2(-playerMovementVector.Y, playerMovementVector.X);
                //If problem try switching the normal and vector
                float V3DotN2 = Vector2.Dot(playerNormal, Vector2.Subtract(collision.Points.Item1, playerPosition.Item1));
                float V4DotN2 = Vector2.Dot(playerNormal, Vector2.Subtract(collision.Points.Item2, playerPosition.Item1));
                Console.WriteLine("Collide");
                if(V3DotN2 * V4DotN2 < 0)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
