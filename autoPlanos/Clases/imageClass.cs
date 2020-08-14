using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using b = biblioteca.configuracion;
namespace autoPlanos.Clases
{
    class imageClass
    {
        #region Set Class Variables …
        Byte[] bytes;//arreglo de bytes para la imagen
        private imageClass clsobj;
        private List<imageClass> lstobj;
        #endregion
        #region Set Property Data …
        private int _id_escudo;
        public int id_escudo
        {
            get { return _id_escudo; }
            set { _id_escudo = value; }
        }
        private string _municipio;
        public string municipio
        {
            get { return _municipio; }
            set { _municipio = value; }
        }
        private byte[] _imagen;
        public byte[] Imagen
        {
            get { return _imagen; }
            set { _imagen = value; }
        }
        #endregion
        public imageClass ObtenerImagen(DataTable tbl, int i)
        {
            try{
                clsobj = new imageClass();
                clsobj.id_escudo = Convert.ToInt32(tbl.Rows[i][0]);
                clsobj.municipio = tbl.Rows[i][1].ToString();
                bytes =( Byte[])(tbl.Rows[i][2]);
                clsobj.Imagen = bytes;
            }catch (Exception ex){
                MessageBox.Show("Error: " + ex.Message+"\n" + ex.StackTrace, "ObtenerImagen");
            }
            return clsobj;
        }
        public List<imageClass> ObtenerImagenes()
        {
            try{
                lstobj = new List<imageClass>();
                lstobj.Clear();
                DataTable tbl =b.mostrarMunicipios();
                for (int i = 0; i < tbl.Rows.Count ; i++)
                    lstobj.Add(ObtenerImagen(tbl, i));
            }catch (Exception ex){
                MessageBox.Show("Error: " + ex.Message + "\n" + ex.StackTrace, "ObtenerImagenes");
            }
            return lstobj;
        }
    }
}
