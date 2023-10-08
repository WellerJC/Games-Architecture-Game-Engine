using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenGL_Game.Managers;
using OpenGL_Game.Scenes;

namespace OpenGL_Game.Menus
{
    public partial class GameLost : Form
    {
        public GameLost()
        {
            InitializeComponent();
        }

        private void GameLost_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            GameScene.closegame = true;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            MainEntry.menu = true;
            GameScene.closegame = true;
        }
    }
}
