using ESRI.ArcGIS.ArcMapUI;
using autoPlanos;
using System.Collections.Generic;
using System;

static class globales
{
    public static readonly string nomlayerSELECT = "select_area";
    public static readonly string nomlayerMUNICIPIOS = "MUNICIPIOS";
    public static string patHome = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
    public static readonly string rutaD = "C:\\IRCEP\\temp\\dictionary\\";
    public static readonly string ruta = "C:\\IRCEP\\temp\\Escudos_Municipios\\";
    public static readonly string planos = @"C:\\IRCEP\\planos";
    //public static readonly string gdb = "C:\\IRCEP\\temp\\n_aux.gdb\\";
    public static readonly string gdb = patHome + "\\ArcGIS\\Default.gdb\\";
    public static readonly List<string> ccE = new List<string> { nomlayerSELECT, "manzana", "zonasValor", "MANZANAS", "ZONA", nomlayerMUNICIPIOS };//   
    
   
}
