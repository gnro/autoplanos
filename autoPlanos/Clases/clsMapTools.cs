﻿using System;
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
//using ESRI.ArcGIS.ADF.COMSupport;
//using autoPlanos.Diccionary;
using B = featureTools.feature;
using G = geoProcesos.tools;
using System.Collections;
using autoPlanos.Diccionary;
using ESRI.ArcGIS.Geoprocessor;
using System.Data;

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
                double y = 26.45;
                double x = 36.25;
                pEnv.PutCoords(x, (y-2.8),(x+2.48), y);//Cambia el tamaño
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
        public void resizeMeasuredGrid(IMap pMap, IMapGrid mapGrid)
        {
            try
            {
           // IMeasuredGrid measuredGrid = new MeasuredGridClass();
            //mapGrid = measuredGrid as IMapGrid;
            IMeasuredGrid measuredGrid = mapGrid as IMeasuredGrid;

            //Set the IMeasuredGrid properties.
            //Origin coordinates and interval sizes are in map units.
            measuredGrid.FixedOrigin = true;
            measuredGrid.Units = pMap.MapUnits;
            double escalaD = pMap.MapScale;
            double n = (escalaD / 20);// *100;
            long escala = (long)n;// funcAux.redondea(Convert.ToInt64(escalaD / 4), 1, 10);
            
            measuredGrid.XIntervalSize = escala; //Meridian interval.
            measuredGrid.YIntervalSize = funcAux.redondea(Convert.ToInt64(escalaD / 16), 1, 10); //Parallel interval.

            //Set the IProjectedGrid properties.
            IProjectedGrid projectedGrid = measuredGrid as IProjectedGrid;
            projectedGrid.SpatialReference = pMap.SpatialReference;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "zoomToCapaPageLayout");
                MessageBox.Show("Error: " + ex.StackTrace);
            }
        }
        public void resizeIGrid(IMxDocument pMxDoc){
            IMap map = pMxDoc.FocusMap;
            try
            {
                IGraphicsContainer graphicsContainer = pMxDoc.PageLayout as IGraphicsContainer;
                IFrameElement frameElement = graphicsContainer.FindFrame(map);
                IMapFrame mapFrame = frameElement as IMapFrame;
                IMapGrids mapGrids = mapFrame as IMapGrids;

                IMapGrid mapGrid = null;
                if (mapGrids.MapGridCount > 0)
                {
                    mapGrid = mapGrids.get_MapGrid(0);
                    resizeMeasuredGrid(map,mapGrid);
                }
                else
                {
                    MessageBox.Show("No grid found.");
                }
               
                return;
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
            //IElement[] functionReturnValue = null;HorizontalAlignmentRightToLeft
            ILayer MANZANAS;
            List<IElement> pElem = new List<IElement>();
            List<IElement> pElemU = new List<IElement>();
            ArrayList nom_plano = null;
            IMap pMap = pMxDoc.FocusMap;
            ILayer zonasValor = default(ILayer);
            ILayer seleccionZona = default(ILayer);
            ILayer selectArea = default(ILayer);
            ILayer manzana;
            string p;
            int n = 17;
            MANZANAS = B.returnLayerByName(ArcMap.Document, "MANZANAS");
            try {
                G.selectLayerByAttribute(municipios, "municipio = " + n_municipio, true);
                G.copyFeatures(municipios, globales.gdb + "municipio");
               // //////////////////////////////-----------/////////////////////////////
                IElement m = cambiaEscudo(Convert.ToInt16(n_municipio), pMxDoc.PageLayout);
                pElem.Add(m);
  #region Nmbre del plano y titulo de municipio
                nom_plano = (B.returnFieldData(B.returnFeatureLayerByName(ArcMap.Document, "municipio").FeatureClass, "NOMBRE_MUNICIPIO"));
                foreach (string itm in funcAux.StatUnique(nom_plano)){
                    nomPlano = itm;
                    p = funcAux.checaPalabra(nomPlano,n).ToUpper();
                    //Nombre del municipio
                    if (p.Length >n) 
                        /*pElem.Add( addTitleToLayout(pMxDoc, TamañoLetra, X, Y, (IGraphicsContainer)pMxDoc.PageLayout, true, p.ToUpper())) ;*/
                        pElem.Add(addTitleToLayout(pMxDoc, 8, 76.09 - (n / 7), 43.15, (IGraphicsContainer)pMxDoc.PageLayout, false, p));
                    else
                        pElem.Add(addTitleToLayout(pMxDoc, 8, 76.09 - (n / 7), 43.7, (IGraphicsContainer)pMxDoc.PageLayout, false, p));
                    break; 
                }
  #endregion
                G.selectLayerByAttribute(seleccion, "municipio = " + n_municipio + " AND n_plano = " + n_plano, true);
                selectArea = B.returnLayerByName(ArcMap.Document, "select_area");
                G.selectClip(seleccion, selectArea, globales.gdb + "seleccionZona");
               /*Inserta el numero del plano*/
                pElem.Add( addTitleToLayout(pMxDoc, 12.5, 70.85, 11.064, (IGraphicsContainer)pMxDoc.PageLayout, true, n_plano + "/" + t_plano));
                seleccionZona = B.returnLayerByName(ArcMap.Document, "seleccionZona");

                G.selectClip(zona, seleccionZona, globales.gdb + "zonasValor");
                B.limpiarZonasTmp(ArcMap.Document, "municipio");
                zonasValor = B.returnLayerByName(ArcMap.Document, "zonasValor");
                // estilado ZonaValor 
                G.applySymbologyFromLayer(zonasValor, zona);
                /*Inserta los valores de las zonas
                 x= │  y= ─
                 */
                if (B.returnFirstFieldData(B.returnFeatureClassByName(pMxDoc, "zonasValor"), "VALOR") == " ")
                {
                    B.limpiarZonasTmp(pMxDoc, "seleccionZona");
                    pElem.Add(addTitleToLayout(pMxDoc, 40, 32, 27, (IGraphicsContainer)pMxDoc.PageLayout, false, "SIN ZONAS DE VALOR"));
                }
                else
                {
                    addMapSurroundM(pMxDoc, "zonasValor", "clave_zona", "VALOR", 76.90,36.93, ref pElem);
                    pMap.ClearSelection();
                }
                G.selectClip(MANZANAS, zonasValor, globales.gdb + "manzana");
                manzana = B.returnLayerByName(ArcMap.Document, "manzana");
               // G.copyFeatures(manzana, globales.gdb + "manzana" + n_municipio);////////////////////////////////////////////////////////////
                G.applySymbologyFromLayer(manzana, MANZANAS);
                pMap.ClearSelection();
                B.limpiarZonasTmp(ArcMap.Document, "seleccionZona");
                ///////////////////Creamos una tabla//////////////////////////
                string temp="";
                foreach (var str in nom_plano)
                    temp += str.ToString();
                
                 addMapSurroundTabla(temp, pMxDoc.PageLayout, ref pElem);
                 ///////////////////////////////////////////////////
#region Macro mapa

                 IMapDocument pMapDoc = new MapDocumentClass();
                IMap pMap2 = pMxDoc.Maps.get_Item(1);

                pMxDoc.ActiveView = (IActiveView)pMap2;
                string mun = "municipio";
                ILayer SELECCION = B.returnLayerByName(ArcMap.Document, mun);
                G.selectLayerByAttribute(SELECCION, "municipio = " + n_municipio, true);

                B.zoomToSeleccion(pMxDoc, mun);
                pMap2.ClearSelection();

                pMxDoc.ActiveView.Refresh();
                pMap = pMxDoc.Maps.get_Item(0);
                pMxDoc.ActiveView = (IActiveView)pMap;
                pMxDoc.ActiveView = (IActiveView)pMxDoc.PageLayout;
#endregion
                return pElem;
            } catch (System.Exception ex) {
                MessageBox.Show("Error: " + ex.Message, "clsMapTools.discrimaDatosLayer");
                MessageBox.Show("Error: " + ex.StackTrace);
                return null;
            }
        }
        //////////////////////////////////////////////
        public void expo(string nom_mun)
        {
            ILayer municipio = B.returnLayerByName(ArcMap.Document, "municipio");
        }
        //////////////////////////////////////////////////////
        private int dimendionesImg(string ruta) {
            var image = Image.FromFile(ruta);
            return image.Height;
        }
        public void addMapSurroundTabla( string nombreMun, IPageLayout pPageLayout, ref List<IElement> pElem)
        {
            IPictureElement pPicElement = new BmpPictureElementClass();
            string tabla = @"C:\IRCEP\temp\tablas\";
            try
            {
                tabla = tabla+ nombreMun + ".png";

               double y2= dimendionesImg(tabla)*.015;
                pPicElement.ImportPictureFromFile(tabla);
                pPicElement.MaintainAspectRatio = false;

                //Set the New Element to the Picture Element
                IElement pElement = (IElement)pPicElement;

                //Create Target Envelope
                IEnvelope pEnv = new EnvelopeClass();
                double y = 13.0;//10 //25.7;//en el mapa
                double x = 34.70; //27.50;//en el mapa
                //void putCoords(double xMin, double yMin, double xMax, double yMax)
                //pEnv.PutCoords(x, (y - 2.5), (x + 5.4), y);//Cambia el tamaño
                pEnv.PutCoords(x, (y-y2), (x + 5.4), y );//Cambia el tamaño
                pElement.Geometry = pEnv;
                
                //set the container as the pagelayout and add the element created
                IGraphicsContainer pGraphicsContainer = (IGraphicsContainer)pPageLayout;
                pGraphicsContainer.AddElement(pElement, 0);

                IActiveView pActiveView = (IActiveView)pPageLayout;
                pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, pElement, pEnv);
                pElem.Add(  pElement);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "clsMapTools.addMapSurroundM \n Linea" + ex.StackTrace);
            }
        }
        public List<IElement> addMapSurroundM(IMxDocument pMxDoc, string nombreLayer, string campoLayer, string campoLayer2, double x, double y, ref  List<IElement> pElem)
        {
            List<objetos> clave_zona = null;
            string clave = null;
            string[] arrayTmp = null;
            int j = 0;
            int i;
            IFeatureClass fc = default(IFeatureClass);
            int n = 0;
            string leyenda = null;
            // = new List<IElement>(); 
            fc = B.returnFeatureLayerByName(ArcMap.Document, nombreLayer).FeatureClass;
            try
            {
 #region valores abreviatura               
                clave_zona = lTools.returMatrizDataUnique(fc, campoLayer, campoLayer2);
                n = clave_zona.Count();
                j = 0;
                for (i = 0; i < n; i++)
                {
                    Array.Resize(ref arrayTmp, i + 1); 
                    arrayTmp[i] = clave_zona[i].clave;
                    switch (clave_zona[i].clave)
                    {
                        case "LOC. FORÁNEA":
                        case "LOC FORÁNEA":
                            leyenda = "LOC. FORÁNEA = LOCALIDAD FORÁNEA\n";
                            break;
                        case "LOC. FORANEA":
                        case "LOC FORANEA":
                            leyenda = "LOC. FORANEA = LOCALIDAD FORÁNEA\n";
                            break;
                        case "SUB.":
                        case "SUB":
                            leyenda = "SUB. = SUBURBANO\n ";
                            break;
                    }
                }
                System.Array.Sort(arrayTmp);
#endregion
#region valores monetario de la zona
                /*
                double tLetra = 5;//tamaño de la letra
                vDiccionario v = new vDiccionario(clave_zona);
                for (i = 0; i < n; i++)
                {
                    clave = v.sayValorZ(arrayTmp[i]);

                    //clave = "0";
                     //clave = funcAux.convierteaMoneda(clave);
                    IElement m;
                    if (clave.Length <= 7)
                        m = addTitleToLayout(pMxDoc, tLetra, x, y, (IGraphicsContainer)pMxDoc.PageLayout, false, clave);
                    else
                        m = addTitleToLayout(pMxDoc, tLetra, x - 0.215, y, (IGraphicsContainer)pMxDoc.PageLayout, false, clave);
                    pElem.Add(m);
                    //x= │  y= ─
                    j = j + 1;
                    //y = y - 1.532; para times rome de 10
                    y = y - 1.430;
                    }
                    */
                #endregion
                if (leyenda != null){
                    leyenda = "CLAVE:\n " + leyenda;
                    IElement m = addTitleToLayout(pMxDoc, 5.0, 70, 10.9, (IGraphicsContainer)pMxDoc.PageLayout, true, leyenda,true);
                    pElem.Add(m);
                }
                return pElem;
            } catch (System.Exception ex) {
                MessageBox.Show("Error: " + ex.Message, "clsMapTools.addMapSurroundM");
                MessageBox.Show("Error: " + ex.StackTrace);
                return null;
            }
        }
        public IElement addTitleToLayout(IMxDocument pMxDoc,double S, double x, double y,  IGraphicsContainer pGc, bool b, string texto,bool rb=false) {
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
                myFont.Name = "Arial";
                myFont.Bold = b;
                myFont.Size = 9;
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
                //pTxtSym.HorizontalAlignmentRightToLeft
                if (rb)
                    pTxtSym.HorizontalAlignment = ESRI.ArcGIS.Display.esriTextHorizontalAlignment.esriTHALeft;
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
