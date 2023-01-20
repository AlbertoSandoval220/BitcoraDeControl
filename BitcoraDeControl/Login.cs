using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace BitcoraDeControl
{
    public partial class Login : Form
    {
        public string matricula; //Variable global que se utilizará para pasar el parámetro al siguiente formulario
        public Login() //Método constructor de la clase del formulario login
        {
            InitializeComponent(); //Método que inicializa y muestra todos los elementos del formulario junto a sus
        }                           //Características

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) //Método que responde al evento
        {//al hacer click al linklabel
            MessageBox.Show("Pide al encargado de laboratorio que te proporcione\r\n" +
                "el código para poder ingresar como usuario invitado.",
                "Usuario Invitado", MessageBoxButtons.OK, MessageBoxIcon.Information); //Muestra un cuadro de diálogo
        }

        private bool is_validate() //Método para validar el texto ingresado
        {
            bool no_error = true; //Variable booleana para mostrar si existe un error o no se inicializa indicando que no lo hay

            if (txtMatricula.Text == string.Empty || txtMatricula.Text.Length != 14) //Condicional en el cuál se compara
            { //la mátricula ingresada
                //En el caso de que se haya enviado una cadena vacía o una con una lóngitud distina a 14
                no_error = false; // Si se cumple alguna de las premisas, se define que si existe un error
                //Muestra un cuadro de diálogo de error
                MessageBox.Show("Verifíque la información ingresada", "Error en la matrícula", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtMatricula.Text = ""; //Limpia el cuadro de texto
            }

            return no_error; //Retorna el valor booleano que indica si hay error o no
        }

        private void txtMatricula_KeyPress(object sender, KeyPressEventArgs e) //Método que se ejecuta al presionar una tecla 
        {//sobre el cuadro de texto de matrícula obtiene el valor y lo guarda en una variable "e"
            if (!(char.IsNumber(e.KeyChar)) && (e.KeyChar != (char)Keys.Back)) //Condicional que verifíca si la tecla presionada
            {//es un número y o tecla de eliminar (No acepta ningún otro valor
                e.Handled = true; //Si la condición se cumple, entonces, la escritura de la tecla presionada se realiza
            }//Es decir, escribe el número presionado o "borra" si se presiona la tecla de retroceso
        }//Si no se presiona una de estas teclas, no procede el evento y por ende, no se escribirá nada en el campo de texto

        private void btnIngresar_Click(object sender, EventArgs e)
        {
            if(is_validate())//Llama al método de validación, si este no retorna error, se ejecuta lo siguiente
            {
                matricula = txtMatricula.Text; //Almacena lo obtenido en la variable global
                string query = "select matricula from alumno where matricula=\"" + matricula + "\";"; //Guarda el query en una variable string
                if(bd(query) != null || bd(query) != "") //Llama al método BD, envía como parámetro el string query
                {//Si este método no retorna nulo, se ejecuta lo siguiente
                    Bitacora bitacora = new Bitacora(matricula); //Se instancia el objeto de la clase Login y se guarda en la variable login, envía el parámetro solicitado
                    bitacora.Show();
                    this.Hide();
                }
            }
        }

        public string bd(string select) //Método que realiza la conexión a la BD
        {                               //Obtiene como parámetro un string que será el query a ejecutrar

            //Se crea la conexión a la Base de Datos para iniciar sesión

            string datos = null; //Variable para recibir la respuesta del query
            string cadenaConexion = "Database=bitacora; Data Source=localhost ; User Id= root ; Password=;"; //Parámetros para la conexión a la BD
            //Nombre de la BD: Bitacora, Servidor:localhost, usuario: root, contraseña: 

            MySqlConnection conexionBD = new MySqlConnection(cadenaConexion); //Establece la conexión a la BD
            MySqlDataReader reader = null;
            try //Try catch para evitar excepciones
            {
                MySqlCommand comando = new MySqlCommand(select); //Crea el comando que recibió como parámetro el método
                comando.Connection = conexionBD; //Ejecuta el comando en la BD conectada
                conexionBD.Open(); //Abre la conexión a la BD

                reader = comando.ExecuteReader(); //Lectura de la ejecucion del comando

                while (reader.Read()) //Se ejecutará hasta que deje de recibir información
                {
                    datos += reader.GetString(0) + "\n"; //Almacena la respuesta de la BD en una variable string
                }

                if (datos == "" || datos == null) //Si la información recibida es nula, entonces existe un error
                {
                    MessageBox.Show("Verifíque la información ingresada BD", "Error en la matrícula", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //Muestra un mensaje de error
                    txtMatricula.Text = ""; //Elimina lo que hay en el campo de texto
                }
                else
                {
                    return datos; //Si no hay error, retorna el string obtenido
                }
            }
            catch (MySqlException ex) //Se ejecuta en caso de existir una excepcion
            {
                MessageBox.Show("Verifíque la información ingresada\n"+ex, "Error en la matrícula", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //Muestra un mensaje de error
                txtMatricula.Text = "";//Elimina lo que hay en el campo de texto
            }
            finally //Al finalizar la ejecución del bloque trycatch
            {
                conexionBD.Close(); //Se cierra la conexión a la BD
            }

            return ""; //Si no hay conexión, no devuelve nada
        }
    }
}
