using System;

namespace OpenGL_Game.Components
{
    [FlagsAttribute]
    enum ComponentTypes {
        COMPONENT_NONE     = 0,
	    COMPONENT_POSITION = 1 << 0,
        COMPONENT_GEOMETRY = 1 << 1,
        COMPONENT_TEXTURE  = 1 << 2,
        COMPONENT_VELOCITY = 2 << 2,
        COMPONENT_SHADER = 2 << 3,
        COMPONENT_AUDIO = 3 << 3,
        COMPONENT_COLLISION_SPHERE = 3 << 4,
        COMPONENT_COLLISION_MESH = 4 << 4,
        COMPONENT_COLLISION_LINE = 4 << 5,
    }

    interface IComponent
    {
        ComponentTypes ComponentType
        {
            get;
        }
    }
}
