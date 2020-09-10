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
            String  nombreDestino;
            String  directorioDestino;
            List<IElement> pElem = null;
            chkCapas V = new chkCapas();
            int n;
            try {
                string capaFaltante = V.validaProyecto(ArcMap.Document);
                if (capaFaltante != ".") {
                    MessageBox.Show("No se encuentra la capa: " + capaFaltante, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                plano.capasOnOff(false);
                string lMun = Microsoft.VisualBasic.Interaction.InputBox("Municipios (en numero, separados por comas)", "Municipio?");
                string[] valores = lMun.Split(',');
                if (valores[0] == "")
                    return;
                SELECCION = B.returnLayerByName(ArcMap.Document, globales.nomlayerSELECT);
                directorioDestino =  plano.seleccionaDir();
                MUNICIPIOS = B.returnLayerByName(ArcMap.Document, globales.nomlayerMUNICIPIOS);
                string[] mun = lMun.Split(',');
                foreach (var Value in mun) {
                    if (Value == "")
                        continue;
                    B.limpiarLayerTmp(ArcMap.Document);
                    IGraphicsContainer pGraphicsContainer;
                    n = B.cuentaSeleccion("municipio =" + Value.ToString(), SELECCION);
                    ZONA = B.returnLayerByName(ArcMap.Document, "ZONA");
                    clsExport  exporta = new clsExport();for (int i = 1; i <= n; i++) {
                        if (n == 0)
                            break;
                            G.selectLayerByAttribute(SELECCION, "n_plano =" + Convert.ToString(i) + " AND municipio =" + Value, false);
                            B.zoomToSeleccion(pMxDoc, globales.nomlayerSELECT);
                            plano.ToggleActiveView(pMxDoc);
                            pElem =  plano.discrimaDatosLayer(Value.ToString(), Convert.ToString(i), n.ToString(),  pMxDoc, MUNICIPIOS,  SELECCION,  ZONA);
                            plano.zoomToPageLayout(pMxDoc);
                            plano.resizeIGrid(pMxDoc);
                            nombreDestino = plano.nomPlano + " " + Convert.ToString(i) + " de " + n.ToString();
                            B.layerVisible(ArcMap.Document, "manzana", true);
                            B.layerVisible(ArcMap.Document, "zonasValor", true);
                            exporta.exportMapa("PDF", 379, 1, false, nombreDestino,directorioDestino);
                            pGraphicsContainer = (IGraphicsContainer)pMxDoc.PageLayout;
                            pMxDoc.ActiveView = (IActiveView)pMxDoc.FocusMap;
                            foreach (IElement elemento in pElem)
                                pGraphicsContainer.DeleteElement(elemento);
                        }
                }
                B.limpiarZonasTmp(ArcMap.Document, "municipioszv");
                Cursor.Current = Cursors.Default;
                MessageBox.Show("Proceso terminado");
                plano.capasOnOff(true);
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
