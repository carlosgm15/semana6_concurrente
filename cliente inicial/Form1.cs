using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace Proyecto_V1
{
    public partial class Form1 : Form
    {
         private DataTable tabla;
        Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        IPEndPoint remoteEP = new IPEndPoint(IPAddress.Parse("192.168.56.101"), 9200);
        byte[] data = new byte[1024];

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            try 
            { 
                socket.Connect(remoteEP);
                this.BackColor = Color.Green;
            }
            catch (SocketException ee) 
            { 
                Console.WriteLine("Unable to connect to server. "); 
                Console.WriteLine(ee); 
                return; 
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //logear
            String registro = "1/" + B_Mail.Text + "/" + B_Password.Text + "\0";
            socket.Send(Encoding.ASCII.GetBytes(registro));

            int dataSize = socket.Receive(data);
            string resultado = (Encoding.ASCII.GetString(data, 0, dataSize));

            if (resultado == "0\0") //Tengo que consieguir poder quitar esto
            {
                MessageBox.Show("Correctamente Logueado");
                //MessageBox.Show(resultado);
            }
            else
            {
                MessageBox.Show("Ha habido algun error con el Log In");
                MessageBox.Show(resultado);
            }

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            String registro = "2/" + B_Mail.Text + "/" + B_Password.Text + "\0";
            socket.Send(Encoding.ASCII.GetBytes(registro));

            int dataSize = socket.Receive(data);
            string resultado = (Encoding.ASCII.GetString(data, 0, dataSize));
        } 

        private void B_cuestionar_Click(object sender, EventArgs e)
        {
            if (C_1.Checked) //Dar el jugador que es  mayor de edad
            {
                String registro = "3/";
                // Enviamos al servidor el nombre tecleado
                socket.Send(Encoding.ASCII.GetBytes(registro));

                //Recibimos la respuesta del servidor
                int dataSize = socket.Receive(data);
                string resultado = (Encoding.ASCII.GetString(data, 0, dataSize));
                MessageBox.Show("El nombre del jugador mayor de edad es : " + resultado);
            }

            if (C_2.Checked) //Dar el id del jugador con el mayor puntaje de la partida
            {
                String registro = "4/";
                // Enviamos al servidor el nombre tecleado
                socket.Send(Encoding.ASCII.GetBytes(registro));

                //Recibimos la respuesta del servidor
                int dataSize = socket.Receive(data);
                string resultado = (Encoding.ASCII.GetString(data, 0, dataSize));
                MessageBox.Show("El id del jugador con mayor puntaje es : " + resultado);
            }

            if (C_3.Checked) //Dar la divison del jugador 
            {
                String registro = "5/" + cuestionador.Text;
                // Enviamos al servidor el nombre tecleado
                socket.Send(Encoding.ASCII.GetBytes(registro));

                //Recibimos la respuesta del servidor
                int dataSize = socket.Receive(data);
                string resultado = (Encoding.ASCII.GetString(data, 0, dataSize));
                MessageBox.Show("La division del jugador : " + cuestionador.Text + " es:" + resultado);
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {

            string mensaje = "0/";
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            socket.Send(msg);

            this.BackColor = Color.Gray;
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
        }

        private void Lista_ConBt_Click(object sender, EventArgs e)
        {
            tabla = new DataTable();
            //crear columna y fila
            DataColumn column;
            DataRow row;
            //Crear la columna Usuario
            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "Usuario";
            column.ReadOnly = true;
            column.Unique = true;
            //añadir a la tabla
            this.tabla.Columns.Add(column);
           
            //Limpiamos info de la tabla
            tabla.Rows.Clear();

            //Asignamos el numero 19 pedir lista conectados
            string mensaje = "6/";
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            socket.Send(msg);
            //Recibimos la respuesta del servidor
            byte[] msg2 = new byte[80];
            socket.Receive(msg2);
            mensaje = Encoding.ASCII.GetString(msg2).Split(',')[0];
            //Separamos los conectados
            string[] ListaSeparada;
            ListaSeparada = mensaje.Split('\0');
            int i;
            ListaSeparada = mensaje.Split('/');
            ListaSeparada = mensaje.Split('/');
            
            //tabla.Columns.Clear();
            //Colocamos info en la tabla
            for (i = 0; i < ListaSeparada.Length; i++)
            {
                row = tabla.NewRow();
                row["Usuario"] = ListaSeparada[i];
                tabla.Rows.Add(row);
                ListaSeparada = mensaje.Split('/');
            }
            //añadimos la tabla al grid
            dataGridView1.DataSource = tabla;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Form formulario = new Form2();
            formulario.Show();
        }

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }
        }

    }

