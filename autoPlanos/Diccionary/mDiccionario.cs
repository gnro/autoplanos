using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Windows.Forms;
using autoPlanos.Clases;

namespace autoPlanos.Diccionary
{
    class mDiccionario
    {
        //private Dictionary<string, double[]> _dics = new Dictionary<string, double[]>();
        private ArrayList _zonas = new ArrayList();
        public   mDiccionario(){
            string archDicc = "sinonimosClv.geodic";
            try {
                string[] strLineas = File.ReadAllLines(globales.rutaD + archDicc);
                string[] campos; 
                foreach (string linea in strLineas)
                {
                    campos = linea.Split(",".ToCharArray());
                    _zonas.AddRange(campos.ToList());
                }
            }catch (Exception ex) {
                MessageBox.Show("Error: " + ex.Message, "mDiccionario");
                MessageBox.Show("Error: " + ex.StackTrace);
            }
        }
       private  string extraeClaveZona(string color){
           string colorResul = null;
           clsToys tamagochi = new clsToys();
           colorResul = "ERROR";
           try {
               if (color.Length > 2) {
                   foreach (string Value in _zonas) {
                       colorResul = tamagochi.extraePalabra(color, Value);
                   }
               }
           } catch (Exception ex) {
               MessageBox.Show("Error: " + ex.Message, "extraeClaveZona");
               MessageBox.Show("Error: " + ex.StackTrace);
           }
           return colorResul;
       }

    }
}
