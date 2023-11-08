using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace BetterCallSOAP
{
    public class ResponseDto
    {
        public string resultText { get; set; }

        public XmlDocument resultXML { get; set; }
        public string resultCode { get; set; }

        public System.Drawing.Color  resultColor { get; set; }

        public string resultError { get; set; }


    }
}
