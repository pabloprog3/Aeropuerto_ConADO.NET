using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Entidades;

namespace Aeropuerto_ConADO.NET
{
    public partial class frmAvion : Form
    {
        public frmAvion()
        {
            InitializeComponent();
        }


        private Avion _avion;

        public Avion avion
        {
            get { return this._avion; }
            set { this._avion = value; }
        }


        public frmAvion(Avion unAvion, string queHacer)
            : this()
        {
            this._avion = unAvion;

            switch (queHacer)
            {
                case "baja":
                    foreach (Control c in this.Controls)
                    {
                        if (c is TextBox || c is ComboBox)
                            c.Enabled = false;
                    }
                    break;

                case "modificar":
                    this.txtMatricula.Enabled = false;
                    break;
            }
           

            if (this._avion.Codigo == 1000)
                this.cboxDestino.SelectedIndex = 0;
            else if (this._avion.Codigo == 1005)
                this.cboxDestino.SelectedIndex = 1;
            else if (this._avion.Codigo == 1010)
                this.cboxDestino.SelectedIndex = 2;
            else if (this._avion.Codigo == 1015)
                this.cboxDestino.SelectedIndex = 3;
            else if (this._avion.Codigo == 1020)
                this.cboxDestino.SelectedIndex = 4;


            this.txtMatricula.Text = this._avion.Matricula.ToString();
            this.txtMarca.Text = this._avion.Marca.ToString();
            this.txtModelo.Text = this._avion.Modelo.ToString();
            this.txtCapacidad.Text = this._avion.Capacidad.ToString();

           
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            this._avion = new Avion();
            int codigo;

            if (this.cboxDestino.SelectedIndex == 0)
                codigo = 1000;
            else if (this.cboxDestino.SelectedIndex == 1)
                codigo = 1005;
            else if (this.cboxDestino.SelectedIndex == 2)
                codigo = 1010;
            else if (this.cboxDestino.SelectedIndex == 3)
                codigo = 1015;
            else
                codigo = 1020;

            this._avion.Matricula = int.Parse(this.txtMatricula.Text);
            this._avion.Marca = this.txtMarca.Text;
            this._avion.Modelo = this.txtModelo.Text;
            this._avion.Capacidad = int.Parse(this.txtCapacidad.Text);
            this._avion.Codigo = codigo;


            this.DialogResult = DialogResult.OK;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void btnAceptar_Click_1(object sender, EventArgs e)
        {
            this._avion = new Avion();
            int codigo;

            if (this.cboxDestino.SelectedIndex == 0)
                codigo = 1000;
            else if (this.cboxDestino.SelectedIndex == 1)
                codigo = 1005;
            else if (this.cboxDestino.SelectedIndex == 2)
                codigo = 1010;
            else if (this.cboxDestino.SelectedIndex == 3)
                codigo = 1015;
            else
                codigo = 1020;

            this._avion.Matricula = int.Parse(this.txtMatricula.Text);
            this._avion.Marca = this.txtMarca.Text;
            this._avion.Modelo = this.txtModelo.Text;
            this._avion.Capacidad = int.Parse(this.txtCapacidad.Text);
            this._avion.Codigo = codigo;

            this.DialogResult = DialogResult.OK;
        }

        private void btnCancelar_Click_1(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }



    }
}
