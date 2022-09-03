using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace wrMainAntiRansomeware
{
    public partial class ProductKeyForm : Form
    {
        public static Form AForm;

        public static Form thisForm = null;
        public static com.webroam.license1.cloudsecurity cs1;
        public static string SessionOfLic = (10010000 + new Random().Next(1, 9999)).ToString();
        public static string License;
        [ServiceContract]
        public interface ICloudSecurity
        {
            [OperationContract]
            string Step1(string session, string args);

            [OperationContract]
            string Step2(string session, string args);

            [OperationContract]
            string Step3(string session, string args);

            [OperationContract]
            string Step4(string session, string args);

            [OperationContract]
            int doAuthenticate(string session, string license);

            [OperationContract]
            int logout(string session);
        }

        public ProductKeyForm()
        {
            InitializeComponent(); AForm = this;
            try
            { 
            thisForm = this;
            }
            catch (Exception em) { ProductKeyForm.FAppendAllText("wrlog2.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine); }

        }

        public static Dictionary<string, StreamWriter> sw = new Dictionary<string, StreamWriter>();
        public static void FAppendAllText(string stream, string v)
        {/*
            using (StreamWriter sw = new StreamWriter(stream, true))
            {*/
            if (!sw.ContainsKey(stream))
            {
                sw.Add(stream, new StreamWriter(stream));
            }
            sw[stream].Write(v);
            //}
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            { 
            if (MessageBox.Show("Are you sure you want to close the license activation process?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                this.Close();
            }
            catch (Exception em) { ProductKeyForm.FAppendAllText("wrlog2.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine); }

        }

        public static string[] license;
        private void button2_Click(object sender, EventArgs e)
        {
            button2.Enabled = false;
            try
            { 
            if(textBox1.Text.Trim()=="")
            {
                MessageBox.Show("You must fill every field required!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox1.Focus();
                    button2.Enabled = true;
                    return;
            }
                string sdec = Encoding.UTF8.GetString(Form1.Aes256Decrypt(Form1.SHexToByteArray(Form1.StrToHex(textBox1.Text))));
            license = sdec.Split(' ');
              //  MessageBox.Show(String.Join(" ", license));
                if (!(license.Length == 6 && license[1] == "1001" && license[4] == "1"))
                {
                    MessageBox.Show("Invalid Product key!");
                    button2.Enabled = true;
                    return;
                }
                License = textBox1.Text;

                EndpointAddress basicAuthEndpoint = new EndpointAddress("https://license1.webroam.com/l_service/check/simple_server.php?wsdl");
                
                var transportElement = new HttpsTransportBindingElement();
                transportElement.AuthenticationScheme = AuthenticationSchemes.Basic;
                
                var messegeElement = new TextMessageEncodingBindingElement();
                messegeElement.MessageVersion = MessageVersion.CreateVersion(EnvelopeVersion.Soap11);


                var binding = new CustomBinding(messegeElement, transportElement);
                //binding.Security.Mode = BasicHttpsSecurityMode.Transport;
                //CustomBinding binding = new CustomBinding();
                binding.Name = "TlsBinding";

                HttpsTransportBindingElement httpsTransport = new HttpsTransportBindingElement();
                httpsTransport.AuthenticationScheme = AuthenticationSchemes.Basic;
                binding.Elements.Clear();
                binding.Elements.Add(new TextMessageEncodingBindingElement()
                {
                    MessageVersion = System.ServiceModel.Channels.MessageVersion.Soap11,
                    WriteEncoding = new UTF8Encoding(false)
                });
                binding.Elements.Add(httpsTransport);
                //cs1 = new com.webroam.license1.cloudsecurity();
                //var uri = new Uri("https://license1.webroam.com/l_service/check/simple_server.php");
                //ServiceHost myServiceHost = new ServiceHost(typeof(com.webroam.license1.cloudsecurity), uri);

                cs1 = new com.webroam.license1.cloudsecurity();
                cs1.UseDefaultCredentials = false;
                cs1.PreAuthenticate = false;
                cs1.Credentials = new NetworkCredential("myuser","webroam_123");

                string lic = textBox1.Text.Trim().Replace("'", "\\'");
                int i = cs1.doAuthenticate(ProductKeyForm.SessionOfLic, lic);
                
                /*ChannelFactory<ICloudSecurity> factory = new ChannelFactory<ICloudSecurity>(binding, basicAuthEndpoint);
                factory.Credentials.UserName.UserName = "myuser";
                factory.Credentials.UserName.Password = "webroam_123";
                //cs1 = factory.CreateChannel();
                var proxy = factory.CreateChannel();*/

             //   cs1 = proxy;
                
               // int i = cs1.doAuthenticate(ProductKeyForm.SessionOfLic, lic);
               //MessageBox.Show(i.ToString());
                if (i==-1)
            {
                MessageBox.Show("Authentication Error!","", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    button2.Enabled = true;
                    return;
            }
              
                string rs = cs1.Step1(ProductKeyForm.SessionOfLic, lic);
                
                
             //   MessageBox.Show(lic+Environment.NewLine+rs);
            if (rs != "OK")
                {
                    MessageBox.Show("Invalid License!");
                    button2.Enabled = true;
                    return;
                }
            //SAPwebservice.YS_TEST_SERVICE sapClient = new SAPwebservice.YS_TEST_SERVICE(basicAuthBinding, basicAuthEndpoint);

            this.Visible = false;
            if (ActivateForm.thisForm == null)
                new ActivateForm().Show();
            else
                ActivateForm.thisForm.Show();
                //this.Close();
            }
            catch (Exception em) {
                ProductKeyForm.FAppendAllText("wrlog2.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);
                MessageBox.Show("Could not proceed to next step! Some error occured.","Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            button2.Enabled = true;

        }

        private void ProductKeyForm_Load(object sender, EventArgs e)
        {

        }
    }
}
