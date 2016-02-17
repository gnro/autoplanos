using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.ArcMapUI;
using System.Windows.Forms;
using B = featureTools.feature;
using G = geoProcesos.tools;
using autoPlanos.Clases;

namespace autoPlanos.Botones
{
    public class btnMunicipioAreas : ESRI.ArcGIS.Desktop.AddIns.Button
    {
        IMxDocument pMxDoc =ArcMap.Document;
       // clsGeoTools arcpy = new clsGeoTools();
        ILayer select_area;
        ILayer MUNICIPIOS;
        ArrayList mun= new ArrayList();
      //  clsFeatureTools lTools = new clsFeatureTools();
        //arcpy = new clsGeoTools();
        public btnMunicipioAreas()
        {
        }

        protected override void OnClick(){
          // var mun = default(List);
            try{
                B.limpiarLayerTmp(ArcMap.Document);
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
                select_area = B.returnLayerByName(ArcMap.Document,globales.nomlayerSELECT);
                MUNICIPIOS = B.returnLayerByName(ArcMap.Document, globales.nomlayerMUNICIPIOS);
                G.selectLayerByLocation(MUNICIPIOS, select_area, "INTERSECT");
                G.copyFeatures(MUNICIPIOS, globales.gdb + "municipioszv");
                mun = B.listadeValores(ArcMap.Document, "municipioszv", "municipio");
                foreach (var element in mun)
                        insertaMunicipio(element);
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
                B.limpiarZonasTmp(ArcMap.Document, "municipio");
                B.limpiarZonasTmp(ArcMap.Document, "municipioszv");
                B.limpiarLayerTmp(ArcMap.Document);
                MessageBox.Show("Proceso terminado");
                pMxDoc.ActiveView.Refresh();
            }catch (Exception ex) {
                MessageBox.Show("Error: " + ex.Message, "btnMunicipioAreas");
                MessageBox.Show("Error: " + ex.StackTrace);
                return;
            }
        }
        private void insertaMunicipio(object n_municipio)
        {
            ILayer municipio = default(ILayer);
            try
            {
                G.selectLayerByAttribute(MUNICIPIOS, "municipio = " + n_municipio, true);
                G.copyFeatures(MUNICIPIOS, globales.gdb + "municipio");

                municipio = B.returnLayerByName(ArcMap.Document, "municipio");
                G.selectLayerByLocation(select_area, municipio, "HAVE_THEIR_CENTER_IN");
                G.calculateField(select_area, "municipio", n_municipio.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "insertaMunicipio");
                MessageBox.Show("Error: " + ex.StackTrace);
                return;
            }
        }
        protected override void OnUpdate()
        {
        }
    }
}
