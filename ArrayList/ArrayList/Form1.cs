using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace ArrayLista
{
    public partial class Form1 : Form
    {
        List<Persona> Personas = new List<Persona>();



        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            if (File.Exists("Personas.xml"))
            {
                XmlSerializer serializador = new XmlSerializer(typeof(List<Persona>));
                FileStream lector = File.OpenRead("Personas.xml");
                Personas = (List<Persona>)serializador.Deserialize(lector);

                lector.Close();

                
            }
            dgvDatos.DataSource = Personas;


        }



        private void tbNuevo_Click_1(object sender, EventArgs e)
        {
            if (!ValidarID()) return;


            if (Existe(txtId.Text))
            {
                errorProvider1.SetError(txtId, "El ID ya existe");
                txtId.Focus();
                return;
            }
            errorProvider1.SetError(txtId, "");


            if (!ValidarOtroCampos()) return;




            var mi_persona = new Persona();

            mi_persona.ID = txtId.Text;
            mi_persona.Nombres = txtNombres.Text;
            mi_persona.Apellidos = txtApellidos.Text;
            mi_persona.E_mail = txtCorreo.Text;
            mi_persona.Fecha_nacimiento = datatime_fecha_nacimento.Value;
            mi_persona.Salario = Convert.ToDecimal(txtSueldo.Text);


            Personas.Add(mi_persona);
            dgvDatos.DataSource = null;
            dgvDatos.DataSource = Personas;

            txtId.Clear();
            txtNombres.Clear();
            txtApellidos.Clear();
            txtCorreo.Clear();
            txtSueldo.Clear();

            txtId.Focus();
        }

        private bool ValidarOtroCampos()
        {
            if (txtNombres.Text == "")
            {
                errorProvider1.SetError(txtNombres, "Debe ingrese el nombre de una persona");
                txtNombres.Focus();
                return false;
            }
            errorProvider1.SetError(txtNombres, "");

            if (txtApellidos.Text == "")
            {
                errorProvider1.SetError(txtApellidos, "Debe ingrese los apellidos de una persona");
                txtApellidos.Focus();
                return false;
            }
            errorProvider1.SetError(txtApellidos, "");


            Regex regEmail = new Regex(@"^(([^<>()[\]\\.,;:\s@\""]+"
                          + @"(\.[^<>()[\]\\.,;:\s@\""]+)*)|(\"".+\""))@"
                          + @"((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}"
                          + @"\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+"
                          + @"[a-zA-Z]{2,}))$",
                          RegexOptions.Compiled);

            if (!regEmail.IsMatch(txtCorreo.Text)) //isMatch si paso o no paso la expresion regular 
            {
                errorProvider1.SetError(txtCorreo, "Debe ingrese una direccion de correo valida");
                txtCorreo.Focus();
                return false;
            }

            errorProvider1.SetError(txtCorreo, "");


            decimal salario;

            if (!Decimal.TryParse(txtSueldo.Text, out salario))
            {
                errorProvider1.SetError(txtSueldo, "Debe ingrese una cifra numerica");
                txtSueldo.Focus();
                return false;
            }
            if (salario < 0)
            {
                errorProvider1.SetError(txtSueldo, "Debe ingrese un numero mayor a (0) ");
                txtSueldo.Focus();
                return false;
            }
            errorProvider1.SetError(txtSueldo, "");

            return true;
        }

        private bool Existe(string Id)
        {
            foreach (Persona Persona in Personas)
            {
                if (Persona.ID == Id) return true;

            }

            return false;
        }

        private void tsbBuscar_Click(object sender, EventArgs e)
        {
            if (!ValidarID()) return;

            Persona miP_ersona = GetPersona(txtId.Text);

            if (miP_ersona == null)
            {
                errorProvider1.SetError(txtId, "La persona que busca no existe");
                txtId.Focus();
                return;
            }
            errorProvider1.SetError(txtId, "");

            txtNombres.Text = miP_ersona.Nombres;
            txtApellidos.Text = miP_ersona.Apellidos;
            txtCorreo.Text = miP_ersona.E_mail;
            datatime_fecha_nacimento.Value = miP_ersona.Fecha_nacimiento;
            txtSueldo.Text = miP_ersona.Salario.ToString();

        }

        public bool ValidarID()
        {
            if (txtId.Text == "")
            {
                errorProvider1.SetError(txtId, "Debe ingrese el ID de una persona");
                txtId.Focus();
                return false;
            }
            errorProvider1.SetError(txtId, "");
            return true;


        }

        public Persona GetPersona(string ID)
        {
            foreach (Persona mipersona in Personas)
            {
                if (mipersona.ID == ID) return mipersona;

            }
            return null;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (!ValidarID()) return;

            if (GetPersona(txtId.Text) == null)
            {
                errorProvider1.SetError(txtId, "Persona no exite");
                txtId.Focus();
                return;
            }
            errorProvider1.SetError(txtId, "");

            if (!ValidarOtroCampos()) return;

            foreach (Persona miPersona in Personas)
            {
                if (miPersona.ID == txtId.Text)
                {
                    miPersona.ID = txtId.Text;
                    miPersona.Nombres = txtNombres.Text;
                    miPersona.Apellidos = txtApellidos.Text;
                    miPersona.E_mail = txtCorreo.Text;
                    miPersona.Fecha_nacimiento = datatime_fecha_nacimento.Value;
                    miPersona.Salario = Convert.ToDecimal(txtSueldo.Text);
                    break;
                }

            }

            dgvDatos.DataSource = null;
            dgvDatos.DataSource = Personas;

            //txtId.Clear();
            //txtNombres.Clear();
            //txtApellidos.Clear();
            //txtCorreo.Clear();
            //txtSueldo.Clear();

            //txtId.Focus();


        }

        private void tsbLimpiar_Click(object sender, EventArgs e)
        {
            txtId.Clear();
            txtNombres.Clear();
            txtApellidos.Clear();
            txtCorreo.Clear();
            txtSueldo.Clear();

            txtId.Focus();
        }

        private void tsbBorrar_Click(object sender, EventArgs e)
        {
            DialogResult respueta = MessageBox.Show(this, "Esta seguro de Borrar el reguistro?", "Confirmacion", MessageBoxButtons.YesNo,
                MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

            if (respueta == DialogResult.No) return;

            if (!ValidarID()) return;

            if (GetPersona(txtId.Text) == null)
            {
                errorProvider1.SetError(txtId, "Persona no exite");
                txtId.Focus();
                return;
            }
            errorProvider1.SetError(txtId, "");

            foreach (Persona miPersona in Personas)
            {
                if (miPersona.ID == txtId.Text)
                {
                    Personas.Remove(miPersona);
                    break;
                }
                
            }

            tsbLimpiar_Click(sender, e);
            dgvDatos.DataSource = null;
            dgvDatos.DataSource = Personas;


        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            XmlSerializer serializador = new XmlSerializer(typeof(List<Persona>));
            TextWriter escritor = new StreamWriter("Personas.xml");
            serializador.Serialize(escritor, Personas);
        }

    }
}
