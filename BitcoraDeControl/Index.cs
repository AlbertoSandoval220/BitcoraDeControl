using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BitcoraDeControl
{
    public partial class Index : Form
    {
        public Index() //Método constructor de la clase del formulario principal
        {
            InitializeComponent(); //Método que inicializa y muestra todos los elementos del formulario junto a sus
        }                           //carácteristicas

        private void btnIngresar_Click(object sender, EventArgs e) //Método que responde al evento 
        {                                                           //al hacer click al botón "Ingresar"
            Login login = new Login(); //Se instancia el objeto de la clase Login y se guarda en la variable login
            login.Show(); //Aplicamos el método show al elemento login para mostrar el formulario
            this.Hide(); //Aplicamos el método hide para ocultar el formulario actual
        }

        private void Index_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = e.CloseReason == CloseReason.UserClosing;
        }
    }
}
