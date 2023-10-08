using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGL_Game.Components
{
    class ComponentCollisionLine : IComponent
    {
        Vector2 normal;
        Tuple<Vector2, Vector2> points;
        
        public ComponentCollisionLine(Vector2 pointA, Vector2 pointB)
        {          
            points = new Tuple<Vector2,Vector2>(pointA,pointB);
            Vector2 line = this.points.Item2 - this.points.Item1;
            normal = new Vector2(line.Y, -line.X);          
            
        }

        public ComponentCollisionLine(Vector2 pointA, Vector2 pointB, Vector2 normal)
        {
            points = new Tuple<Vector2, Vector2>(pointA, pointB);
            this.normal = normal;
        }

        public Vector2 Normal
        {
            get { return normal; }
            set { normal = value; }
        }
        public Tuple<Vector2,Vector2> Points
        {
            get { return points; }
            set { points = value; }
        }
        public ComponentTypes ComponentType
        {
            get { return ComponentTypes.COMPONENT_COLLISION_LINE; }
        }
    }
}
