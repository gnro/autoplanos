using SpreadsheetLight;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace autoPlanos.Forms
{
    public partial class frmCreaTabla : Form
    {
        string txtMunicipio;
        string nomExcel;
        public frmCreaTabla()
        {
            InitializeComponent();
        }
        public frmCreaTabla(string nomMunicipio, string archivoExcel)
        {
            InitializeComponent();
            txtMunicipio = nomMunicipio;
            nomExcel = archivoExcel;
        }

        private void frmCreaTabla_Load(object sender, EventArgs e)
        {
            LLenarGridLIST();
        }
        private void LLenarGridLIST()
        {
            FileStream fs = new FileStream(nomExcel, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            /**/
            SLDocument documento = new SLDocument(fs);
            int rowIndex = 8;
            while (!string.IsNullOrEmpty(documento.GetCellValueAsString(rowIndex, 4)))
            {
                string comp1 = documento.GetCellValueAsString(rowIndex, 4).ToString();
                if (comp1 == txtMunicipio)
                {
                    string zona = documento.GetCellValueAsString(rowIndex, 5);
                    double pesos = documento.GetCellValueAsDouble(rowIndex, 6);

                    ListViewItem fila = new ListViewItem(zona);
                    fila.SubItems.Add(pesos.ToString("C", new CultureInfo("us-US")));
                    listView1.Items.Add(fila);
                }
                rowIndex += 1;
            }
            listView1.Height = listView1.Height + (listView1.Items.Count * 17);
            listView1.GridLines = true;
            listView1.BackColor = Color.White;
        }
    }
}
