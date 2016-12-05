using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Data.SqlClient;
using Entidades;
using Microsoft.VisualBasic;
using System.IO;

namespace Aeropuerto_ConADO.NET
{
    public partial class frmPrincipal : Form
    {

        private DataSet _dataSetAviones;
        private SqlDataAdapter _dataAdapterAviones;

        public frmPrincipal()
        {
            InitializeComponent();
        }



        private DataTable CrearDataTableVuelos()
        {
            DataTable dtVuelos = new DataTable("Vuelos");

            DataColumn Codigo = new DataColumn("codigo", typeof(int));
            DataColumn Destino = new DataColumn("destino", typeof(string));

            Codigo.AutoIncrement = true;
            Codigo.AutoIncrementSeed = 1000;
            Codigo.AutoIncrementStep = 5;

            dtVuelos.Columns.Add(Codigo);
            dtVuelos.Columns.Add(Destino);

            dtVuelos.PrimaryKey = new DataColumn[] { dtVuelos.Columns["codigo"] };

            DataRow dato1 = dtVuelos.NewRow();
            DataRow dato2 = dtVuelos.NewRow();
            DataRow dato3 = dtVuelos.NewRow();
            DataRow dato4 = dtVuelos.NewRow();
            DataRow dato5 = dtVuelos.NewRow();

            dato1["destino"] = "Caribe";
            dato2["destino"] = "Buenos Aires";
            dato3["destino"] = "Mar del Plata";
            dato4["destino"] = "Bariloche";
            dato5["destino"] = "Mendoza";


            dtVuelos.Rows.Add(dato1);
            dtVuelos.Rows.Add(dato2);
            dtVuelos.Rows.Add(dato3);
            dtVuelos.Rows.Add(dato4);
            dtVuelos.Rows.Add(dato5);


            dtVuelos.WriteXmlSchema(AppDomain.CurrentDomain.BaseDirectory + "VuelosEsquema.XML");
            dtVuelos.WriteXml(AppDomain.CurrentDomain.BaseDirectory + "VuelosDatos.XML");

            return dtVuelos;
        }


        private void ConfigurarDataAdapter()
        {
            SqlConnection sqlConn = new SqlConnection(Properties.Settings.Default.strConexion);

            SqlCommand select = new SqlCommand("select Matricula as IdAvion, Marca, Modelo, Capacidad, codigoVuelo from Aviones", sqlConn);
            SqlCommand insert = new SqlCommand("insert into Aviones (Matricula, Marca, Modelo, Capacidad, codigoVuelo) values (@MATRICULA, @MARCA, @MODELO, @CAPACIDAD, @CODIGO)", sqlConn);
            SqlCommand delete = new SqlCommand("delete from Aviones where Matricula=@MATRICULA", sqlConn);
            SqlCommand update = new SqlCommand("update Aviones set Marca=@MARCA, Modelo=@MODELO, Capacidad=@CAPACIDAD, codigoVuelo=@CODIGO where Matricula=@MATRICULA", sqlConn);

            this._dataAdapterAviones.SelectCommand = select;
            this._dataAdapterAviones.InsertCommand = insert;
            this._dataAdapterAviones.DeleteCommand = delete;
            this._dataAdapterAviones.UpdateCommand = update;

            this._dataAdapterAviones.InsertCommand.Parameters.Add("@MATRICULA", SqlDbType.Int, 18, "idAvion");
            this._dataAdapterAviones.InsertCommand.Parameters.Add("@MARCA", SqlDbType.VarChar, 50, "Marca");
            this._dataAdapterAviones.InsertCommand.Parameters.Add("@MODELO", SqlDbType.VarChar, 50, "Modelo");
            this._dataAdapterAviones.InsertCommand.Parameters.Add("@CAPACIDAD", SqlDbType.Int, 18, "Capacidad");
            this._dataAdapterAviones.InsertCommand.Parameters.Add("@CODIGO", SqlDbType.Int, 18, "codigoVuelo");

            this._dataAdapterAviones.DeleteCommand.Parameters.Add("@MATRICULA", SqlDbType.Int, 18, "idAvion");

            this._dataAdapterAviones.UpdateCommand.Parameters.Add("@MATRICULA", SqlDbType.Int,18, "IdAvion");
            this._dataAdapterAviones.UpdateCommand.Parameters.Add("@MARCA", SqlDbType.VarChar, 50, "Marca");
            this._dataAdapterAviones.UpdateCommand.Parameters.Add("@MODELO", SqlDbType.VarChar, 50, "Modelo");
            this._dataAdapterAviones.UpdateCommand.Parameters.Add("@CAPACIDAD", SqlDbType.Int, 18, "Capacidad");
            this._dataAdapterAviones.UpdateCommand.Parameters.Add("@CODIGO", SqlDbType.Int, 18, "codigoVuelo");

        }


        private void TraerDatos()
        {
            SqlConnection SqlConn = new SqlConnection(Properties.Settings.Default.strConexion);
            DataTable dtVuelos = new DataTable("Vuelos");
            DataTable dtAviones = new DataTable("Aviones");

            //CARGAR TABLA DE AVIONES CON DATAREADER
            using (SqlConn)
            {
                SqlCommand comando = new SqlCommand("select Matricula as IdAvion, Marca, Modelo, Capacidad, codigoVuelo from Aviones", SqlConn);
                SqlConn.Open();
                SqlDataReader lector = comando.ExecuteReader();

                if (lector.HasRows == true)
                {
                    dtAviones.Load(lector);
                }
                else
                {
                    MessageBox.Show("No hay registros para guardar");
                }

                lector.Close();
            } //termina using

            this._dataSetAviones.Tables.Add(dtAviones);

            //CARGAR TABLA DE VUELOS
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "VuelosEsquema.XML") && File.Exists(AppDomain.CurrentDomain.BaseDirectory + "VuelosDatos.XML"))
            {
                dtVuelos.ReadXmlSchema(AppDomain.CurrentDomain.BaseDirectory + "VuelosEsquema.XML");
                dtVuelos.ReadXml(AppDomain.CurrentDomain.BaseDirectory + "VuelosDatos.XML");
            }
            else
            {
                dtVuelos = this.CrearDataTableVuelos();
            }

            this._dataSetAviones.Tables.Add(dtVuelos);
        }

        private void EstablecerRelaciones()
        {
            DataRelation relacion = new DataRelation("AvionesVuelos", this._dataSetAviones.Tables["Vuelos"].Columns["codigo"],
                this._dataSetAviones.Tables["Aviones"].Columns["codigoVuelo"]);

            this._dataSetAviones.Relations.Add(relacion);

        }


        private void guardarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                this._dataAdapterAviones.Update(this._dataSetAviones.Tables["Aviones"]);
                MessageBox.Show("La base de datos se ha actualizado correctamente!!!", "Actualizacion Correcta");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error al actualizar BD", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }

        private void altaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAvion frmAltaAvion = new frmAvion();
            frmAltaAvion.Text = "Alta de Avion";

            DataRow registro = this._dataSetAviones.Tables["Aviones"].NewRow();

            if (frmAltaAvion.ShowDialog() == DialogResult.OK)
            {
                registro["idAvion"] = frmAltaAvion.avion.Matricula;
                registro["Marca"] = frmAltaAvion.avion.Marca;
                registro["Modelo"] = frmAltaAvion.avion.Modelo;
                registro["Capacidad"] = frmAltaAvion.avion.Capacidad;
                registro["codigoVuelo"] = frmAltaAvion.avion.Codigo;

                this._dataSetAviones.Tables["Aviones"].Rows.Add(registro);

            }
        }

        private void frmPrincipal_Load(object sender, EventArgs e)
        {
            this._dataAdapterAviones = new SqlDataAdapter();
            this._dataSetAviones = new DataSet();

            this.ConfigurarDataAdapter();
            this.TraerDatos();
            this.EstablecerRelaciones();
        }

        private void bajaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string strBajaAvion = Interaction.InputBox("Ingrese matricula del avion", "Dar Baja");
            int id, indice = -1;


            if (!int.TryParse(strBajaAvion, out id))
                MessageBox.Show("Error en la matricula del avion ingresado. Verificar", "Error de dato");
            else
            {
                foreach (DataRow row in this._dataSetAviones.Tables["Aviones"].Rows)
                {
                    if (row["idAvion"].ToString() == id.ToString())
                    {
                        indice = this._dataSetAviones.Tables["Aviones"].Rows.IndexOf(row);
                        break;
                    }
                }
            }

            if (indice == -1)
            {
                MessageBox.Show("No se encuentra el avion en la tabla", "Sin registro");
            }
            else
            {
                int matriculaEncontrada = (int)this._dataSetAviones.Tables["Aviones"].Rows[indice]["idAvion"];
                string marcaEncontrada = (string)this._dataSetAviones.Tables["Aviones"].Rows[indice]["Marca"];
                string ModeloEncontrado = (string)this._dataSetAviones.Tables["Aviones"].Rows[indice]["Modelo"];
                int capacidadEncontrada = (int)this._dataSetAviones.Tables["Aviones"].Rows[indice]["Capacidad"];
                int codigoEncontrado = (int)this._dataSetAviones.Tables["Aviones"].Rows[indice]["codigoVuelo"];

                Avion avionEncontrado = new Avion(matriculaEncontrada, marcaEncontrada, ModeloEncontrado, capacidadEncontrada,
                    codigoEncontrado);

                frmAvion frmBajaAvion = new frmAvion(avionEncontrado, "baja");
                frmBajaAvion.Text = "Dar de Baja Avion" + matriculaEncontrada;

                if (frmBajaAvion.ShowDialog() == DialogResult.OK)
                {
                    this._dataSetAviones.Tables["Aviones"].Rows[indice].Delete();
                    MessageBox.Show("Registro eliminado correctamente.");
                }
            }

        }

        private void modificacionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string strBajaAvion = Interaction.InputBox("Ingrese matricula del avion", "Dar Baja");
            int id, indice = -1;

            if (!int.TryParse(strBajaAvion, out id))
                MessageBox.Show("Error en la matricula del avion ingresado. Verificar", "Error de dato");
            else
            {
                foreach (DataRow row in this._dataSetAviones.Tables["Aviones"].Rows)
                {
                    if (row["idAvion"].ToString() == id.ToString())
                    {
                        indice = this._dataSetAviones.Tables["Aviones"].Rows.IndexOf(row);
                        break;
                    }
                }
            }

            if (indice == -1)
            {
                MessageBox.Show("No se encuentra el avion en la tabla", "Sin registro");
            }
            else
            {
                int matriculaEncontrada = (int)this._dataSetAviones.Tables["Aviones"].Rows[indice]["idAvion"];
                string marcaEncontrada = (string)this._dataSetAviones.Tables["Aviones"].Rows[indice]["Marca"];
                string ModeloEncontrado = (string)this._dataSetAviones.Tables["Aviones"].Rows[indice]["Modelo"];
                int capacidadEncontrada = (int)this._dataSetAviones.Tables["Aviones"].Rows[indice]["Capacidad"];
                int codigoEncontrado = (int)this._dataSetAviones.Tables["Aviones"].Rows[indice]["codigoVuelo"];

                Avion avionEncontrado = new Avion(matriculaEncontrada, marcaEncontrada, ModeloEncontrado, capacidadEncontrada,
                    codigoEncontrado);

                frmAvion frmModificarAvion = new frmAvion(avionEncontrado, "modificar");
                frmModificarAvion.Text = "Modificar datos del Avion" + matriculaEncontrada;

                if (frmModificarAvion.ShowDialog() == DialogResult.OK)
                {
                    this._dataSetAviones.Tables["Aviones"].Rows[indice]["Marca"] = frmModificarAvion.avion.Marca;
                    this._dataSetAviones.Tables["Aviones"].Rows[indice]["Modelo"] = frmModificarAvion.avion.Modelo;
                    this._dataSetAviones.Tables["Aviones"].Rows[indice]["Capacidad"] = frmModificarAvion.avion.Capacidad;
                    this._dataSetAviones.Tables["Aviones"].Rows[indice]["codigoVuelo"] = frmModificarAvion.avion.Codigo;
                    MessageBox.Show("Registro modificado correctamente.");
                }
            }
        }

        private void avionesToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            frmMostrar formMostrarAviones = new frmMostrar("aviones", this._dataSetAviones);
            formMostrarAviones.Text = "Grilla de Aviones";
            formMostrarAviones.ShowDialog();
        }

        private void vuelosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmMostrar formMostrarVuelos = new frmMostrar("vuelos", this._dataSetAviones);
            formMostrarVuelos.Text = "Grilla de Vuelos";
            formMostrarVuelos.ShowDialog();
        }























    }
}
