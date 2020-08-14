using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Collections;
using System.Globalization;
using System.Data;
using b = biblioteca.configuracion;

namespace autoPlanos.Clases
{
    class clsToys
    {
        public string checaPalabra(string palabra,int n){
            int i = 1;
            int e = 0;
            int espacio = 0;
            try{
                if (palabra.Length > n) {
                    String pal = palabra;
                    while (i < n){
                        e = pal.IndexOf(" ", i);
                        if (e > 0)
                            i = espacio = e;
                        i = i + 1;
                    }
                    palabra = insertIntro(palabra, espacio);
                    return palabra;
                }
                else
                    return palabra;
            }catch (System.Exception ex){
                MessageBox.Show("Error: " + ex.Message + "\n" + ex.StackTrace, "checaPalabra");
                return null;
            }
        }
        private string insertIntro(string p, int n){
            string a = null;
            string b = null;
            a = p.Substring(0, n);
            b = p.Remove(0, a.Length + 1);
            return (a + Environment.NewLine + b);
        }
        public string convierteaMoneda(string cantidad){
            double m = 0;
            if (cantidad !="")
                m = Convert.ToDouble(cantidad);

            return m.ToString("C", CultureInfo.CurrentCulture);
        }
        public long redondea(long numero, long m, long tope){
            long r = 0;
            long res = 0;
            long n = 0;
            r = numero % (10 * m);
            // para redondear a la siguiente mayor a 5 (r > (5 * m))
            //para redondear a la menr siguiente (r > (9 * m))
            if ((r > (1 * m)))
                res = (10 * m) - r;
            else
                res = r * -1;
            n = numero + res;
            if ((m >= tope))
                return numero;
            return redondea(n, (10 * m), tope);
        }
        public string extraePalabra(string palabra, string busqueda){
            int result = 0;
            int final = 0;
            string palabraResul = null;
            palabraResul = "ERROR";
            try{
                result = -1;
                final = busqueda.Length;
                if ((palabra.Length > 2) && (final > 0)) {
                    if (String.Equals(busqueda, palabra) ){
                        palabraResul = palabra;
                        return palabraResul;
                    }
                    result = palabra.IndexOf(busqueda);
                    if (result > 0){
                        palabraResul = palabra.Substring(result, final);
                        return palabraResul;
                    }
                }
                return palabraResul;
            } catch (Exception ex){
                MessageBox.Show("Error: " + ex.Message + "\n" + ex.StackTrace, "extraePalabra");
            }
            return palabraResul;
        }
        public ArrayList StatUnique(ArrayList col){
            ArrayList unique = new ArrayList();
            bool bFound = false;
            foreach (var item in col) {
                bFound = false;
                foreach (var value in unique) 
                    if (item.ToString() == value.ToString()) {
                        bFound = true;
                        break;
                    }
                if (!bFound)
                    unique.Add(Convert.ToString(item));
            }
            return unique;
        }
        public String buscaEscudo(int codigo){
            Byte[] bytes;
            try { 
                long dt = DateTime.Now.ToFileTime();
                DataTable tbl = b.busquedaMunicipio(codigo);
                bytes = (Byte[])(tbl.Rows[0][2]);
                string archivoimagen = globales.ruta + dt.ToString() + ".png";
                //abriendo el archivo para su creacion en memoria con(filestream) 
                System.IO.FileStream fs = new System.IO.FileStream(archivoimagen, System.IO.FileMode.CreateNew, System.IO.FileAccess.Write);
                fs.Write(bytes, 0, bytes.Length);
                fs.Close();
                return archivoimagen;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message + "\n" + ex.StackTrace, "buscaEscudo");
                return null;
            }
        }
        public List<objetos> eliminaDuplicadosM(ArrayList Arr, ArrayList eglo){
            List<objetos> lists = new List<objetos>();
            bool b = false;
            int n = 0;
            int i = 0;
            i = 0;
            try {
                insertaDatosM(Arr[i].ToString(), eglo[i].ToString(),  ref lists);
                n = Arr.Count;
                for (i = 1; i < n; i++){
                    b = true;
                    for (int j = 0; j < i; j++)
                        if (Arr[i].ToString() == Arr[j].ToString()){
                            b = false;
                            break;
                        }
                    if (b)
                        insertaDatosM(Arr[i].ToString(), eglo[i].ToString(), ref lists);
                }
                return lists;
            }catch (System.Exception ex){
                MessageBox.Show("Error: " + ex.Message + "\n" + ex.StackTrace, "eliminaDuplicadosM");
                return null;
            }
        }
        public void insertaDatosM(string a, string b, ref  List<objetos> lists){
            objetos M = new objetos();
            M.clave = a;
            M.valor = b;
            lists.Add(M);
        }
    }
    public class objetos {
        public string clave { get; set; }
        public string valor { get; set; }
    }
}
