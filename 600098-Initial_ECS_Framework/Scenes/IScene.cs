using OpenTK;

namespace OpenGL_Game.Scenes
{
    interface IScene
    {
        void Render(FrameEventArgs e);
        void Update(FrameEventArgs e);
        void Close();

       
    }

    enum SceneTypes
    {
        SCENE_NONE,
        SCENE_GAME,
        SCENE_MENU,
        SCENE_GAMEOVER,
        SCENE_GAMEWIN
    }
}
