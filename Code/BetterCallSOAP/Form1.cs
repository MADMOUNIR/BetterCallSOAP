using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Services.Description;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace BetterCallSOAP
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// Variables public
        /// </summary>
        private MethodInfo[] methodInfo;
        private ParameterInfo[] param;
        private Type service;
        private Type[] paramTypes;
        private properties myProperty;
        private string MethodName = "";
        private string WSnamespace = "";

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                progressBar1.Value = 10;
                if (txtBody.Text == "")
                {
                    MessageBox.Show("Body must be filled !");
                }
                else
                {
                    ResponseDto result = WSHelper.InvokeService(txtUrl.Text, tbHeader.Text , "user", txtBody.Text );
                    progressBar1.Value = 50;
                    txtResult.Text = (result.resultError != null) ? result.resultError : System.Xml.Linq.XDocument.Parse(result.resultText).ToString();
                    lblResult.Text = result.resultCode;
                    lblResult.ForeColor = result.resultColor;
                    progressBar1.Value = 70;
                }
            }
            catch (Exception ex)
            {
                progressBar1.Value = 70;
                throw ex;
            }
            
        }

        

        public  void DicoverWS(string url)
        {
            try
            {
                //messageTextBox.Text += "Generating WSDL \r\n";
                progressBar1.Visible = true;
                progressBar1.PerformStep();

                Uri uri = new Uri(url);

                // messageTextBox.Text += "Generating WSDL \r\n";
                 progressBar1.PerformStep();

                WebRequest webRequest = WebRequest.Create(uri);
                webRequest.Credentials = CredentialCache.DefaultCredentials;
                System.IO.Stream requestStream = webRequest.GetResponse().GetResponseStream();
                // Get a WSDL file describing a service
                ServiceDescription sd = ServiceDescription.Read(requestStream);
                WSnamespace = sd.TargetNamespace;
                string sdName = sd.Services[0].Name;
                // Add in tree view
                treeWsdl.Nodes.Add(sdName);

               // messageTextBox.Text += "Generating Proxy \r\n";
                progressBar1.PerformStep();

                // Initialize a service description servImport
                ServiceDescriptionImporter servImport = new ServiceDescriptionImporter();
                servImport.AddServiceDescription(sd, String.Empty, String.Empty);
                servImport.ProtocolName = "Soap";
                servImport.CodeGenerationOptions = CodeGenerationOptions.GenerateProperties;

                //messageTextBox.Text += "Generating assembly  \r\n";
                 progressBar1.PerformStep();

                CodeNamespace nameSpace = new CodeNamespace();
                CodeCompileUnit codeCompileUnit = new CodeCompileUnit();
                codeCompileUnit.Namespaces.Add(nameSpace);
                // Set Warnings
                ServiceDescriptionImportWarnings warnings = servImport.Import(nameSpace, codeCompileUnit);

                if (warnings == 0)
                {
                    StringWriter stringWriter = new StringWriter(System.Globalization.CultureInfo.CurrentCulture);
                    Microsoft.CSharp.CSharpCodeProvider prov = new Microsoft.CSharp.CSharpCodeProvider();
                    prov.GenerateCodeFromNamespace(nameSpace, stringWriter, new CodeGeneratorOptions());

                    //messageTextBox.Text += "Compiling assembly \r\n";
                     progressBar1.PerformStep();

                    // Compile the assembly with the appropriate references
                    string[] assemblyReferences = new string[2] { "System.Web.Services.dll", "System.Xml.dll" };
                    CompilerParameters param = new CompilerParameters(assemblyReferences);
                    param.GenerateExecutable = false;
                    param.GenerateInMemory = true;
                    param.TreatWarningsAsErrors = false;
                    param.WarningLevel = 4;

                    CompilerResults results = new CompilerResults(new TempFileCollection());
                    results = prov.CompileAssemblyFromDom(param, codeCompileUnit);
                    Assembly assembly = results.CompiledAssembly;
                    service = assembly.GetType(sdName);


                    //messageTextBox.Text += "Get Methods of Wsdl \r\n";
                     progressBar1.PerformStep();

                    methodInfo = service.GetMethods();
                    foreach (MethodInfo t in methodInfo)
                    {
                        if (t.Name == "Discover")
                            break;


                        treeWsdl.Nodes[0].Nodes.Add(t.Name);
                    }

                    treeWsdl.Nodes[0].Expand();

                    //messageTextBox.Text += "Now ready to invoke \r\n ";
                     progressBar1.PerformStep();
                    //this.tabControl1.SelectedTab = this.tabPage1;

                }
              //  else
                 //   messageTextBox.Text += warnings;
            }
            catch (Exception ex)
            {
               // messageTextBox.Text += "\r\n" + ex.Message + "\r\n\r\n" + ex.ToString(); ;
                progressBar1.Value = 70;
            }
        }

        private void btnDiscover_Click(object sender, EventArgs e)
        {
            treeWsdl.Nodes.Clear();
            if(txtUrl.Text != "")
            {
                DicoverWS(string.Format("{0}?WSDL", txtUrl.Text));
            }
            else
            {
                MessageBox.Show("URL must be filled !" );
            }
            
        }

        private void treeWsdl_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Parent != null)
            {
                List<string> lstParam = new List<string>();
                MethodName = e.Node.Text;
                param = methodInfo[e.Node.Index].GetParameters();
                myProperty = new properties(param.Length);

                // Get the Parameters Type
                paramTypes = new Type[param.Length];
                for (int i = 0; i < paramTypes.Length; i++)
                {
                    paramTypes[i] = param[i].ParameterType;
                }

                foreach (ParameterInfo temp in param)
                {

                    //treeParameters.Nodes[0].Nodes.Add(temp.ParameterType.Name + "  " + temp.Name);
                    lstParam.Add(temp.Name);

                }

                // treeParameters.ExpandAll();

               txtBody.Text =  WSHelper.CreateSOAPBody(MethodName, WSnamespace, lstParam);
            }
        }

        private void btnURL_Click(object sender, EventArgs e)
        {
            ListURL frm = new ListURL();
            frm.ShowDialog(this);
            txtUrl.Text = frm.selectedURL;
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
        private ToolTip tt;
        private void tbHeader_MouseHover(object sender, EventArgs e)
        {
            tt = new ToolTip();
            tt.InitialDelay = 0;
            tt.IsBalloon = true;
            tt.Show(string.Empty, tbHeader);
            tt.Show("header as multilane text, each lane separated by  <:> between  key and value", tbHeader, 0);
        }

        private void tbHeader_MouseLeave(object sender, EventArgs e)
        {
            tt.Hide(tbHeader);
        }
    
    }
}
