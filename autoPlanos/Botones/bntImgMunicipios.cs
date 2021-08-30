using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace autoPlanos.Botones
{
    public class bntImgMunicipios : ESRI.ArcGIS.Desktop.AddIns.Button
    {
        public bntImgMunicipios()
        {
        }

        protected override void OnClick()
        {
            Forms.frmGridEscudos bDicc = new Forms.frmGridEscudos();
            bDicc.ShowDialog();
        }

        protected override void OnUpdate()
        {
        }
    }
}
