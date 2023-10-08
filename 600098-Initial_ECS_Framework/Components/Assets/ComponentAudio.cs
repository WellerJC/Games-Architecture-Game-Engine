using OpenGL_Game.Managers;
using OpenTK;
using OpenTK.Audio.OpenAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGL_Game.Components
{
    class ComponentAudio : IComponent
    {
        Vector3 sourcePosition;
        int audioSource;
        int audioBuffer;

        public ComponentAudio(float x, float y, float z, string audioName)
        {
            audioBuffer = ResourceManager.LoadAudio(audioName);
            audioSource = AL.GenSource();
            AL.Source(audioSource, ALSourcei.Buffer, audioBuffer); // attach the buffer to a source
            AL.Source(audioSource, ALSourceb.Looping, true); // source loops infinitely
            sourcePosition = new Vector3(x, y, z);
            AL.Source(audioSource, ALSource3f.Position, ref sourcePosition);
            AL.SourcePlay(audioSource); // play the ausio source
        }

        public void SetPosition(Vector3 emitterPosition)
        {
            AL.Source(audioSource, ALSource3f.Position, ref emitterPosition);
        }

        public int AudioSource
        {
            get { return audioSource; }
            set { audioSource = value; }
        }
        public ComponentTypes ComponentType
        {
            get { return ComponentTypes.COMPONENT_AUDIO; }
        }

    }
}
