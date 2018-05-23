using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;

/// <TODO>
///     For now
///         Grab all the input fields from a XFA pdf and store into a list
///         Display XFA pdf on the browser (Most likly need to flatten it)
///         
///     Later
///         Overlay inputs on the displayed Xfa PDF
///         
/// </TODO>

namespace XmlPdfMapper.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// As of right now, this method grabs a xfa pdf convert it into a XML document and inserts the content of the XML document into a string builder
        /// Run the program to see a large XML string in the bottom of the index page.
        /// </summary>
        /// <returns></returns>

        public ActionResult Index()
        {
            //The XFA based pdfs (First one is the document Allstate uses)
            var path = @"C:\Users\jag27\Documents\Personal\Infosys\AllState\Projects\XmlPdfMapper\XmlPdfMapper\Assets\ABJ45A1AL.pdf";
            //XFA pdf I found on the internet (only here for testing purposes)
            //var path = @"C:\Users\jag27\Documents\Personal\Infosys\AllState\Projects\XmlPdfMapper\XmlPdfMapper\Assets\purchase_order.pdf";
            PdfReader reader = new PdfReader(path);


            // grab xfa field and convert to XML document
            XfaForm xfa = new XfaForm(reader);
            XmlDocument xml = xfa.DomDocument;
            reader.Close();

            if (!string.IsNullOrEmpty(xml.DocumentElement.NamespaceURI))
            {
                xml.DocumentElement.SetAttribute("xmlns", "");
                XmlDocument new_xml = new XmlDocument();
                new_xml.LoadXml(xml.OuterXml);
                xml = new_xml;
            }

            var sb = new StringBuilder(4000);
            var Xsetting = new XmlWriterSettings() { Indent = true };
            using (var writer = XmlWriter.Create(sb, Xsetting))
            {
                xml.WriteTo(writer);
            }
            ViewBag.Form = sb.ToString();
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}