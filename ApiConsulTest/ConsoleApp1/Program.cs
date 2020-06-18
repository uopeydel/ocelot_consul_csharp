using FastReport;
using FastReport.Data;
using FastReport.Export.PdfSimple;
using FastReport.Table;
using FastReport.Utils;
using FastReport.Web;
using System;
using System.Data;
using System.Drawing;
using System.IO;

namespace ConsoleApp1
{
    class Program
    {
        private static void Test()
        {
            RegisteredObjects.AddConnection(typeof(MsSqlDataConnection));
            WebReport webReport = new WebReport();

            MsSqlDataConnection sqlConnection = new MsSqlDataConnection();
            sqlConnection.ConnectionString = @"Data Source=DESKTOP-HDMVNBB\MSSQLSERVER2019;AttachDbFilename=;Initial Catalog=FRTEST;Integrated Security=False;Persist Security Info=True;User ID=sa;Password=123456";
            sqlConnection.CreateAllTables();
            webReport.Report.Dictionary.Connections.Add(sqlConnection);

            var pathDir = @"D:\FS_Template\";
            var fileDir = @"Untitled.frx";
            var pathCombine = Path.Combine(pathDir, fileDir);
            webReport.Report.Load(pathCombine);
            webReport.Report.SetParameterValue("Header", " _ Header Of Report");
            webReport.Report.SetParameterValue("prm_name", "first_2");
            var imageUrl = @"https://waymagazine.org/wp-content/uploads/2018/08/vdo-img-bg.jpg";
            webReport.Report.SetParameterValue("imageUrl", imageUrl);



            PictureObject pic = webReport.Report.FindObject("Picture2XXX") as PictureObject;
            //Set object bounds
            pic.Bounds = new RectangleF(Units.Centimeters * 9, 0, Units.Centimeters * 5, Units.Centimeters * 5);
            //Set the image
            System.Net.WebRequest request =
            System.Net.WebRequest.Create(imageUrl);
            System.Net.WebResponse response = request.GetResponse();
            Stream responseStream =
                response.GetResponseStream();
            Bitmap bitmap2 = new Bitmap(responseStream);

            pic.Image = bitmap2;
            //Build report
            // create MSChart Object
            // MSChartObject chart = new MSChartObject();
            // chart.Parent = page.ReportTitle;
            // chart.Bounds = new RectangleF(0, 45, Units.Centimeters * 19, Units.Centimeters * 10);
            // MSChartSeries barSeries = chart.AddSeries(SeriesChartType.Pie);
            // chart.DataSource = report1.GetDataSource("Items_Orders");
            // barSeries.XValue = "[Items_Orders.ShipCountry]";
            // barSeries.YValue1 = "[Items_Orders.Year]";
            // barSeries.GroupBy = GroupBy.XValue;
            // WebReport1.Report = report1;

            webReport.Report.Prepare();

            PDFSimpleExport pdfExport = new PDFSimpleExport();

            var fileDirDest = @"Untitled.pdf";
            var pathCombineDest = Path.Combine(pathDir, fileDirDest);
            pdfExport.Export(webReport.Report, pathCombineDest);
        }



        private static void ManyPage()
        {
            RegisteredObjects.AddConnection(typeof(MsSqlDataConnection));
            WebReport webReport = new WebReport();

            MsSqlDataConnection sqlConnection = new MsSqlDataConnection();
            sqlConnection.ConnectionString = @"Data Source=DESKTOP-HDMVNBB\MSSQLSERVER2019;AttachDbFilename=;Initial Catalog=FRTEST;Integrated Security=False;Persist Security Info=True;User ID=sa;Password=123456";
            sqlConnection.CreateAllTables();
            webReport.Report.Dictionary.Connections.Add(sqlConnection);

            var pathDir = @"D:\FS_Template\";
            var fileDir = @"ManyPage.frx";
            var pathCombine = Path.Combine(pathDir, fileDir);
            webReport.Report.Load(pathCombine);


            // get the data source by its name
            //TableObject Table1 = webReport.Report.FindObject("Table1xxx") as TableObject; 
            //DataSourceBase rowData = webReport.Report.GetDataSource("TablePage");



            webReport.Report.Prepare();

            PDFSimpleExport pdfExport = new PDFSimpleExport();

            var fileDirDest = @"ManyPage.pdf";
            var pathCombineDest = Path.Combine(pathDir, fileDirDest);
            pdfExport.Export(webReport.Report, pathCombineDest);
        }


        static void Main(string[] args)
        {
            try
            {
               var receied =  Encrypt.EncryptConvertJavaToCSharp(Encrypt.privateKey , Encrypt.tokenKey);
                Console.WriteLine(receied);

                var receiedDecrypt = Encrypt.Decrypt( receied , Encrypt.privateKey);

                //return;
                //ManyPage();
                //Test();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            Console.ReadKey();
        }

    }
}
