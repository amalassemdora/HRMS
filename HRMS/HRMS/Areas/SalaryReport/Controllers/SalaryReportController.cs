using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using BL.Employee;
using BL.SalaryReport;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace HRMS.Areas.SalaryReport
{
    public class SalaryReportController : Controller
    {
        // GET: SalaryReport/SalaryReport
		public string monthsearch = (Int32.Parse(DateTime.Now.Month.ToString()) - 1).ToString();
        public string yearsearch = DateTime.Now.Year.ToString();

        MyEmployee emp = new MyEmployee();
        Salary_Report report = new Salary_Report();
        public ActionResult Index()
        {
            ViewBag.month = (Int32.Parse(DateTime.Now.Month.ToString()) - 1).ToString();
            ViewBag.year = DateTime.Now.Year.ToString();
            ViewBag.salaryreport = report;
            var data = emp.GetAllEmployee();
            return View(data);
        }
        public ActionResult Search(string Month, string Year)
        {
            ViewBag.month = Month;
            monthsearch = ViewBag.month;
            ViewBag.year = Year;
            yearsearch = ViewBag.year;
            ViewBag.salaryreport = report;
            var data = emp.GetAllEmployee();
            return View(data);
        }
        public FileResult CreatePdf()
        {
            MemoryStream workStream = new MemoryStream();
            StringBuilder status = new StringBuilder("");
            DateTime dTime = DateTime.Now;
            //file name to be created   
            string strPDFFileName = string.Format("SamplePdf" + dTime.ToString("yyyyMMdd") + "-" + ".pdf");
            Document doc = new Document();
            doc.SetMargins(0f, 0f, 0f, 0f);
            //Create PDF Table with 10 columns  
            PdfPTable tableLayout = new PdfPTable(10);
            doc.SetMargins(0f, 0f, 0f, 0f);
            //Create PDF Table  

            //file will created in this path  
            string strAttachment = Server.MapPath("~/Downloads/" + strPDFFileName);
            PdfWriter.GetInstance(doc, workStream).CloseStream = false;
            doc.Open();

            //Add Content to PDF   
            doc.Add(Add_Content_To_PDF(tableLayout));

            // Closing the document  
            doc.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;


            return File(workStream, "application/pdf", strPDFFileName);

        }

        protected PdfPTable Add_Content_To_PDF(PdfPTable tableLayout)
        {

            float[] headers = { 10, 35, 20, 20, 20, 20, 20, 20, 20, 24 }; //Header Widths  
            tableLayout.SetWidths(headers); //Set the pdf headers  
            tableLayout.WidthPercentage = 100; //Set the PDF File witdh percentage  
            tableLayout.HeaderRows = 1;
            //Add Title to the PDF file at the top  

            var employees = emp.GetAllEmployee();

            tableLayout.AddCell(new PdfPCell(new Phrase("\n\nSalary Report of Month:"+monthsearch+" & year:"+ yearsearch+"\n\n", new Font(Font.FontFamily.HELVETICA, 8, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
            {
                Colspan = 12,
                Border = 0,
                PaddingBottom = 5,
                HorizontalAlignment = Element.ALIGN_CENTER
            });


            ////Add header  
            AddCellToHeader(tableLayout, "#");
            AddCellToHeader(tableLayout, "Name");
            AddCellToHeader(tableLayout, "Basic Salary");
            AddCellToHeader(tableLayout, "#Attended");
            AddCellToHeader(tableLayout, "#Absence");
            AddCellToHeader(tableLayout, "#overtime");
            AddCellToHeader(tableLayout, "#Discount");
            AddCellToHeader(tableLayout, "overtime");
            AddCellToHeader(tableLayout, "deductions");
            AddCellToHeader(tableLayout, "Net salary");

            ////Add body  

            foreach (var item in employees)
            {

                AddCellToBody(tableLayout, item.Id.ToString());
                AddCellToBody(tableLayout, item.Name);
                AddCellToBody(tableLayout, item.Salary);
                AddCellToBody(tableLayout, (report.GetAttendedDays(item.Id,monthsearch,yearsearch)).ToString()); 
                AddCellToBody(tableLayout, (report.GetAbsenceDays(item.Id, monthsearch, yearsearch)).ToString());
                AddCellToBody(tableLayout, (report.Getovertime(item.Id, monthsearch, yearsearch)).ToString());
                AddCellToBody(tableLayout, (report.Getdeductiontime(item.Id, monthsearch, yearsearch)).ToString());
                AddCellToBody(tableLayout, (report.GetAmountOfOvertime(item.Id, monthsearch, yearsearch)).ToString());
                AddCellToBody(tableLayout, (report.GetAmountOfDeductiontime(item.Id, monthsearch, yearsearch)).ToString());
                AddCellToBody(tableLayout, (report.GetNetSalary(item.Id, monthsearch, yearsearch)).ToString());
            }

            return tableLayout;
        }
        // Method to add single cell to the Header  
        private static void AddCellToHeader(PdfPTable tableLayout, string cellText)
        {

            tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(Font.FontFamily.HELVETICA, 8, 1, iTextSharp.text.BaseColor.YELLOW)))
            {
                HorizontalAlignment = Element.ALIGN_LEFT,
                Padding = 5,
                BackgroundColor = new iTextSharp.text.BaseColor(128, 0, 0)
            });
        }

        // Method to add single cell to the body  
        private static void AddCellToBody(PdfPTable tableLayout, string cellText)
        {
            tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(Font.FontFamily.HELVETICA, 8, 1, iTextSharp.text.BaseColor.BLACK)))
            {
                HorizontalAlignment = Element.ALIGN_LEFT,
                Padding = 5,
                BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255)
            });
        }
    }
}