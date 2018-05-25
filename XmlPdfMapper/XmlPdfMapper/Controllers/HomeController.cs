using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;

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

        public static string pdfPath;
        public static string xmlPath;


        public ActionResult Index()
        {
            //This is jsut a hard coded in path to a XFA based pdf (this should be change to your specific path)
            var src = @"C:\Users\jag27\Documents\Personal\Infosys\AllState\Projects\XmlPdfMapper2\XmlPdfMapper\XmlPdfMapper\Assets\ABJ45A1AL.pdf";

            //Reads the PDF
            PdfReader reader = new PdfReader(src);
            //Grabs the forms
            XfaForm xfa = new XfaForm(reader);

            //Grab the dataset noe
            XmlNode node = xfa.DatasetsNode;
            reader.Close();

            //Write the dataset node into a string builder and store it into a ViewBag
            var sb = new StringBuilder(4000);
            var Xsettings = new XmlWriterSettings() { Indent = true };
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(node.OuterXml);
            using (var writer = XmlWriter.Create(sb, Xsettings))
            {
                doc.WriteTo(writer);
            }
            ViewBag.Form = sb.ToString();


            //---------------------------------------------------------------------------------------------------------------------------------------------------------
            //Loads a XML document(hard coded right now) and split them by new line
            //This is jsut a hard coded in path to a XFA based pdf (this should be change to your specific path)

            if (String.IsNullOrEmpty(xmlPath)) ViewBag.Xml = new string[0] ;
            else
            {
                XDocument x = XDocument.Load(xmlPath);

                var xmlArr = x.ToString().Split('\n');

                ViewBag.Xml = xmlArr;
            }


            return View();
        }

        //Opens and read a pdf file(hard coded right now)
        public FileStreamResult GetPdf() {

            if (String.IsNullOrEmpty(pdfPath)) return null;
            FileStream fs = new FileStream(pdfPath, FileMode.Open, FileAccess.Read);
            return File(fs, "application/pdf");
        }


        [HttpPost]
        public ActionResult UploadPdf(HttpPostedFileBase file) {
            try
            {
                if (file.ContentLength > 0)
                {
                    string _FileName = Path.GetFileName(file.FileName);
                    string _path = Path.Combine(Server.MapPath("~/UploadedFiles"), _FileName);
                    file.SaveAs(_path);
                    pdfPath = _path;   
                }

                return Redirect("Index");
            }
            catch {
                return Redirect("Index");

            }
        }


        [HttpPost]
        public ActionResult UploadXml(HttpPostedFileBase xmlfile) {

            try
            {
                if (xmlfile.ContentLength > 0)
                {
                    string _FileName = Path.GetFileName(xmlfile.FileName);
                    string _path = Path.Combine(Server.MapPath("~/UploadedFiles"), _FileName);
                    xmlfile.SaveAs(_path);
                    xmlPath = _path;
                }

                return Redirect("Index");
            }
            catch
            {
                return Redirect("Index");
            }
        }
    }
}