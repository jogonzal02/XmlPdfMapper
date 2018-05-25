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
using System.Diagnostics;

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

            if (String.IsNullOrEmpty(pdfPath)) { ViewBag.Pdf = new string[0]; }
            else
            {
                //Reads the PDF
                PdfReader reader = new PdfReader(pdfPath);

                //Grabs the xfa forms
                XfaForm xfa = new XfaForm(reader);

                //Grab the dataset node
                string xmllist = xfa.DatasetsNode.InnerXml;
                XDocument doc = XDocument.Parse(xmllist);

                //grab the name of all the dataset nodes
                var result = doc.Descendants().Select(x => x.Name).ToList();

                reader.Close();

                ViewBag.Pdf = result;

            }
     

            //---------------------------------------------------------------------------------------------------------------------------------------------------------
            //Loads a XML document(hard coded right now) and split them by new line

            if (String.IsNullOrEmpty(xmlPath)) ViewBag.Xml = new string[0];
            else
            {

                XDocument x = XDocument.Load(xmlPath);
                var xmlArr = x.ToString().Split('\n');
                ViewBag.Xml = xmlArr;
            }


            return View();
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