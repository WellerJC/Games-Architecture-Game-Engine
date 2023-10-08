using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using OpenTK.Input;
using OpenTK.Audio.OpenAL;
using OpenGL_Game.Managers;

namespace OpenGL_Game.Scenes
{
    class MainMenuScene : Scene
    {
        int audioSource;

        bool keybind;
        public MainMenuScene(SceneManager sceneManager) : base(sceneManager)
        {
            // Set the title of the window
            sceneManager.Title = "Main Menu";
            // Set the Render and Update delegates to the Update and Render methods of this class
            sceneManager.renderer = Render;
            sceneManager.updater = Update;

            sceneManager.mouseDelegate += Mouse_BottonPressed;

            int audioBuffer = ResourceManager.LoadAudio("Audio/menu_music.wav"); audioSource = AL.GenSource(); AL.Source(audioSource, ALSourcei.Buffer, audioBuffer); 
            AL.Source(audioSource, ALSourceb.Looping, true); Vector3 sourcePosition = new Vector3(0, 0, 0); AL.Source(audioSource, ALSource3f.Position, ref sourcePosition);
            AL.SourcePlay(audioSource); // play the ausio source
            
        }

        public override void Update(FrameEventArgs e)
        {
        }

        public override void Render(FrameEventArgs e)
        {
            GL.Viewport(0, 0, sceneManager.Width, sceneManager.Height);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0, sceneManager.Width, 0, sceneManager.Height, -1, 1);

            GUI.clearColour = Color.CornflowerBlue;

            //Display the Title
            float width = sceneManager.Width, height = sceneManager.Height, fontSize = Math.Min(width, height) / 10f;
            GUI.Label(new Rectangle(0, (int)(fontSize / 2f), (int)width, (int)(fontSize * 2f)), "Blastoid", (int)fontSize, StringAlignment.Center);

            

            GUI.Render();
        }

        public void Mouse_BottonPressed(MouseButtonEventArgs e)
        {
            if (keybind == false)
            {
                switch (e.Button)
                {
                    case MouseButton.Left:
                        keybind = true;
                        AL.SourcePause(audioSource);
                        sceneManager.ChangeScene(SceneTypes.SCENE_MENU);
                        break;
                }
            }
        }

        public override void Close()
        {
            sceneManager.mouseDelegate -= Mouse_BottonPressed;
        }
    }
}