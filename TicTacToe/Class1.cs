using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace TicTacToe
{
    class Class1
    {
        MySqlConnection mycon = new MySqlConnection("username=naylin;password=nay;datasource=192.168.43.141;database=tictactoe;");
        DataTable dt = new DataTable();
       
     
        public int getPort(String name)
        {
            
            String sql = "select * from users where name=@name";
            MySqlCommand cmd = new MySqlCommand(sql, mycon);
            cmd.Parameters.AddWithValue("@name", name);
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            da.Fill(dt);
            mycon.Close();
            int port = int.Parse(dt.Rows[0]["id"].ToString())+1990;
            return port;
        }

        public void _Select()
        {
            String sql = "select * from users";
            MySqlCommand cmd = new MySqlCommand(sql, mycon);
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            da.Fill(dt);
            mycon.Close();
        }

        public void _Delete(String name)
        {
            mycon.Open();
            MySqlCommand del_cmd = new MySqlCommand("delete from users where name=@name", mycon);
            del_cmd.Parameters.AddWithValue("@name", name);
            del_cmd.ExecuteNonQuery();
            mycon.Close();
        }

        public void _Update(String name,int port)
        {
            mycon.Open();
            MySqlCommand insert_cmd = new MySqlCommand("Update users set port=@port where name=@name", mycon);
            insert_cmd.Parameters.AddWithValue("@port", port);
            insert_cmd.Parameters.AddWithValue("@name", name);
            insert_cmd.ExecuteNonQuery();
            mycon.Close();
        }
        public void _Insert(String name)
        {
            String ip = _getaIpAddress();
            mycon.Open();
            String nameMM = name;
            MySqlCommand insert_cmd = new MySqlCommand("Insert into users (name,ip_add) values (@name,@ip)", mycon);
            insert_cmd.Parameters.AddWithValue("@name", nameMM);
            insert_cmd.Parameters.AddWithValue("@ip", ip);
            insert_cmd.ExecuteNonQuery();
            mycon.Close();
        }

        public String _getaIpAddress()
        {
            IPHostEntry host;
            String ip = "";
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ipadd in host.AddressList)
            {
                if (ipadd.AddressFamily.ToString() == "InterNetwork")
                {
                    ip = ipadd.ToString();

                }
            }
            return ip;
        }

        public String _getIPBYName(String name)
        {
            String sql = "select * from users where name=@name";
            MySqlCommand cmd = new MySqlCommand(sql, mycon);
            cmd.Parameters.AddWithValue("@name", name);
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            da.Fill(dt);
            mycon.Close();
            String ip_add = dt.Rows[0]["ip_add"].ToString();
            return ip_add;
        }



        
    }
}
