using OpenGL_Game.Managers;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGL_Game.Scenes
{
    class GameWinScene : Scene
    {
        public GameWinScene(SceneManager sceneManager) : base(sceneManager)
        {
            // Set the title of the window
            sceneManager.Title = "Game Won!";
            // Set the Render and Update delegates to the Update and Render methods of this class
            sceneManager.renderer = Render;
            sceneManager.updater = Update;
            sceneManager.keyboardDownDelegate += Keyboard_KeyDown;
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

            GUI.clearColour = Color.LimeGreen;

            //Display the Title
            float width = sceneManager.Width, height = sceneManager.Height, fontSize = Math.Min(width, height) / 10f;
            GUI.Label(new Rectangle(0, (int)(fontSize / 2f), (int)width, (int)(fontSize * 2f)), "CONGRATULATIONS!", (int)fontSize, StringAlignment.Center);
            GUI.Label(new Rectangle(0, 200, (int)width, (int)(fontSize * 2f)), "YOU DEFEATED ALL OF THE DRONES", 50, StringAlignment.Center);
            GUI.Label(new Rectangle(0, 400, (int)width, (int)(fontSize * 2f)), "(PRESS ANY BUTTON TO RETURN TO THE MAIN MENU)", 10, StringAlignment.Center);
            GUI.Render();
        }
        public void Keyboard_KeyDown(KeyboardKeyEventArgs e)

        {

            sceneManager.ChangeScene(SceneTypes.SCENE_GAMEWIN);
        }

        public override void Close()
        {
            sceneManager.keyboardDownDelegate += Keyboard_KeyDown;
        }
    }
}
