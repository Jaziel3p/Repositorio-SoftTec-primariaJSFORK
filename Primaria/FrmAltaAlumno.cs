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
using System.Net.Mail;


namespace Primaria
{
    public partial class FrmAltaAlumno : Form
    {
        bool fechabool = false; bool coincideCURP = false;
        Conexion conecta = new Conexion();
        MySqlConnection sqlCnn = new MySqlConnection(Conexion.cadena);
        MySqlCommand sqlCmd;
        MySqlDataReader myReader;
        string sql; string fecha = ""; string sexo = "";
        public FrmAltaAlumno()
        {
            InitializeComponent();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnRegistroAlum_Click(object sender, EventArgs e)
        {
            int veritutor;
             confirmaCURP();

             if (validarEmail(txtCorreo.Text) == false) { MessageBox.Show("Formato de correo invalido"); }
             else {
             if(coincideCURP == true)
                 {
                     DialogResult dialogResult = MessageBox.Show("Declaro que:  \n-He verificado que la información ingresada proviene de documentos oficiales" +
                         "\n-He ingresado y verificado correctamente la información"
                         , "Advertencia de responsabilidad",
                         MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                     if (dialogResult == DialogResult.Yes)
                     {
                        veritutor = verificaTutor();

                        if(veritutor == 0) {
                            DialogResult dialogResult1 = MessageBox.Show("¿Registrar nuevo tutor?"
                         , "Registro Nuevo de Tutor",
                         MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (dialogResult1 == DialogResult.Yes)
                            {
                               try
                                {
                                    string sql = "call softteci_primaria.registraAlumno('" + txtNombre.Text + "', '" + txtApellidoP.Text + "', '" + txtApellidoM.Text + "', '" + fecha + "', '" + sexo + "', '" + txtCurp.Text + "', '" + comboBox2.SelectedItem.ToString()
                                        + "', '" + txtNombreTut.Text + "', '" + txtApePTutor.Text + "', '" + txtApeMTutor.Text + "', '" + txtCorreo.Text + "');";
                                    sqlCnn.Open();
                                    sqlCmd = new MySqlCommand(sql, sqlCnn);
                                    sqlCmd.ExecuteNonQuery();
                                    MessageBox.Show("Registro Exitoso");
                                }
                                catch (MySqlException ex)
                                {
                                    if (ex.Number == 1044) { MessageBox.Show("La CURP registrada ya existe"); }
                                    if (ex.Number == 1062) { MessageBox.Show("El correo Registrado ya existe"); }
                                }
                                sqlCnn.Close();
                            }
                            else { MessageBox.Show("Operacion cancelada" , "Cancelado", MessageBoxButtons.OK , MessageBoxIcon.Error);}
                        }
                        else if (veritutor > 0) {
                            try
                            {
                                string sql = "call softteci_primaria.registraAlumno('" + txtNombre.Text + "', '" + txtApellidoP.Text + "', '" + txtApellidoM.Text + "', '" + fecha + "', '" + sexo + "', '" + txtCurp.Text + "', '" + comboBox2.SelectedItem.ToString()
                                    + "', '" + txtNombreTut.Text + "', '" + txtApePTutor.Text + "', '" + txtApeMTutor.Text + "', '" + txtCorreo.Text + "');";
                                sqlCnn.Open();
                                sqlCmd = new MySqlCommand(sql, sqlCnn);
                                sqlCmd.ExecuteNonQuery();
                                MessageBox.Show("Registro Exitoso");
                            }
                            catch (MySqlException ex)
                            {
                                if (ex.Number == 1044) { MessageBox.Show("La CURP registrada ya existe"); }
                                if (ex.Number == 1062) { MessageBox.Show("El correo Registrado ya existe"); }
                            }
                            sqlCnn.Close();

                        }
                        
                        

                    }
                     else if (dialogResult == DialogResult.No)
                     {
                         MessageBox.Show("Registro Cancelado", "Cancelación");
                     }
                 }
             }    

           
        }
        private int verificaTutor()
        {
          
        
            
            string nombre, apepat, apemat, correo;
            int resul = 0;
            nombre = txtNombreTut.Text; apepat = txtApePTutor.Text; apemat = txtApeMTutor.Text; correo = txtCorreo.Text;
            try
            {
                // select softteci_primaria.verificaTutor('nombre', 'apepat', 'apemat', 'correo');

                string sql = "select softteci_primaria.verificaTutor('" + nombre + "','" + apepat + "','"
                       + apemat + "','" + correo +"');";
                
                sqlCnn.Open();
                sqlCmd = new MySqlCommand(sql, sqlCnn);
                myReader = sqlCmd.ExecuteReader();
                while (myReader.Read())
                {
                     resul = Int32.Parse(myReader.GetString(0));
                    
                }
                
               

            }
            catch (Exception e) { resul = -1; MessageBox.Show("Ocurrio un error, reintentelo"); }
            sqlCnn.Close();

            return resul;
        }
        static bool validarEmail(string email)
        {
            try
            {
                if (email.Equals("")) { MessageBox.Show("El correo esta vacio"); }
                else { new MailAddress(email); }
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
        private void confirmaCURP()
        {
            //inicializando las variables
           
            string curp;
            int contador = 0;
            string apepat1 = ""; string cadena = ""; string apepat2 = "";
            string apemat = "";
            string nombre = "";

            string cadena2 = "";
            //manejando el apellido paterno
            try
            {    
                apepat1 = txtApellidoP.Text.Substring(0, 1);
                cadena = txtApellidoP.Text;
                apepat2 = "";
                cadena2 = txtApellidoP.Text.Substring(1);

                foreach (char c in cadena2)
                {
                    if (c == 'A' && contador == 0)
                    {
                        apepat2 = "A"; contador = 1;
                    }
                    if (c == 'E' && contador == 0)
                    {
                        apepat2 = "E"; contador = 1;
                    }
                    if (c == 'I' && contador == 0)
                    {
                        apepat2 = "I"; contador = 1;
                    }
                    if (c == 'O' && contador == 0)
                    {
                        apepat2 = "O"; contador = 1;
                    }
                    if (c == 'U' && contador == 0)
                    {
                        apepat2 = "U"; contador = 1;
                    }
                }
            } catch (Exception) { MessageBox.Show("Porfavor introduce un apellido paterno valido"); }

            //manejando el apellido materno
            try
            {
                apemat = txtApellidoM.Text.Substring(0, 1);
            }
            catch { MessageBox.Show("Porfavor introduce un apellido materno valido"); }
           
            //manejando el nombre
            try { nombre = txtNombre.Text.Substring(0, 1);
                if(txtNombre.Text.Substring(0, 1).Equals(" "))
                {
                    throw new ApplicationException("Espacio en nombre");
                }
            
            } catch { MessageBox.Show("Porfavor introduce un nombre valido"); }

            //manejando la fecha de nacimiento
            try
            {
                if (fechabool)
                {
                    fecha = Fecha.Value.ToString("yyMMdd");
                }
                else
                {
                    MessageBox.Show("No ha seleccionado una fecha");
                }
            }
            catch { MessageBox.Show("Selecciona una fecha valida");}

            //manejando el sexo
            try
            {
                if (combosexo.SelectedItem != null)
                {
                    sexo = combosexo.SelectedItem.ToString();

                    if (sexo.Equals("MASCULINO")) { sexo = "H"; }
                    if (sexo.Equals("FEMENINO")) { sexo = "M"; }
                }
                else { throw new InvalidOperationException("Error"); }
            }
            catch { { MessageBox.Show("Porfavor Selecciona un sexo"); } }

              //  HIVA010225H
              //  GR     DLLA8
            curp =(apepat1 + apepat2 + apemat + nombre + fecha + sexo);
            try
            {
                if (txtCurp.Text.Length < 18)
                {
                    coincideCURP = false;
                    MessageBox.Show("Curp menor a 18 digitos");
                }
                else if (txtCurp.Text.Length == 18)
                {
                   
                    
                    if (curp.Equals(txtCurp.Text.Substring(0, 11)))
                    {
                        coincideCURP = true;
                        //La curp coincide
                    }
                    else
                    {
                        coincideCURP = false;
                    }
                }
            }
            catch { MessageBox.Show("Porfavor introduzca una CURP valida"); }

            
            if (coincideCURP == false  ) { MessageBox.Show("La CURP no coincide");}
          //  MessageBox.Show(curp +" " + txtCurp.Text.Substring(0,11));
           // MessageBox.Show(cadena2);


        }

        private void txtNombre_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsLetter(e.KeyChar) && !char.IsWhiteSpace(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtApellidoP_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsLetter(e.KeyChar) &&
             (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void txtApellidoM_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsLetter(e.KeyChar) &&
             (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void txtCurp_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsLetter(e.KeyChar) &&
             (e.KeyChar != '.') && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void FrmAltaAlumno_Load(object sender, EventArgs e)
        {
            Fecha.Format = DateTimePickerFormat.Custom;
            Fecha.CustomFormat = "dd/MMM/yyyy";
            comboBox2.Items.Clear();
        }

        private void Fecha_ValueChanged(object sender, EventArgs e)
        {
            fechabool = true;
            oculta.Visible = false;
        }
        private void cargaGrupos()
        {
          //  call softteci_primaria.muestraGrupos("+2+");";
            string sql = "call softteci_primaria.muestraGrupos("+comboBox1.SelectedItem.ToString()+"); ";
            comboBox2.Items.Clear();
            sqlCnn.Open();
            sqlCmd = new MySqlCommand(sql, sqlCnn);
            myReader = sqlCmd.ExecuteReader();
            while (myReader.Read())
            {
                
                comboBox2.Items.Add(myReader["idgrupo"].ToString());

                    
            }
            sqlCnn.Close();
        }

        private void txtNombreTut_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = e.KeyChar == Convert.ToChar(Keys.Space);
        }

        private void txtApePTutor_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = e.KeyChar == Convert.ToChar(Keys.Space);
        }

        private void txtApeMTutor_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = e.KeyChar == Convert.ToChar(Keys.Space);
        }

        private void txtCorreo_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = e.KeyChar == Convert.ToChar(Keys.Space);
        }

        private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            cargaGrupos();
        }
    }
}
