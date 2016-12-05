using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Aeropuerto_ConADO.NET
{
    public partial class frmMostrar : Form
    {
        public frmMostrar()
        {
            InitializeComponent();
            
        }


        public frmMostrar(string queHacer, DataSet ds)
            :this()
        {
            this.ConfigurarDataGridView();

            switch (queHacer)
            {
                case "aviones":
                    this.dgvData.DataSource = ds.Tables["Aviones"];
                    break;

                case "vuelos":
                    this.dgvData.DataSource = ds.Tables["Vuelos"];
                    break;
            }
        }


        private void ConfigurarDataGridView()
        {
            //PERMISOS SOBRE DATAGRID
            this.dgvData.AllowUserToResizeRows = false;
            this.dgvData.AllowUserToAddRows = false;
            this.dgvData.AllowUserToResizeColumns = false;
            this.dgvData.AllowUserToDeleteRows = false;


            //ESTILOS FILAS
            this.dgvData.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Sunken;
            this.dgvData.RowHeadersVisible = false;
            this.dgvData.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            //ESTILOS COLUMNAS
            this.dgvData.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.dgvData.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.dgvData.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Raised;
            this.dgvData.BorderStyle = BorderStyle.Fixed3D;
            this.dgvData.CellBorderStyle = DataGridViewCellBorderStyle.Raised;
            this.dgvData.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;

        }





    }
}
