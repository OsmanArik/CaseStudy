using Shared.Core.SOAP.Models;
using System.Net;
using System.Text;
using System.Xml.Linq;

namespace Shared.Core.SOAP.Concrete
{
    public class SOAPClient : IDisposable
    {
        #region Variable

        protected static XNamespace NsEnv = "http://schemas.xmlsoap.org/soap/envelope/";

        #endregion

        #region Constructor

        public SOAPClient() { }

        #endregion

        #region Methods

        #region Public Methods

        public virtual string RequestSchemaCreate(string request)
            => $@"
                  <soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" 
                                    xmlns:tem=""http://tempuri.org/""
                                    xmlns:flig=""http://schemas.datacontract.org/2004/07/FlightProvider"">
                     <soapenv:Header/>
                     <soapenv:Body>
                        <tem:AvailabilitySearch>
                           <tem:request>
                              {request}
                           </tem:request>
                        </tem:AvailabilitySearch>
                     </soapenv:Body>
                  </soapenv:Envelope>";

        public virtual async Task<string> SendRequestAsync(RequestParameterModel model)
        {
            string strResponse = "";

            try
            {
                var request = (HttpWebRequest)WebRequest.Create(model.Url);

                request.Method = "POST";
                request.ContentType = "text/xml; charset=UTF-8";
                request.ContentLength = model.Request.Length;
                request.Headers.Add("SOAPAction", model.SoapActionName);
                request.Headers.Add("Accept-Encoding", "gzip, deflate");
                request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                request.ServicePoint.Expect100Continue = true;

                using (var streamWriter = new StreamWriter(await request.GetRequestStreamAsync(), Encoding.ASCII))
                {
                    await streamWriter.WriteAsync(model.Request);
                }

                using (var response = (HttpWebResponse)await request.GetResponseAsync())
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    strResponse =await reader.ReadToEndAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return strResponse;
        }

        public virtual string ReadResponse(string response)
        {
            var xElement = XElement.Parse(response);
            var xeFault = xElement.Element(NsEnv + "Body")?.Element(NsEnv + "Fault");

            return xeFault != null ? xeFault.ToString() : xElement.Element(NsEnv + "Body")?.Elements().First()?.Elements().First().ToString();
        }

        public void Dispose()
        {
           
        }

        #endregion

        #endregion

    }
}
