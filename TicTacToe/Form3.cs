using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TicTacToe
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
            this.Size = new Size(475, 510);
        }

         Class1 user=new Class1();
        public static String selectName = "";
      public static  String category = "";
       
        
        //Start btn............
        private void button1_Click(object sender, EventArgs e)
        {
            clearBtn();
           
            if (listBox1.SelectedItem!=null)
            {
                playbtn_panel.Enabled = false;
                radioButton2.Enabled = false;
                radioButton1.Enabled = false;
                selectName = listBox1.SelectedItem.ToString();
                panel1.Visible = false;
                panel2.Visible = false;
                panel4.Visible = true;
                panel5.Visible = true;
              

                int port = user.getPort(selectName);
                String ip_add = user._getIPBYName(selectName);
                user._Delete(selectName);
                category = "c";
                _Client(port,ip_add);
            }
            else
                label15.Text = "You didn't select player";
        }
        
        private void Form3_Load(object sender, EventArgs e)
        {
            textView.Visible = false;
            panel1.Location = new Point(1, 106);//for player list....
            panel1.Size = new Size(262, 363);
            panel1.Visible = false;
            panel2.Location = new Point(1, 106);//for start page......
            panel2.Size = new Size(420, 358);
            panel2.Visible = true;
            panel5.Size = new Size(262, 363);//for play room......
            panel5.Location = new Point(1, 106);
            panel5.Visible = false;
            panel4.Size = new Size(200, 363);//for score.....
            panel4.Location = new Point(264, 106);
            panel4.Visible = false;
            panel6.Location = new Point(1, 106);//for player list....
            panel6.Size = new Size(262, 363);
            panel6.Visible = false;
          //  pictureBox13.Visible = false;
            //label19.Visible = false;
            label20.Visible = false;
           // panel8.Visible = false;

            getListInListBox();
           
           
        }
        MySqlConnection mycon;
        int count = 0;
        public void getListInListBox()
        {
            try
            {
                    mycon = new MySqlConnection("username=naylin;password=nay;datasource=192.168.43.141;database=tictactoe;");
               
                String sql = "select * from users";
                MySqlCommand cmd = new MySqlCommand(sql, mycon);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                int check = dt.Rows.Count;
                for (int i = 0; i < check; i++)
                {
                    listBox1.Invoke((MethodInvoker)delegate()
                    {
                        String btn = dt.Rows[i]["name"].ToString();
                        listBox1.Items.Add(btn);
                    });
                }
                mycon.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Server Time Out......getList");
            }
        }


        public static String name = "";

        //Connect Button......
        private void button3_Click(object sender, EventArgs e)
        {
            if(textBox1.Text!="")
            {
                listBox1.Items.Clear();
                getListInListBox();
                label9.Text = textBox1.Text;
                panel2.Visible = false;
                panel1.Visible = true;
                panel4.Visible = true;
                panel5.Visible = false;

            }else
            {
                label11.Text = "You should enter your name...";
            }
    
        }
       
        //Host Button............
        private void button2_Click(object sender, EventArgs e)
        {
            if(textBox1.Text!="")
            {
                label9.Text = textBox1.Text;
                panel2.Visible = false;
                panel5.Visible = false;//play room.......
                panel4.Visible = true;
                panel1.Visible = false;
                panel6.Visible = true;
                button1.Enabled = false;
                label15.Visible = false;
                user._Insert(textBox1.Text);
                int port = user.getPort(textBox1.Text);//get port..........
                user._Update(textBox1.Text, port);
                name = textBox1.Text;
                category = "s";

                _Servrer(port);//for Server Start.........

            }else
            {
                label11.Text = "You should enter your name...";
            }
          
        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            //for(int i=0;i<listBox1.Items.Count;i++)
            //listBox1.Items.RemoveAt(i);
            getListInListBox();
        }

        private void C3_Click(object sender, EventArgs e)
        {
            String sign = "";
            radioButton1.Enabled = false;
            radioButton2.Enabled = false;
            Button button = (Button)sender;
            byte[] sendData = Encoding.ASCII.GetBytes(button.Name.ToString());
            if(category=="c")
            {
                new_socket.Send(sendData, 0, sendData.Length, 0);//error
            }else
            {
                new_client.Send(sendData, 0, sendData.Length, 0);//error
            }
           
            if(radioButton1.Checked)
            {
                sign = "X";
            }else
            {
                sign = "O";
            }
            button.Text = sign;
            button.BackColor = Color.LawnGreen;
            button.Enabled = false;
            count++;
            playbtn_panel.Enabled = false;
            checkForWinner();
        }


        Socket new_server, new_client;
        Boolean check = true;
        Boolean flag = true;
        Boolean server_check = true;
        public void _Servrer(int port)
        {
            try
            {
                new_server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, port);
                new_server.Bind(endPoint);
                new_server.Listen(100);
                clearBtn();
                new Thread(delegate()
                {
                    if (server_check)
                    {
                            new_client = new_server.Accept();
                       
                        if (check)
                            check = false;
                        {
                            Invoke((MethodInvoker)delegate()
                               {
                                   panel5.Visible = true;
                                   panel6.Visible = false;
                               });
                        }
                        while (flag)
                        {
                            try
                            {
                                byte[] buffer = new byte[1024];
                                int received = new_client.Receive(buffer, 0, buffer.Length, 0);
                                Array.Resize(ref buffer, received);

                                Invoke((MethodInvoker)delegate()
                                {
                                    String btnId = Encoding.ASCII.GetString(buffer);
                                    if (btnId == "new")
                                    {
                                        new_Game msgBox = new new_Game();
                                        msgBox.ShowDialog();
                                        if (msgBox.clicked == "OK")
                                        {
                                            clearBtn();
                                            sentForNew(msgBox.clicked);
                                        }
                                        else
                                        {
                                            sentForNew(msgBox.clicked);
                                        }
                                    }
                                    else if (btnId == "A1" || btnId == "A2" || btnId == "A3" || btnId == "B1" || btnId == "B2" || btnId == "B3" || btnId == "C1" || btnId == "C2" || btnId == "C3")
                                    {
                                        receiveData(btnId);
                                        checkForWinner();
                                        playbtn_panel.Enabled = true;
                                    }
                                    else if (btnId == "OK")
                                    {
                                        clearBtn();
                                    }
                                    else if (btnId == "Cancel")
                                    {
                                        MessageBox.Show("Other player rejected...", "Reject", MessageBoxButtons.OK);
                                    }
                                    else
                                    {
                                        textView.Visible = true;
                                        listBox2.Items.Add("Fri :" + btnId);
                                        timer2.Start();
                                    }


                                });
                            }
                            catch (Exception ex)
                            {
                                server_check = false;
                                flag = false;
                                MessageBox.Show("Client is Closed.....");
                                }
                        }
                    }
                    
                }).Start();
            }catch(Exception ex)
            {
                MessageBox.Show("Client is closed...");
            }
        }

        public void sentForNew(String data)
        {
            byte[] sendData = Encoding.ASCII.GetBytes(data);
            if (category == "c")
            {
                new_socket.Send(sendData, 0, sendData.Length, 0);//error
            }
            else
            {
                new_client.Send(sendData, 0, sendData.Length, 0);//error
            }
        }
       
        Socket new_socket;
        public void _Client(int port,String ip_add)
        {
           
                new_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                new Thread(delegate()
                {
                 try
                  {
                    new_socket.Connect(new IPEndPoint(IPAddress.Parse(ip_add), port));
                    while (flag)
                    {
                        try
                        {
                            byte[] buffer = new byte[255];
                            int rec = new_socket.Receive(buffer, 0, buffer.Length, 0);

                            Array.Resize(ref buffer, rec);
                            Invoke((MethodInvoker)delegate()
                            {
                                String btn = Encoding.Default.GetString(buffer);
                                if (btn == "new")
                                {
                                    new_Game msgBox = new new_Game();
                                    msgBox.ShowDialog();
                                    if (msgBox.clicked == "OK")
                                    {
                                        clearBtn();
                                        sentForNew(msgBox.clicked);
                                    }
                                    else
                                    {
                                        sentForNew(msgBox.clicked);
                                    }
                                    //clearBtn();
                                }
                                else if (btn == "A1" || btn == "A2" || btn == "A3" || btn == "B1" || btn == "B2" || btn == "B3" || btn == "C1" || btn == "C2" || btn == "C3")
                                {
                                    receiveData(btn);
                                    checkForWinner();
                                    playbtn_panel.Enabled = true;
                                }
                                else if (btn == "OK")
                                {
                                    clearBtn();
                                }
                                else if (btn == "Cancel")
                                {
                                    MessageBox.Show("Other player rejected...", "Reject", MessageBoxButtons.OK);
                                }
                                else 
                                {
                                    textView.Visible = true;
                                    listBox2.Items.Add("Fri :"+btn);
                                    timer2.Start();
                                }
                               
                            });
                        }

                        catch (Exception ex)
                        {
                            flag = false;
                            MessageBox.Show("Exit","Close");
                           
                        }
                    }
                     }
                     catch (Exception ex)
                     {
                         flag = false;
                         MessageBox.Show("Server Time out...\n" + ex.Message);
                     }
                }).Start();
           

        }
       
        public void receiveData(String btn)
        {
            String sign = "";
            if(radioButton1.Checked)
            {
                sign = "O";
            }
            else
            {
                sign = "X";
            }
            if (btn == "A1")
            {
                A1.Text = sign;
                A1.Enabled = false;
                count++;
            }
            else if (btn == "A2")
            {
                A2.Text = sign;
                A2.Enabled = false;
                count++;
            }
            else if (btn == "A3")
            {
                A3.Text = sign;
                A3.Enabled = false;
                count++;
            }
            else if (btn == "B1")
            {
                B1.Text = sign;
                B1.Enabled = false;
                count++;
            }
            else if (btn == "B2")
            {
                B2.Text = sign;
                B2.Enabled = false;
                count++;
            }
            else if (btn == "B3")
            {
                B3.Text = sign;
                B3.Enabled = false;
                count++;
            }
            else if (btn == "C1")
            {
                C1.Text = sign;
                C1.Enabled = false;
                count++;
            }
            else if (btn == "C2")
            {
                C2.Text = sign;
                C2.Enabled = false;
                count++;
            }
            else
            {
                C3.Text = sign;
                C3.Enabled = false;
                count++;
            }
           // MessageBox.Show(textBox1.Text+"..."+count.ToString());
        }

        
        Boolean bo;
        public void checkForWinner()
        {
            String sign = "";
            if (radioButton1.Checked)
            {
                sign = "X";
            }
            if(radioButton2.Checked)
            {
                sign = "O";
            }

            //horizonal checks
            bool there_is_a_winner = false;
            if ((A1.Text == A2.Text) && (A2.Text == A3.Text) && (!A1.Enabled) && (!A2.Enabled) && (!A3.Enabled) && (A1.Text != "") && (A2.Text != "") && (A3.Text != ""))
            {
                there_is_a_winner = true;
                if (A1.Text == sign) bo = true;
                else bo = false;
               
            }
            if ((B1.Text == B2.Text) && (B2.Text == B3.Text) && (!B1.Enabled) && (!B2.Enabled) && (!B3.Enabled) && (B1.Text != "") && (B2.Text != "") && (B3.Text != ""))
            {
                there_is_a_winner = true;
                if (B1.Text == sign) bo = true;
                else bo = false;
                
            }
            if ((C1.Text == C2.Text) && (C2.Text == C3.Text) && (!C1.Enabled) && (!C2.Enabled) && (!C3.Enabled) && (C1.Text != "") && (C2.Text != "") && (C3.Text != ""))
            {
                there_is_a_winner = true;
                if (C1.Text == sign) bo = true;
                else bo = false;
            }
            //vertical checks
            else
                if ((A1.Text == B1.Text) && (B1.Text == C1.Text) && (!A1.Enabled) && (!B1.Enabled) && (!C1.Enabled) && (C1.Text != "") && (B1.Text != "") && (C1.Text != ""))
                {
                    there_is_a_winner = true;
                    if (A1.Text == sign) bo = true;
                    else bo = false;
                    
                }
            if ((A2.Text == B2.Text) && (B2.Text == C2.Text) && (!A2.Enabled) && (!B2.Enabled) && (!C2.Enabled) && (A2.Text != "") && (B2.Text != "") && (C2.Text != ""))
            {
                there_is_a_winner = true;
                if (A2.Text == sign) bo = true;
                else bo = false;


            }
            if ((A3.Text == B3.Text) && (B3.Text == C3.Text) && (!A3.Enabled) && (!B3.Enabled) && (!C3.Enabled) && (C3.Text != "") && (B3.Text != "") && (A3.Text != ""))
            {
                there_is_a_winner = true;
                if (A3.Text == sign) bo = true;
                else bo = false;
            }
            //diagonal checks

            else
                if ((A1.Text == B2.Text) && (B2.Text == C3.Text) && (!A1.Enabled) && (!B2.Enabled) && (!C3.Enabled) && (A1.Text != "") && (B2.Text != "") && (C3.Text != ""))
                {
                    there_is_a_winner = true;
                    if (A1.Text == sign) bo = true;
                    else bo = false;
                }
            if ((A3.Text == B2.Text) && (B2.Text == C1.Text) && (!C1.Enabled) && (!B2.Enabled) && (!A3.Enabled) && (A3.Text != "") && (B2.Text != "") && (C3.Text != ""))
            {
                there_is_a_winner = true;
                if (A3.Text == sign) bo = true;
                else bo = false;
            }

            if (there_is_a_winner)
            {
                this.Enabled = true;
                //String winner = " ";
                //if (bo == false)
                //    winner = "O";
                //else
                //    winner = "X";
                    score(bo);
                   
                   
            }
            else
            {
                if (count == 9)
                {
                    this.Enabled = true;
                    label14.Text = (int.Parse(label14.Text) + 1).ToString();
                    new drawMessageBox().Show();
                    clearBtn();
                    if (category == "s")
                        playbtn_panel.Enabled = true;
                    else
                        playbtn_panel.Enabled = false;
                  
                }
                   
            }
        }

        public void score(Boolean data)
        {
            if (data)
            {
                label12.Text = (int.Parse(label12.Text) + 1).ToString();
                new meaasgeBox().Show();
                clearBtn();
                playbtn_panel.Enabled = false;
            }
            else
            {
                label13.Text = (int.Parse(label13.Text) + 1).ToString();
                new loseMessageBox().Show();
                clearBtn();
                playbtn_panel.Enabled = true;
            }

            
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            byte[] sendData = Encoding.ASCII.GetBytes("new");
            if (category == "c")
            {
                new_socket.Send(sendData, 0, sendData.Length, 0);//error
            }
            else
            {
                new_client.Send(sendData, 0, sendData.Length, 0);//error
            }
           // clearBtn();
        }

        public void clearBtn()
        {
            A1.Text = "";
            A1.Enabled = true;
            A1.BackColor = Color.FromArgb(224, 224, 224);
            A2.Text = "";
            A2.Enabled = true;
            A2.BackColor = Color.FromArgb(224, 224, 224);
            A3.Text = "";
            A3.Enabled = true;
            B1.Text = "";
            B1.Enabled = true;
            B2.Text = "";
            B2.Enabled = true;
            B3.Text = "";
            B3.Enabled = true;
            C1.Text = "";
            C1.Enabled = true;
            C2.Text = "";
            C2.Enabled = true;
            C3.Text = "";
            C3.Enabled = true;
            count = 0;
            A3.BackColor = Color.FromArgb(224, 224, 224);
            C1.BackColor = Color.FromArgb(224, 224, 224);
            C2.BackColor = Color.FromArgb(224, 224, 224);
            C3.BackColor = Color.FromArgb(224, 224, 224);
            B1.BackColor = Color.FromArgb(224, 224, 224);
            B2.BackColor = Color.FromArgb(224, 224, 224);
            B3.BackColor = Color.FromArgb(224, 224, 224);
            //playbtn_panel.Enabled = true;
            if (category == "c")
                playbtn_panel.Enabled = false;
            else
                playbtn_panel.Enabled = true;
           
        }


       
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
           
            int num = 0;
            num++;
            if (num <= 100)
            {
                label20.Visible = false;
                timer1.Stop();
            }
          
        }

        private void panel6_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Form3_FormClosed(object sender, FormClosedEventArgs e)
        {

            if (category == "c")
            {
                if (new_socket.Connected)
                    new_socket.Close();
            }
            if (category == "s")
            {
               
                if (new_server.Connected)
                {
                    new_server.Shutdown(SocketShutdown.Both);
                   // new_server.Close();
                }

                check = false;
                server_check = false;
               new_server.Close();
            }
           
            user._Delete(textBox1.Text);
            Application.Exit();
        }

      

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            String btn = textBox2.Text;
            if (btn != "new" && btn != "A1" && btn != "A2" && btn != "A3" && btn != "B1" && btn != "B2" && btn != "B3" && btn != "C1" && btn != "C2" && btn != "C3" && btn != "OK" && btn != "Cancel")
            {
                textView.Visible = true;
                listBox2.Items.Add("Me :" + textBox2.Text);
                byte[] sendData = Encoding.ASCII.GetBytes(textBox2.Text);
                if (category == "c")
                {
                    new_socket.Send(sendData, 0, sendData.Length, 0);//error
                }
                else
                {
                    new_client.Send(sendData, 0, sendData.Length, 0);//error
                }
               
                textBox2.Text = "";
                timer2.Start();
            }else
            {
                label20.Visible = true;
                label20.Text = "This text doesn't use...";
                timer1.Start();
            }
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
          
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            int num = 0;
            num++;
            if (num <= 100)
            {
                textView.Visible = false;
                timer2.Stop();
            }
        }

        private void Form3_FormClosing(object sender, FormClosingEventArgs e)
        {
          
        }

    }
}
