using autoPlanos.Forms;

namespace autoPlanos.Clases
{
    class exporTabla
    {
        public void LLenarGridLIST(string txtMunicipio, string archivoExcel) {
            frmCreaTabla frmCE = new frmCreaTabla(txtMunicipio, archivoExcel);
            frmCE.Show();
        }
    }
}
