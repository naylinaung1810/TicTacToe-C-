using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TicTacToe
{
    public partial class new_Game : Form
    {
        public new_Game()
        {
            InitializeComponent();
        }
        public String clicked;

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            clicked = "Cancel";
            this.Close();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            clicked = "OK";
            this.Close();
        }
    }
}
