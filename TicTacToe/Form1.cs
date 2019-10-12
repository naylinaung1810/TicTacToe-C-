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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            button1.Visible = false;
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            int count = 0;
            panel1.Size = new Size(panel1.Size.Width - 2, panel1.Size.Height);
            panel2.Size = new Size(panel2.Size.Width - 2, panel2.Size.Height);
            panel3.Size = new Size(panel3.Size.Width - 2, panel3.Size.Height);
            if(panel1.Size.Width<=0)
            {
                button1.Visible = true;
                    timer1.Stop();
               
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            new Form3().ShowDialog();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
