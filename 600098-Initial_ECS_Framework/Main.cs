using System;
using System.Windows.Forms;
using OpenGL_Game.Managers;

namespace OpenGL_Game
{
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class MainEntry
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]

        static void Main()
        {
           
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainMenu());

            if(menu == true)
            {
                Application.Restart();
            }
        }

        public static bool menu = false;

      //  public static void MainMenu()
       // {
        //    Application.Restart();

        //}
    }
#endif
}
