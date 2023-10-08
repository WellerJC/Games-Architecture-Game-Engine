using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenGL_Game.Managers;
using OpenGL_Game.Scenes;
using OpenTK;
using OpenTK.Audio.OpenAL;

namespace OpenGL_Game
{
    public partial class MainMenu : Form
    {
        
        SoundPlayer music;
        public MainMenu()
        {
            music = new SoundPlayer("Audio/menu_music.wav");
            music.PlayLooping();
            InitializeComponent();
        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        private void button1_Click(object sender, EventArgs e)
        {
            music.Stop();
            Close();
            using (var game = new SceneManager())
            {

                game.VSync = OpenTK.VSyncMode.Off;
                game.Run();

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OptionsMenu op = new OptionsMenu();
            op.Show();
           

        }
        private void button3_Click(object sender, EventArgs e)
        {            
            Close();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
