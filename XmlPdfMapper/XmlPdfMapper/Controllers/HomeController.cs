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
using XmlPdfMapper.Models;
using Newtonsoft.Json;

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
        private MapperContext db = new MapperContext();

        public static string pdfPath;
        public static string xmlPath;


        public ActionResult Index()
        {

            List<Mapper> result = new List<Mapper>();

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
                var pdfNodes = doc.Descendants().Select(x => x.Name.ToString()).ToList();

                reader.Close();

               
                foreach (var node in pdfNodes) {
                    result.Add(new Mapper()
                    {
                        FileName = pdfPath,
                        PdfName = node,

                    });
                }

                ViewBag.Pdf = result;

            }
     

            //---------------------------------------------------------------------------------------------------------------------------------------------------------
            //Loads a XML document(hard coded right now) and split them by new line

            if (String.IsNullOrEmpty(xmlPath)) ViewBag.Presentation = new string[0];
            else
            {

                XDocument xml = XDocument.Load(xmlPath);

                string[] pres = xml.ToString().Split('\n');

                ViewBag.Presentation = pres;

                List<Node> nodeList = new List<Node>();

                foreach (XElement node in xml.Descendants())
                {
                    Node n = new Node()
                    {
                        Name = node.Name.ToString(),
                        Xpath = findPath(node)
                    };
                    nodeList.Add(n);
                }

                Debug.WriteLine(pres.Length + " : " + nodeList.Count);
                ViewBag.Xml = JsonConvert.SerializeObject(nodeList);
            }


            return View(result);
        }

        public ActionResult Compute(List<Mapper> map) {

            if (map == null) return Redirect("Index");
            foreach (var field in map)
            {
                if (!String.IsNullOrEmpty(field.XmlXpath))
                {
                    db.Mappers.Add(field);
                }

            }

            db.SaveChanges();
            xmlPath = null;
            pdfPath = null;

            return Redirect("Index");
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

        [NonAction]
        public string findPath(XElement node) {

            string xPath = node.Name.ToString();

            while (node.Parent != null) {
                xPath = node.Parent.Name + "/" + xPath;
                node = node.Parent;
            }
            return xPath;

        } 
    }
}