using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using stdole;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.ArcMapUI;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.ADF.COMSupport;
using autoPlanos.Diccionary;
using B = featureTools.feature;
using G = geoProcesos.tools;
using System.Collections;

namespace autoPlanos.Clases
{
    class clsMapTools
    {
        public string nomPlano;
        clsToys funcAux= new clsToys();
        clsFeatureTools lTools = new clsFeatureTools();
        public void zoomToPageLayout(IMxDocument pMxDoc)
        {
            double escalaD = 0;
            long escala = 0;
            try{
                IMap pMap = pMxDoc.FocusMap;
                escalaD = pMap.MapScale;
                escala = funcAux.redondea(Convert.ToInt64(escalaD), 1, 1000);
                pMap.MapScale = escala;
            }catch (System.Exception ex) {
                MessageBox.Show("Error: " + ex.Message, "zoomToPageLayout");
                MessageBox.Show("Error: " + ex.StackTrace);
            }
        }
        public string seleccionaDir()
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            string apppath = null;
            dialog.RootFolder = Environment.SpecialFolder.Desktop;
            dialog.SelectedPath = globales.planos;
            dialog.Description = "Seleccione la ruta destino";
            if (dialog.ShowDialog() == DialogResult.OK)
                apppath = dialog.SelectedPath;
            else
                apppath = globales.planos;
            apppath = apppath + "\\";
            return apppath;
        }
        public IElement cambiaEscudo(int clvMunicipio, IPageLayout pPageLayout){
            IPictureElement pPicElement = new BmpPictureElementClass();
            string escudo = null;
            try{
                escudo = funcAux.buscaEscudo(clvMunicipio);
                pPicElement.ImportPictureFromFile(escudo);
                pPicElement.MaintainAspectRatio = true;

                //Set the New Element to the Picture Element
                IElement pElement = (IElement)pPicElement;

                //Create Target Envelope
                IEnvelope pEnv = new EnvelopeClass();

                pEnv.PutCoords(35.6, 24.75, 36.44, 25.78);
                pElement.Geometry = pEnv;

                //set the container as the pagelayout and add the element created
                IGraphicsContainer pGraphicsContainer = (IGraphicsContainer)pPageLayout;
                pGraphicsContainer.AddElement(pElement, 0);

                IActiveView pActiveView = (IActiveView)pPageLayout;
                pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, pElement, pEnv);
                return pElement;
            } catch (System.Exception ex) {
                MessageBox.Show("Error: " + ex.Message);
                MessageBox.Show("Error: " + ex.StackTrace, "clsMapTools.cambiaEscudo");
                return null;
            }
        }
       
        public void zoomToCapaPageLayout(IMxDocument pMxDoc){
           // double escalaD = 0;
           // long escala = 0;
            try{
                IMap pMap = pMxDoc.FocusMap;
                pMap.MapScale *= (1.108);//crea un zoomout de +12%
               /* escalaD = pMap.MapScale;
                escala = funcAux.redondea(Convert.ToInt64(escalaD), 1, 1000);
                pMap.MapScale = escala;*/
            }catch (System.Exception ex){
                MessageBox.Show("Error: " + ex.Message, "zoomToCapaPageLayout");
                MessageBox.Show("Error: " + ex.StackTrace);
            }
        }
        public void ToggleActiveView(IMxDocument pMxDoc)
        {
            if (!(pMxDoc.ActiveView is IPageLayout))
                pMxDoc.ActiveView = (IActiveView)pMxDoc.PageLayout;
        }
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public List<IElement> discrimaDatosLayer(string n_municipio, string n_plano, string t_plano, IMxDocument pMxDoc, ILayer municipios, ILayer seleccion, ILayer zona, ILayer calles)
        {
           // clsGeoTools arcpy =new clsGeoTools();
            //IElement[] functionReturnValue = null;
            ILayer MANZANAS;
            List<IElement> pElem = new List<IElement>();
            List<IElement> pElemU = new List<IElement>();
            ArrayList nom_plano = null;
            IMap pMap = pMxDoc.FocusMap;
         //  ILayer municipio = default(ILayer);
            ILayer zonasValor = default(ILayer);
            ILayer seleccionZona = default(ILayer);
            ILayer manzana;
            string p;
            int n = 17;
            MANZANAS = B.returnLayerByName(ArcMap.Document, "MANZANAS");
            try {
                nom_plano = (B.returnFieldData( B.returnFeatureLayerByName(ArcMap.Document, "municipio").FeatureClass, "nombre"));
                foreach (string itm in funcAux.StatUnique(nom_plano)){
                    nomPlano = itm;
                    p = funcAux.checaPalabra(nomPlano,n);
                    //MessageBox.Show(p, nomPlano);
                    if (p.Length >n) 
                        pElem.Add( addTitleToLayout(pMxDoc, 9/*10*/, 74, 47.2, (IGraphicsContainer)pMxDoc.PageLayout, true, p.ToUpper())) ;
                    else
                        pElem.Add(addTitleToLayout(pMxDoc, 9/*10*/, 74, 48, (IGraphicsContainer)pMxDoc.PageLayout, true, p.ToUpper()));
                    break;
                }

                G.selectLayerByAttribute(seleccion, "municipio = " + n_municipio + " AND n_plano = " + n_plano, true);
                G.copyFeatures(seleccion, globales.gdb + "seleccionZona");
               // G.selectClip(seleccion, municipio, globales.gdb + "seleccionZona");
               /*Inserta el numero del plano*/
                pElem.Add( addTitleToLayout(pMxDoc, 6.0, 74.3, 9, (IGraphicsContainer)pMxDoc.PageLayout, false, n_plano + "   de   " + t_plano));
                seleccionZona = B.returnLayerByName(ArcMap.Document, "seleccionZona");

                G.selectClip(zona, seleccionZona, globales.gdb + "zonasValor");
                //B.limpiarZonasTmp(ArcMap.Document, "municipio");
                zonasValor = B.returnLayerByName(ArcMap.Document, "zonasValor");
                // estilado ZonaValor 
                G.applySymbologyFromLayer(zonasValor, zona);
                /*Inserta los valores de las zonas*/
               // addMapSurroundM(pMxDoc, "zonasValor", "clave_zona", "Valor", 72.299, 76.00,ref pElem);
               	pMap.ClearSelection();
                G.selectClip(MANZANAS, zonasValor, globales.gdb + "manzana");
                //G.selectClip(calles, seleccionZona, globales.gdb + "calle");
                manzana = B.returnLayerByName(ArcMap.Document, "manzana");
                G.applySymbologyFromLayer(manzana, MANZANAS);
                pMap.ClearSelection();
                B.limpiarZonasTmp(ArcMap.Document, "seleccionZona");
                /******************************************Genera el macro mapa******************************************************/
                IMapDocument pMapDoc = new MapDocumentClass();
                IMap pMap2 = pMxDoc.Maps.get_Item(1);
                
                pMxDoc.ActiveView = (IActiveView)pMap2;
                string mun = "municipio";
                ILayer SELECCION = B.returnLayerByName(ArcMap.Document, mun);
                G.selectLayerByAttribute(SELECCION, "municipio = " + n_municipio, true);

                B.zoomToSeleccion(pMxDoc, mun);
                pMap2.MapScale *= (1.12);
                pMap2.ClearSelection();

                pMxDoc.ActiveView.Refresh();
                pMap = pMxDoc.Maps.get_Item(0);
                pMxDoc.ActiveView = (IActiveView)pMap;
                pMxDoc.ActiveView = (IActiveView)pMxDoc.PageLayout;
                /******************************************Fin de el macro mapa******************************************************/
                return pElem;
            } catch (System.Exception ex) {
                MessageBox.Show("Error: " + ex.Message, "clsMapTools.discrimaDatosLayer");
                MessageBox.Show("Error: " + ex.StackTrace);
                return null;
            }
        }
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public List<IElement> addMapSurroundM(IMxDocument pMxDoc, string nombreLayer, string campoLayer, string campoLayer2, double x, double x2,ref  List<IElement> pElem)
        {
            List<objetos> clave_zona = null;
            string clave = null;
            string[] arrayTmp = null;
            int j = 0;
            int i;
            IFeatureClass fc = default(IFeatureClass);
            int n = 0;
            double y = 0;
           // = new List<IElement>(); 
            fc = B.returnFeatureLayerByName(ArcMap.Document, nombreLayer).FeatureClass;
            try {
                clave_zona = lTools.returMatrizDataUnique(fc, campoLayer, campoLayer2);
                n = clave_zona.Count();
                y = 35.07;
                j = 0;
                for (i = 0; i <n; i++) {
                    Array.Resize(ref arrayTmp, i + 1);
                    arrayTmp[i] = clave_zona[i].clave;
                    
                }
                System.Array.Sort(arrayTmp);
                vDiccionario v = new vDiccionario(clave_zona);
                for (i = 0; i < n; i++) {
                    //Array.Resize(ref pElem, j + 1);
                    clave = v.sayValorZ(arrayTmp[i]);
                    clave = "0";
                    clave = funcAux.convierteaMoneda(clave);
                    IElement m = addTitleToLayout(pMxDoc, 7.5, x2, y, (IGraphicsContainer)pMxDoc.PageLayout, false, clave);
                    pElem.Add(m);
                    j = j + 1;
                    y = y - 1.532;
                }
                return pElem;
            } catch (System.Exception ex) {
                MessageBox.Show("Error: " + ex.Message, "clsMapTools.addMapSurroundM");
                MessageBox.Show("Error: " + ex.StackTrace);
                return null;
            }
        }
        public IElement addTitleToLayout(IMxDocument pMxDoc,double S, double x, double y,  IGraphicsContainer pGc, bool b, string texto) {
            ITextElement pTxtElem;
            ITextSymbol pTxtSym = default(ITextSymbol);
            IRgbColor myColor = default(IRgbColor);
            stdole.IFontDisp myFont;
            IElement pElem;
            IEnvelope pEnv;
            IPoint pPoint = default(IPoint);
            try{
                // pGc.Reset()
                //Set the font and color properties
                //for the title
                myFont = (stdole.IFontDisp)new stdole.StdFont();
                myFont.Name = "Times New Roman";
                myFont.Bold = b;
               // myFont.Size = S;                
                myColor = new RgbColor();
                myColor.Red = 0;
                myColor.Green = 0;
                myColor.Blue = 0;
                ///''''''''''''''''''''''''''''''
                //Create a text element
                pTxtElem = (ITextElement)new TextElement();
                
                //Create a text symbol
                pTxtSym = new TextSymbol();
                pTxtSym.Color = myColor;
                pTxtSym.Font = myFont;
                pTxtSym.Size = S;
                //Set symbol property
                pTxtElem.Symbol = pTxtSym;

                //set the text property to be the layer's name (Uppercase)
                pTxtElem.Text = texto;

                //Create an envelope for the TextElements Geometry
                pEnv = (IEnvelope)new Envelope();
                pPoint = new ESRI.ArcGIS.Geometry.Point();

                pPoint.X = x;
                pPoint.Y = y;
                pEnv.LowerLeft = pPoint;
                pPoint.X = 1;
                pPoint.Y = 1;
                pEnv.UpperRight = pPoint;
                //set the text elements geomtery
                pElem = (IElement)pTxtElem;
                pElem.Geometry = pEnv;

                //Add the element to the graphics container
                pGc.AddElement(pElem, 1);
                pMxDoc.ActiveView.Refresh();
                return (pElem);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "clsMapTools.addTitleToLayout");
                MessageBox.Show("Error: " + ex.StackTrace);
                return null ;
            }
        }

    }
}
