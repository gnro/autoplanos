using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.ArcMapUI;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Display;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace autoPlanos.Clases
{
    class clsFeatureTools
    {
        public List<objetos> returMatrizDataUnique(IFeatureClass fc, string fld, string fld2)
        {
            int iFldIndex = fc.Fields.FindField(fld);
            int iFldIndex2 = fc.Fields.FindField(fld2);
            IFeatureCursor pFCursor = fc.Search(null, false);
            IFeature pFeature = default(IFeature);
            ArrayList col = new ArrayList();
            ArrayList col2 = new ArrayList();
            clsToys toy  = new clsToys();
            List<objetos> matriz = null;
            int i = 0;
            try
            {
                pFeature = pFCursor.NextFeature();
                while (!(pFeature == null))
                {
                    col.Add(pFeature.get_Value(iFldIndex));
                    col2.Add(pFeature.get_Value(iFldIndex2));
                    if (i > 1)
                        if (col[i - 1].ToString() == col[i].ToString())
                        {
                            col.Remove(i);
                            col2.Remove(i);
                            i = i - 1;
                        }
                    i = i + 1;
                    pFeature = pFCursor.NextFeature();
                }
                if (i > 0)
                    matriz = toy.eliminaDuplicadosM(col, col2);
                else
                    toy.insertaDatosM("ERROR", "0", ref matriz);
                return matriz;
            }catch (System.Exception ex){
                MessageBox.Show("Error: " + ex.Message + "\n" + ex.StackTrace, "clsFeatureTools.returMatrizDataUnique");
                return null;
            }
        }
    }
}
