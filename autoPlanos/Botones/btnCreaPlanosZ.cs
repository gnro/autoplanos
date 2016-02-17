using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using ESRI.ArcGIS.ArcMapUI;
using ESRI.ArcGIS.ADF.Connection.Local;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Framework;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Output;
using System.Windows.Forms;
using autoPlanos.Clases;
using autoPlanos.Diccionary;
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
        internal System.ComponentModel.BackgroundWorker hiloSegundoPlano;
#region "Delete Files from Directory"

///<summary>Deletes from a specified directory that match a certain pattern.</summary>
/// 
///<param name="filePath">A System.String that is the file and path from where to delete files. Example: "C:\temp"</param>
///<param name="pattern"> A System.String that is the pattern for files to delete. Example: "*.txt" or "myfile.*" or "*.*"</param>
///  
///<remarks>All files meeting the pattern will be deleted from the specified directory.</remarks>
public void DeleteFilesFromDir(String filePath, String pattern)
{
    try{
        System.IO.DirectoryInfo directoryInfo = new System.IO.DirectoryInfo(filePath);
        System.IO.FileInfo[] files = directoryInfo.GetFiles(pattern);
        foreach (System.IO.FileInfo file in files)
            file.Delete();
    }
    catch (System.Exception ex)
    {
        return;
    }
}
#endregion
        public btnCreaPlanosZ()
        {
            DeleteFilesFromDir(globales.ruta, "*.*");
        }
        System.Windows.Forms.ProgressBar pb;
        private void CrearProgressBars(int sX, int sY, int sAltura, int sAncho)
        {
             pb= new System.Windows.Forms.ProgressBar();
            pb.SetBounds(sX, sY, sAncho, sAltura);
           // pb.Parent = this;
            pb.Visible = true;
            pb.CreateControl();
            pb.Maximum = 100;
            pb.Minimum = 0;
            pb.Name = "progressBar";
            pb.Step = 1;
            pb.Value = 1;
            pb.PerformStep();
            
        }
        protected override void OnClick()
        {
           // System.Windows.Forms.ProgressBar progressBar;
            ILayer SELECCION;
            IMxDocument pMxDoc = ArcMap.Document;
            clsMapTools plano = new clsMapTools();
            ArrayList mun = new ArrayList();
            String nombreDestino;
            String directorioDestino;
          /*  hiloSegundoPlano = new System.ComponentModel.BackgroundWorker();
            progressBar = new System.Windows.Forms.ProgressBar();
            progressBar.Location = new System.Drawing.Point);
            progressBar.Size = new System.Drawing.Size(
            progressBar.TabIndex = 0;*/
            CrearProgressBars(10, 10,334, 34);
            List<IElement> pElem = null;
            int n;
            try
            {
                B.layerVisible(ArcMap.Document, globales.nomlayerSELECT, false);
                B.layerVisible(ArcMap.Document, "ZONA", false);
                B.groupLayerVisible(ArcMap.Document, "Referencia", false);
                B.groupLayerVisible(ArcMap.Document, "Cartografia", false);
                if (DialogResult.Yes == MessageBox.Show("Listo?", "Atención", MessageBoxButtons.YesNo))
                {/*
                    progressBar.Value = 50;
                   *** funcion.Show();*/
                    SELECCION = B.returnLayerByName(ArcMap.Document, globales.nomlayerSELECT);
                    directorioDestino = plano.seleccionaDir();
                    MUNICIPIOS = B.returnLayerByName(ArcMap.Document, globales.nomlayerMUNICIPIOS);
                    G.selectLayerByLocation(MUNICIPIOS, SELECCION, "INTERSECT");
                    G.copyFeatures(MUNICIPIOS, globales.gdb + "municipioszv");
                    mun = B.listadeValores(ArcMap.Document, "municipioszv", "municipio");
                    B.layerVisible(ArcMap.Document, "municipioszv", false);

                    //forma.incrementPercent(C);
                    foreach (var Value in mun) {
                        B.limpiarLayerTmp(ArcMap.Document);
                        IGraphicsContainer pGraphicsContainer;
                        n = B.cuentaSeleccion("municipio =" + Value.ToString(), SELECCION);
                        ZONA = B.returnLayerByName(ArcMap.Document, "ZONA");
                        CALLES = B.returnLayerByName(ArcMap.Document, "CALLES");
                        clsExport exporta = new clsExport();
       
                ILayer municipio = default(ILayer);
                G.selectLayerByAttribute(MUNICIPIOS, "municipio = " + Value.ToString(), true);
                G.copyFeatures(MUNICIPIOS, globales.gdb + "municipio");
                IElement m = plano.cambiaEscudo(Convert.ToInt16(Value.ToString()), pMxDoc.PageLayout);
                municipio = B.returnLayerByName(ArcMap.Document, "municipio");
                        for (int i = 1; i <= n; i++) {
                            if (n == 0)
                                break;
                            G.selectLayerByAttribute(SELECCION, "n_plano =" + Convert.ToString(i) + " AND municipio =" + Value, false);
                            B.zoomToSeleccion(pMxDoc, globales.nomlayerSELECT);
                            plano.ToggleActiveView(pMxDoc);
                            plano.zoomToCapaPageLayout(pMxDoc);
                            pElem = plano.discrimaDatosLayer(Value.ToString(), Convert.ToString(i), n.ToString(), pMxDoc, MUNICIPIOS, SELECCION, ZONA, CALLES);
                pElem.Add(m);

                            nombreDestino = plano.nomPlano + " " + Convert.ToString(i) + " de " + n.ToString();
                            // B.layerVisible(ArcMap.Document, "calle", false);
                            //B.layerVisible(ArcMap.Document, "calle", true);
                            B.layerVisible(ArcMap.Document, "manzana", true);
                            B.layerVisible(ArcMap.Document, "zonasValor", true);
                            B.layerVisible(ArcMap.Document, "municipio", false);

                            exporta.exportMapa("PDF", 379, 1, false, nombreDestino, directorioDestino);
                            pGraphicsContainer = (IGraphicsContainer)pMxDoc.PageLayout;
                            pMxDoc.ActiveView = (IActiveView)pMxDoc.FocusMap;
                            /*
                                                        if (MessageBox.Show("continuar?", "...", MessageBoxButtons.YesNo) == DialogResult.No)
                                                            return;*/
                            foreach (IElement elemento in pElem)
                                pGraphicsContainer.DeleteElement(elemento);
                        }
                        B.limpiarZonasTmp(ArcMap.Document, "seleccionZona");
                        //C += 10;
                        //forma.incrementPercent(C);
                    }
                    B.layerVisible(ArcMap.Document, globales.nomlayerSELECT, true);
                    B.limpiarZonasTmp(ArcMap.Document, "municipioszv");
                    B.layerVisible(ArcMap.Document, "ZONA", true);
                    System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
                    //
                    MessageBox.Show("Proceso terminado");
                }
                B.groupLayerVisible(ArcMap.Document, "Cartografia", true);
                B.groupLayerVisible(ArcMap.Document, "Referencia", true);
                pMxDoc.ActiveView.Refresh();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "btnCreaPlanosZ");
                MessageBox.Show("Error: " + ex.StackTrace);
                }
        }
        protected override void OnUpdate()
        {
           // Enabled = ArcMap.Application != null;
        }
    }

}
