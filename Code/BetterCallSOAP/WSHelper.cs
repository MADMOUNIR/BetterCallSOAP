using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Xml;
using System.IO;
using System.Windows.Forms;
using System.Web.Services.Description;
using System.Xml.Linq;
using System.Text.RegularExpressions;

namespace BetterCallSOAP
{
    public static class WSHelper
    {
        /// <summary>
        /// Init Web request with URL and Headers
        /// </summary>
        /// <param name="url"></param>
        /// <param name="userId"></param>
        /// <param name="headers"></param>
        /// <returns></returns>
        public static  HttpWebRequest CreateSOAPWebRequest(string url , string userId , Dictionary<string,string> headers)
        {
            HttpWebRequest Req = null;
            try
            {
                //Making Web Request    
                Req = (HttpWebRequest)WebRequest.Create(url);
                //SOAPAction    
                //Content_type    
                Req.ContentType = "text/xml;charset=\"utf-8\"";
                Req.Accept = "text/xml";
                //HTTP method    
                Req.Method = "POST";
                Req.Credentials = CredentialCache.DefaultCredentials;

                //Add Headers from dict
                foreach (string key in headers.Keys)
                {
                    if(!key.Equals("Content-Type") && !key.Equals("Accept"))
                        Req.Headers.Add(key, headers[key]);
                }
              
                //return HttpWebRequest    
                return Req;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return Req;
                //throw;
            }
           
        }
        /// <summary>
        /// Invoker WS
        /// </summary>
        /// <param name="url"></param>
        /// <param name="userId"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        public static ResponseDto InvokeService(string url, string header ,  string userId ,  string body)
        {
            string resultTxt = "";
            ResponseDto result = new ResponseDto();

            try
            {
                //Get headers from text
                Dictionary<string, string> headers = GetHeadersByText(header);
                
                //Calling CreateSOAPWebRequest method    
                HttpWebRequest request = CreateSOAPWebRequest(url, userId , headers);

                XmlDocument SOAPReqBody = new XmlDocument();
                //SOAP Body Request    
                SOAPReqBody.LoadXml(body);
                using (Stream stream = request.GetRequestStream())
                {
                    SOAPReqBody.Save(stream);
                }
                //Geting response from request    

                WebResponse Serviceres = request.GetResponse();
                //Success of call 
                HttpWebResponse webresponse = Serviceres as HttpWebResponse;
                result.resultCode = webresponse.StatusCode.ToString();
                using (StreamReader rd = new StreamReader(Serviceres.GetResponseStream()))
                {
                    //reading stream    
                    var ServiceResult = rd.ReadToEnd();
                    //writting stream result on console    
                    // Console.WriteLine(ServiceResult);
                    resultTxt += ServiceResult.ToString() + "\n";
                    //Console.ReadLine();
                }
                XmlDocument resXML = new XmlDocument();
                resXML.LoadXml(resultTxt);
                result.resultXML = resXML;
                result.resultText = resultTxt;
                result.resultColor = (result.resultCode == "OK") ? System.Drawing.Color.Green : System.Drawing.Color.Red;

            }
            catch (WebException e)
            {
                result.resultCode = ((HttpWebResponse)e.Response).StatusCode.ToString();
                result.resultColor = System.Drawing.Color.Red;
                result.resultError = e.Message;
            }
            catch (Exception ex)
            {
                //HttpWebResponse resError = ex.Response
                result.resultCode = "Internal Error";
                result.resultColor = System.Drawing.Color.Red;
                result.resultError = ex.Message;
            }
            
            
            return result;
        }
        /// <summary>
        /// retourner un dictionnaire des header à partir d'un text multilignes séparé par <:> entre le key et value
        /// </summary>
        /// <param name="txtHeader"></param>
        /// <returns></returns>
        private static Dictionary<string, string> GetHeadersByText(string  txtHeader)
        {
            //TODO
            Dictionary<string, string> header = new Dictionary<string, string>();

            if (txtHeader.Length > 0)
            {
                string IdOrder = Convert.ToString(txtHeader.Trim());

                //replacing "enter" i.e. "\n" by ","
                string temp = IdOrder.Replace("\r\n", "¤");

                string[] ArrIdOrders = Regex.Split(temp, "¤");

                for (int i = 0; i < ArrIdOrders.Length; i++)
                {
                    //split line into key value
                    string[] keyvalue = Regex.Split(ArrIdOrders[i], ":");
                    //if key and value are here 
                    if (keyvalue.Length == 2)
                    header.Add(keyvalue[0], keyvalue[1]);
                }
                
            }
            return header;

        }

        /// <summary>
        /// Create SOAP Body
        /// </summary>
        /// <param name="method">methode name</param>
        /// <param name="ns">namespace</param>
        /// <param name="parameters">list of params</param>
        /// <returns></returns>
        public static string  CreateSOAPBody(string method ,string ns , List<string> parameters)
        {
            string output = "";
            string soapPrefix = "soap";
            string WSPrefix = "wsc";
            XNamespace soapNamespace = "http://www.w3.org/2003/05/soap-envelope";
            XNamespace WsNamespace = ns;

            XmlWriterSettings writerSettings = new XmlWriterSettings();
            writerSettings.OmitXmlDeclaration = false;
            writerSettings.ConformanceLevel = ConformanceLevel.Fragment;
            writerSettings.CloseOutput = false;
            MemoryStream localMemoryStream = new MemoryStream();

            //l'elément XML du param
            XElement param = new XElement(WsNamespace + method);
            foreach (string p in parameters)
            {
                param.Add(new XElement(WsNamespace + p, "?"));
            }
           
            XElement root = new XElement(soapNamespace + "Envelope",
                new XAttribute(XNamespace.Xmlns + soapPrefix, soapNamespace.NamespaceName),
                new XAttribute(XNamespace.Xmlns + WSPrefix, WsNamespace.NamespaceName),
                new XElement(soapNamespace + "Header"),
                new XElement(soapNamespace + "Body",
                   param
                )
            );


            output = root.ToString();

            return output;


        }

        
    }
}
