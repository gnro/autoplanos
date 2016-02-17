using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using autoPlanos.Clases;

namespace autoPlanos.Diccionary
{
    class vDiccionario
    {
        private Dictionary<string, string> _dics = new Dictionary<string, string>();
        public vDiccionario(List<objetos> s)
        {
            int n = 0;
            n = s.Count();
            for (int i = 0; i <n; i++)
                _dics.Add(s[i].clave, s[i].valor);
        }
        public string sayValorZ(string clvzonaV){
            string test;
            return (_dics.TryGetValue(clvzonaV, out test))?test:"0";
        }
    }
}
