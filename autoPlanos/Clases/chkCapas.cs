using ESRI.ArcGIS.ArcMapUI;
using System.Collections.Generic;
using System.Linq;
using fT = featureTools.feature;

namespace autoPlanos.Clases
{
    public class chkCapas
    {
        public string validaProyecto(IMxDocument pMxDoc)
        {
            List<string> lCapas = globales.ccE;
            List<string> cProyecto = fT.returnAllLayersName(pMxDoc);
            string r = "";
            var result = lCapas.Except(cProyecto).ToList();
            foreach (string R in result)
                r += R + ", ";
            string Rr=r.TrimEnd(' ');
            r = Rr.TrimEnd(',');
            return r+".";
        }
    }
}
