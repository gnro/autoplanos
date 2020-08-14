using System;
using System.Collections.Generic;
using ESRI.ArcGIS.ArcMapUI;
using ESRI.ArcGIS.Carto;
using System.Windows.Forms;
using autoPlanos.Clases;
using B = featureTools.feature;
using G = geoProcesos.tools;
using System.Collections;

namespace autoPlanos.Botones
{
    public class btnCreaPlanosZ : ESRI.ArcGIS.Desktop.AddIns.Button
    {
        ILayer ZONA = default(ILayer);
        ILayer CALLES = default(ILayer);
        ILayer MUNICIPIOS = default(ILayer);

#region "Delete Files from Directory"

///<summary>Deletes from a specified directory that match a certain pattern.</summary>
/// 
///<param name="filePath">A System.String that is the file and path from where to delete files. Example: "C:\temp"</param>
///<param name="pattern"> A System.String that is the pattern for files to delete. Example: "*.txt" or "myfile.*" or "*.*"</param>
///  
///<remarks>All files meeting the pattern will be deleted from the specified directory.</remarks>
    public void DeleteFilesFromDir(String filePath, String pattern) {
            try {
                System.IO.DirectoryInfo directoryInfo = new System.IO.DirectoryInfo(filePath);
                System.IO.FileInfo[] files = directoryInfo.GetFiles(pattern);
                foreach (System.IO.FileInfo file in files)
                    file.Delete();
            }catch (System.Exception ex){
                MessageBox.Show("Error: " + ex.Message + "\n" + ex.StackTrace, "DeleteFilesFromDir");
            }
        }
#endregion
        public btnCreaPlanosZ()
        {
            DeleteFilesFromDir(globales.ruta, "*.*");
        }

        protected override void OnClick()
        {
            ILayer SELECCION;
            IMxDocument pMxDoc = ArcMap.Document;
            clsMapTools plano = new clsMapTools();
            ArrayList mun = new ArrayList();
            String  nombreDestino;
            String  directorioDestino;
            List<IElement> pElem = null;
            int n;
            try{
                B.layerVisible(ArcMap.Document, globales.nomlayerSELECT, false);
                B.layerVisible(ArcMap.Document, "ZONA", false);
                B.groupLayerVisible(ArcMap.Document, "Referencia", false);
                B.groupLayerVisible(ArcMap.Document, "Cartografia", false);
                if (DialogResult.Yes== MessageBox.Show("Listo?", "Atención", MessageBoxButtons.YesNo)){
                    SELECCION = B.returnLayerByName(ArcMap.Document, globales.nomlayerSELECT);
                    directorioDestino =  plano.seleccionaDir();
                    MUNICIPIOS = B.returnLayerByName(ArcMap.Document, globales.nomlayerMUNICIPIOS);
                    G.selectLayerByLocation(MUNICIPIOS, SELECCION, "INTERSECT");//
                    G.copyFeatures(MUNICIPIOS, globales.gdb + "municipioszv");
                    mun = B.listadeValores(ArcMap.Document, "municipioszv", "municipio");
                    B.layerVisible(ArcMap.Document, "municipioszv", false);
                    foreach (var Value in mun){
                        B.limpiarLayerTmp(ArcMap.Document);
                        IGraphicsContainer pGraphicsContainer;
                        n = B.cuentaSeleccion("municipio =" + Value.ToString(), SELECCION);
                        ZONA = B.returnLayerByName(ArcMap.Document, "ZONA");
                        clsExport  exporta = new clsExport();
                        for (int i = 1; i <= n; i++) {
                            if (n == 0)
                                break;
                            G.selectLayerByAttribute(SELECCION, "n_plano =" + Convert.ToString(i) + " AND municipio =" + Value, false);
                            B.zoomToSeleccion(pMxDoc, globales.nomlayerSELECT);
                            plano.ToggleActiveView(pMxDoc);
                            pElem =  plano.discrimaDatosLayer(Value.ToString(), Convert.ToString(i), n.ToString(),  pMxDoc, MUNICIPIOS,  SELECCION,  ZONA,  CALLES);
                            plano.zoomToPageLayout(pMxDoc);
                            plano.resizeIGrid(pMxDoc);
                            nombreDestino = plano.nomPlano + " " + Convert.ToString(i) + " de " + n.ToString();
                            B.layerVisible(ArcMap.Document, "manzana", true);
                            /*if (MessageBox.Show("continuar?", "...", MessageBoxButtons.YesNo) == DialogResult.No)
                                return;*/
                            B.layerVisible(ArcMap.Document, "zonasValor", true);
                            exporta.exportMapa("PDF", 379, 1, false, nombreDestino,directorioDestino);
                            pGraphicsContainer = (IGraphicsContainer)pMxDoc.PageLayout;
                            pMxDoc.ActiveView = (IActiveView)pMxDoc.FocusMap;
                            /*if (MessageBox.Show("continuar?", "...", MessageBoxButtons.YesNo) == DialogResult.No)
                                return;*/
                            foreach (IElement elemento in pElem)
                                pGraphicsContainer.DeleteElement(elemento);
                        }
                    }
                    B.layerVisible(ArcMap.Document, globales.nomlayerSELECT, true);
                    B.limpiarZonasTmp(ArcMap.Document, "municipioszv");
                    B.layerVisible(ArcMap.Document, "ZONA", true);
                    System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
                    MessageBox.Show("Proceso terminado");
                }
                B.groupLayerVisible(ArcMap.Document, "Cartografia", true);
                B.groupLayerVisible(ArcMap.Document, "Referencia", true);
                pMxDoc.ActiveView.Refresh();
            } catch (System.Exception ex){
                MessageBox.Show("Error: " + ex.Message + "\n" + ex.StackTrace, "btnCreaPlanosZ");
            }
        }
        private void guarda(string Arch,string nombreDestino){
            ArcMap.Application.SaveAsDocument(Arch + nombreDestino+".mxd");
        }
        protected override void OnUpdate()
        {
           // Enabled = ArcMap.Application != null;
        }
    }

}
