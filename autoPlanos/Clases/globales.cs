using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using ESRI.ArcGIS.ArcMapUI;
using autoPlanos;
static class globales
{
    public static readonly string nomlayerSELECT = "select_area";
    public static readonly string nomlayerMUNICIPIOS = "MUNICIPIOS";
    public static readonly string rutaD = "C:\\IRCEP\\temp\\dictionary\\";
    public static readonly string ruta = "C:\\IRCEP\\temp\\Escudos_Municipios\\";
    public static readonly string gdb = "C:\\IRCEP\\temp\\n_aux.gdb\\";
    public static readonly string planos = @"C:\\IRCEP\\planos";
    public static IMxDocument pMxDoc = ArcMap.Document;
   
}
