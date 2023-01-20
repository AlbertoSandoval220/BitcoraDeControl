using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace BitcoraDeControl
{
    public partial class Bitacora : Form
    {
        string[] horarioInicial =
        {
            "07:00","07:50","08:40","09:50","10:40","11:30","12:20","13:10","14:00","14:50","16:00","16:50","17:40","18:30","19:20"
        };

        string[] horarioFinal =
        {
            "07:50","08:40","09:50","10:40","11:30","12:20","13:10","14:00","14:50","16:00","16:50","17:40","18:30","19:20","20:10"
        };

        public Bitacora(string matricula)
        {
            InitializeComponent();
            llenarTabla(matricula);//Se llama al método para llenar la tabla, dando como parámetro la variable matricula que se obtuvo en la clase anterior
            docente();//Se llama al método para llenar la lista desplegable de los docentes
            horaini();
            //horaFin();
        }

        public void docente() //Método que se usará para llenar la lista desplegable de los docentes
        {
            for (int i = 1; i <= int.Parse(bd("SELECT COUNT(*) FROM docente;")); i++)//Ciclo for para insertar a todos los docentes
            {//Se declara la variable i para iniciar en 1 y tomar ese id como primer elemento
                //La condicional para detener el ciclo será el total de docentes registrados en la BD, este número se obtiene mediante un query
                docentes.Items.Add(bd("SELECT nombre FROM docente WHERE id=" + i + ";")); //Agrega el docente con id "i", este número cambia a medida que el ciclo avanza
            }
            docentes.SelectedIndex = 0; //Primer elemento seleccionado por default
        }

        public void horaini() //Método que se usará para llenar la lista desplegable del horario inicial
        { //Es similar al de docente
            for (int i = 0; i < horarioInicial.LongLength; i++)
            {
                comboBoxHoraIni.Items.Add(horarioInicial[i]);
            }
            comboBoxHoraIni.SelectedIndex = 0;
        }


        private void comboBoxHoraIni_SelectedIndexChanged(object sender, EventArgs e) //Método que se usará para llenar la lista de horario final
        { //Esta lista se llena en base al item seleccionado en la primera lista
            comboBoxHoraFin.DataSource = null; //Se elimina la conexion de la lista
            comboBoxHoraFin.Items.Clear(); //Se borran los elementos existentes
            int x = comboBoxHoraIni.SelectedIndex; //Se genera la variable inicial en base al elemento seleccionado de horario inicial
            for (int i = x; i < horarioInicial.LongLength; i++) //Se llena de la misma manera que el anterior
            {//Solamente cambia el valor inicial porque tomará en cuenta el seleccionado de la lista anterior
                comboBoxHoraFin.Items.Add(horarioFinal[i]);
            }

            comboBoxHoraFin.SelectedIndex = 0; //Se selecciona por default el primer elemento en la lista creada
        }
        public string nombre, aPaterno, aMaterno;
        public void llenarTabla(string mat) //Método para llenar los datos, solicita una variable String con la matricula del alumno
        {
            //Querys para obtener el nombre completo
            nombre = "select nombre from alumno where matricula=" + mat + ";";
            aPaterno = "select aPaterno from alumno where matricula=" + mat + ";";
            aMaterno = "select aMaterno from alumno where matricula=" + mat + ";";

            //Llenando la tabla
            txtNombre.Text = bd(nombre).Replace("\n", " ") + bd(aPaterno).Replace("\n", " ") + bd(aMaterno).Replace("\n", " ");
            //Se concatena nombre, apellido paterno y materno
            //Replace quita el salto de linea y lo sustituye por un espacio en blanco

            txtMatricula.Text = mat;

            txtSemestre.Text = bd("select semestre from alumno where matricula=" + mat + ";").Replace("\n", " ") + "°";
            //Se sustituye el salto de linea por un espacio en blanco, se concatena un '°'

            txtTurno.Text = bd("select turno from alumno where matricula=" + mat + ";").Replace("\n", "");

            txtGrupo.Text = bd("select grupo from alumno where matricula=" + mat + ";").Replace("\n", "");
        }

        private void Bitacora_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = e.CloseReason == CloseReason.UserClosing;
        }

        public string bd(string select) //Mismo Método para la BD que el formulario anterior
        {
            string datos = null;
            string cadenaConexion = "Database=bitacora; Data Source=localhost ; User Id= root ; Password=;";

            MySqlConnection conexionBD = new MySqlConnection(cadenaConexion);
            MySqlDataReader reader = null;
            try
            {
                MySqlCommand comando = new MySqlCommand(select);
                comando.Connection = conexionBD;
                conexionBD.Open();

                reader = comando.ExecuteReader();

                while (reader.Read())
                {
                    datos += reader.GetString(0) + "\n";
                }

                if (datos == "" || datos == null)
                {
                    MessageBox.Show("Verifíque la información ingresada BD", "Error en la matrícula", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    return datos;
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Verifíque la información ingresada\n" + ex, "Error en la matrícula", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                conexionBD.Close();
            }

            return "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string horaI = comboBoxHoraIni.Text;
            string horaF = comboBoxHoraFin.Text;
            string mat = txtMatricula.Text;
            string nombre = bd("select nombre from alumno where matricula=" + mat + ";");
            string aPaterno = bd("select aPaterno from alumno where matricula=" + mat + ";");
            string aMaterno = bd("select aMaterno from alumno where matricula=" + mat + ";");
            string nombreDocente = docentes.Text;



            //This is my insert query in which i am taking input from the user through windows forms
            string query = "insert into reporte (horaIni, horaFin, nombreAlumno, aPaterno, aMaterno, semestre, grupo, nombreDocente, reporte) " +
                "values (\"" + horaI + "\",\"" + horaF
                + "\",\"" + nombre + "\",\"" + aPaterno + "\",\"" + aMaterno + "\"," + txtSemestre.Text.Replace("°", "") + ",\"" + txtGrupo.Text + "\",\"" + nombreDocente + "\",\"" + txtReporte.Text + "\");";

            try
            {
                //This is my connection string i have assigned the database file address path
                string MyConnection2 = "database=bitacora; data source=localhost;username=root;password=;";

                //This is  MySqlConnection here i have created the object and pass my connection string.
                MySqlConnection MyConn2 = new MySqlConnection(MyConnection2);

                //This is command class which will handle the query and connection object.
                MySqlCommand MyCommand2 = new MySqlCommand(query, MyConn2);
                MySqlDataReader MyReader2;
                MyConn2.Open();
                MyReader2 = MyCommand2.ExecuteReader();     // Here our query will be executed and data saved into the database.

                DialogResult result = MessageBox.Show("Bienvenido");


                Application.Exit();





                while (MyReader2.Read())
                {
                }
                MyConn2.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
