using System;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Output;
using ESRI.ArcGIS.Display;
using System.Windows.Forms;

namespace autoPlanos.Clases
{
    class clsExport
    {
        [DllImport("user32.dll", SetLastError = true)]
        static extern bool SystemParametersInfo(uint uiAction, uint uiParam, ref int pvParam, uint fWinIni);

        /* constants used for user32 calls */
        const uint SPI_GETFONTSMOOTHING = 74;
        const uint SPI_SETFONTSMOOTHING = 75;
        const uint SPIF_UPDATEINIFILE = 0x1;
        public void exportMapa(string ExportType, long iOutputResolution, long lResampleRatio, Boolean bClipToGraphicsExtent, string nombreArchivo,string sOutputDir){
            IActiveView docActiveView = ArcMap.Document.ActiveView;
            IExport docExport;
            IPrintAndExport docPrintExport;
            IOutputRasterSettings RasterSettings;
            bool bReenable = false;
            if (GetFontSmoothing())
            {
                /* font smoothing is on, disable it and set the flag to reenable it later. */
                bReenable = true;
                DisableFontSmoothing();
                if (GetFontSmoothing())
                    return;
            }
            // The Export*Class() type initializes a new export class of the desired type.
            if (ExportType == "PDF")
            {
                docExport = new ExportPDFClass();
            }
            else if (ExportType == "EPS")
            {
                docExport = new ExportPSClass();
            }
            else if (ExportType == "AI")
            {
                docExport = new ExportAIClass();
            }
            else if (ExportType == "BMP")
            {
                docExport = new ExportBMPClass();
            }
            else if (ExportType == "TIFF")
            {
                docExport = new ExportTIFFClass();
            }
            else if (ExportType == "SVG")
            {
                docExport = new ExportSVGClass();
            }
            else if (ExportType == "PNG")
            {
                docExport = new ExportPNGClass();
            }
            else if (ExportType == "GIF")
            {
                docExport = new ExportGIFClass();
            }
            else if (ExportType == "EMF")
            {
                docExport = new ExportEMFClass();
            }
            else if (ExportType == "JPEG")
            {
                docExport = new ExportJPEGClass();
            }
            else
            {
                MessageBox.Show("Unsupported export type " + ExportType + ", defaulting to EMF.");
                ExportType = "EMF";
                docExport = new ExportEMFClass();
            }

            docPrintExport = new PrintAndExportClass();

            //set the export filename (which is the nameroot + the appropriate file extension)
            docExport.ExportFileName = sOutputDir + nombreArchivo + "." + docExport.Filter.Split('.')[1].Split('|')[0].Split(')')[0];

            if (docExport is IOutputRasterSettings){
                RasterSettings = (IOutputRasterSettings)docExport;
                RasterSettings.ResampleRatio = (int)lResampleRatio;
            }

            docPrintExport.Export(docActiveView, docExport, iOutputResolution, bClipToGraphicsExtent, null);
            if (bReenable)
            {
                /* reenable font smoothing if we disabled it before */
                EnableFontSmoothing();
                bReenable = false;
                if (!GetFontSmoothing())
                    MessageBox.Show("Unable to reenable Font Smoothing", "Font Smoothing error");
            }
        }

        private void DisableFontSmoothing()
        {
            bool iResult;
            int pv = 0;
            /* call to systemparametersinfo to set the font smoothing value */
            iResult = SystemParametersInfo(SPI_SETFONTSMOOTHING, 0, ref pv, SPIF_UPDATEINIFILE);
        }

        private void EnableFontSmoothing()
        {
            bool iResult;
            int pv = 0;
            /* call to systemparametersinfo to set the font smoothing value */
            iResult = SystemParametersInfo(SPI_SETFONTSMOOTHING, 1, ref pv, SPIF_UPDATEINIFILE);
        }

        private Boolean GetFontSmoothing()
        {
            bool iResult;
            int pv = 0;
            /* call to systemparametersinfo to get the font smoothing value */
            iResult = SystemParametersInfo(SPI_GETFONTSMOOTHING, 0, ref pv, 0);
            if (pv > 0)
                return true;
            else
                return false;
        }
    }
}
