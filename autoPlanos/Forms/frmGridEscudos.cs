using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using autoPlanos.Clases;
using b = biblioteca.configuracion;

namespace autoPlanos.Forms
{
    public partial class frmGridEscudos : Form
    {
    private imageClass objgridimages;
    //private DialogResult result = new DialogResult();
    private OpenFileDialog openFileDialog1 = new OpenFileDialog();
    private Byte[] bytes;//arreglo de bytes para la imagen 
   // private clienteDAO obj = new clienteDAO(); // ClienteDAO 
    int id_object;
        public frmGridEscudos(){
            InitializeComponent();
        }

        private void frmGridEscudos_Load(object sender, EventArgs e){
            cargarGrid();
        }

        private void cmbActualiza_Click(object sender, EventArgs e){
            Actualiza();
            cmbActualiza.Visible = false;
            txtMunicipio.Enabled = true;
        }
        private void txtMunicipio_KeyPress(object sender, KeyPressEventArgs e){
            if (e.KeyChar != 8)
                if((e.KeyChar < 48 || (e.KeyChar > 57)))
                    if (e.KeyChar == (char)Keys.Return){
                        int n = 0;
                        n = Convert.ToInt32(txtMunicipio.Text);
                        if( (n <= gridCatEscudo.RowCount)&&(n>0)){
                            txtMunicipio.Text = "";
                            buscaEscudo(n);
                            gridCatEscudo.Rows[n - 1].Selected = true;
                            gridCatEscudo.CurrentCell = gridCatEscudo.Rows[n - 1].Cells[0];
                        }
                    }
        }
        private void cmbCancelar_Click(object sender, EventArgs e){
            txtMunicipio.Enabled = true; 
            cmbActualiza.Visible = false;
        }
        private void gridCatEscudo_CellDoubleClick(object sender, DataGridViewCellEventArgs e){
            try{
                if (e.RowIndex >= 0 && e.ColumnIndex == 2) {
                    txtMunicipio.Enabled = false;
                    if (iniciaOpenFileDialog() == DialogResult.OK) {
                        picFoto.Image = Image.FromFile(openFileDialog1.FileName);
                        txtMunicipio.Text = gridCatEscudo.Rows[e.RowIndex].Cells[1].Value.ToString();
                        id_object =(int) gridCatEscudo.Rows[e.RowIndex].Cells[0].Value;
                        cmbActualiza.Visible = true;
                    }
                }
            }catch (Exception ex){
                MessageBox.Show("Error: " + ex.Message, "gridCatEscudo_CellDoubleClick");
                MessageBox.Show("Error: " + ex.StackTrace); 
            }
        }
        private void picFoto_DoubleClick(object sender, EventArgs e){
            //if (iniciaOpenFileDialog() == DialogResult.OK) {
            //    String strfoto  = openFileDialog1.FileName;
            //    picFoto.SizeMode = PictureBoxSizeMode.Zoom;
            //    picFoto.Image = Image.FromFile(strfoto);
            //}
        }
        
        private void cargarGrid(){
            objgridimages = new imageClass();
            this.Cursor = Cursors.WaitCursor;
            formatDataGridView();//carta la configueracion del grid
            try{
                gridCatEscudo.DataSource = objgridimages.ObtenerImagenes();
                if (gridCatEscudo.Rows.Count == 0)
                    MessageBox.Show("No existen registros ", "Grid Image", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }catch (Exception ex){
                MessageBox.Show("Error: " + ex.Message, "cargarGrid");
                MessageBox.Show("Error: " + ex.StackTrace); 
            }finally {
                objgridimages = null;
            }
            this.Cursor = Cursors.Default;
        }
        private void formatDataGridView(){
          try{
              gridCatEscudo.AutoGenerateColumns = false;
                gridCatEscudo.Columns.Clear();

                DataGridViewTextBoxColumn columnaIDescudo = new DataGridViewTextBoxColumn();
                columnaIDescudo.DataPropertyName = "id_escudo";
                columnaIDescudo.HeaderText = "No";
                columnaIDescudo.ReadOnly = true;
                columnaIDescudo.Visible = true;
                columnaIDescudo.Width = 30;
                gridCatEscudo.Columns.Add(columnaIDescudo);

                DataGridViewTextBoxColumn columnaNomMunicipio = new DataGridViewTextBoxColumn();
                columnaNomMunicipio.DataPropertyName = "municipio";
                columnaNomMunicipio.HeaderText = "Municipio";
                columnaNomMunicipio.ReadOnly = true;
                columnaNomMunicipio.Visible = true;
                columnaNomMunicipio.Width = 80;
            
                gridCatEscudo.Columns.Add(columnaNomMunicipio);

                DataGridViewImageColumn columnaImagen = new DataGridViewImageColumn();
                columnaImagen.DataPropertyName = "Imagen";
                columnaImagen.HeaderText = "Escudo";
                columnaImagen.ImageLayout = DataGridViewImageCellLayout.Zoom;
                columnaImagen.ReadOnly = true;
                columnaImagen.Visible = true;
                columnaImagen.Width = 100;
            
                gridCatEscudo.Columns.Add(columnaImagen);
              }catch (Exception ex){
                  MessageBox.Show("Error: " + ex.Message, "formatDataGridView");
                  MessageBox.Show("Error: " + ex.StackTrace);
            }
        }
        private void limpiarcontroles() {
            txtMunicipio.Text = "";
            picFoto.Image = null;
            txtMunicipio.Focus();
        }
        private DialogResult iniciaOpenFileDialog(){
            openFileDialog1.InitialDirectory = @"Bibliotecas\Imágenes";
            openFileDialog1.Filter = "jpg|*.jpg|png|*.png|bmp|*.bmp";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;
            
            return (openFileDialog1.ShowDialog());
        }
        private void Actualiza(){
            string id;
            try{
                id = id_object.ToString();
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                picFoto.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                int nres = b.actualizaMunicipio(id, ms);
                if( nres == 1 )
                   // MessageBox.Show("El Municipio fue registrado", "Insercion");
                cmbActualiza.Visible = true;
                cargarGrid();
                limpiarcontroles();
            }catch (Exception ex){
                MessageBox.Show("Error: " + ex.Message, "Actualiza");
                MessageBox.Show("Error: " + ex.StackTrace);
            }
        }
         private void buscaEscudo(int codigo){
             clsToys t = new clsToys();
             try{
                 picFoto.Image = null;
                 picFoto.SizeMode = PictureBoxSizeMode.Zoom;
                 picFoto.Image = Image.FromFile(t.buscaEscudo(codigo));
             }
             catch (Exception ex)
             {
                 MessageBox.Show("Error: " + ex.Message, "buscaEscudo local");
                 MessageBox.Show("Error: " + ex.StackTrace); 
                 return;
             }
        }

       
    }
}
