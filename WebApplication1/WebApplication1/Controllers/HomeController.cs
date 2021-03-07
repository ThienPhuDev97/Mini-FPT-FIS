using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.IO;
using WebApplication1.Service;
using System.Dynamic;
using System.IO.Compression;
using SharpCompress.Readers;
using System.Drawing;


namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private string connectionString = "Data Source=DESKTOP-GCDRL3V;Initial Catalog=Demo1FIS;Integrated Security=True";
        Handle service = new Handle();
        HandleManageQuestion handleManage = new HandleManageQuestion();

        public ActionResult Index()
        {

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }


        public ActionResult ImportExcel()
        {
            dynamic data = new ExpandoObject();
            data.list_dm = service.getALLDM(StoreProcedure.showAllDM);
            data.list_dv = service.getALLDV(StoreProcedure.showAllDV);
            data.list_mt = service.getALLMT(StoreProcedure.showAllMT);
            return View(data);

        }

        //public bool readXLS(string FilePath)
        //    {
        //        FileInfo existingFile = new FileInfo(FilePath);
        //        using (ExcelPackage package = new ExcelPackage(existingFile))
        //        {
        //            //get the first worksheet in the workbook
        //            ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
        //            int colCount = worksheet.Dimension.End.Column;  //get Column Count
        //            int rowCount = worksheet.Dimension.End.Row;     //get row count
        //            string queryString = "INSERT INTO tableName VALUES";        //Here I am using "blind insert". You can specify the column names Blient inset is strongly not recommanded
        //            string eachVal = "";
        //            bool status;
        //            for (int row = 1; row <= rowCount; row++)
        //            {
        //                queryString += "(";
        //                for (int col = 1; col <= colCount; col++)
        //                {
        //                    eachVal = worksheet.Cells[row, col].Value.ToString().Trim();
        //                    queryString += "'" + eachVal + "',";
        //                }


        //            }
        //            queryString = queryString.Remove(queryString.Length - 1, 1);    //removing last comma (,) from the string
        //            status = this.runQuery(queryString);    //executing query
        //            return status;
        //        }
        //    }

        [HttpPost]
        public ActionResult Receive(HttpPostedFileBase file)
        {
            var usersList = new List<QuestionAnswer>();
            if (file == null || file.ContentLength == 0)
            {

                return Content("Please select a excel file");
            }
            else
            {
                if (file.FileName.EndsWith("xls") || file.FileName.EndsWith("xlsx"))
                {
                    try
                    {
                        string fileName2 = Path.GetFileName(file.FileName);
                        string path = Path.Combine(Server.MapPath("~/ImportExcel"), fileName2);
                        file.SaveAs(path);

                        string fileName = file.FileName;
                        string fileContentType = file.ContentType;
                        byte[] fileBytes = new byte[file.ContentLength];
                        var data = file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));

                        using (var package = new ExcelPackage(file.InputStream))
                        {
                            var currentSheet = package.Workbook.Worksheets;
                            var workSheet = currentSheet.First();
                            var noOfCol = workSheet.Dimension.End.Column;
                            var noOfRow = workSheet.Dimension.End.Row;
                            for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                            {
                                var codeQuestion = "QT-" + DateTime.Now.ToString("MM-dd-yyyy-HH-mm-ss");
                                int count = 1;
                                var user = new QuestionAnswer();
                                user.question.ID_Cauhoi = codeQuestion;
                                if (workSheet.Cells[rowIterator, count].Value != null)
                                {
                                    user.question.Content_QS = workSheet.Cells[rowIterator, count].Value.ToString();
                                }
                                if (workSheet.Cells[rowIterator, ++count].Value != null)
                                {
                                    user.question.ID_Type = Int32.Parse(workSheet.Cells[rowIterator, count].Value?.ToString());
                                }
                                if (workSheet.Cells[rowIterator, ++count].Value != null)
                                {
                                    user.question.Id_Level = Int32.Parse(workSheet.Cells[rowIterator, count].Value?.ToString());
                                }
                                
                                user.question.UrlFile = workSheet.Cells[rowIterator, ++count].Value != null ?  workSheet.Cells[rowIterator, count].Value?.ToString() : null;

                                for (int i = 0; i < 4; i++)
                                {
                                    if (workSheet.Cells[rowIterator, ++count].Value != null)
                                    {
                                        TraLoi tl = new TraLoi();
                                        tl.Content_AS = workSheet.Cells[rowIterator, count].Value.ToString();

                                        tl.Explain = workSheet.Cells[rowIterator, ++count].Value != null ? workSheet.Cells[rowIterator, count].Value.ToString() : "";
                                        
                                        tl.ID_TypesAs = workSheet.Cells[rowIterator, ++count].Value != null ? Int32.Parse(workSheet.Cells[rowIterator, count].Value.ToString()) : 0;
                                        tl.ID_Cauhoi = codeQuestion;
                                        user._answers.Add(tl);

                                    }
                                    else
                                    {
                                        count += 2;
                                    }
                                   
                                }

                                usersList.Add(user);
                            }
                        }

                        return Content("Success");
                    }
                    catch (Exception e)
                    {

                        throw;
                    }

                }
                else
                {
                    return Content("File type is incorrect");
                }
            }        
        }

        [HttpPost]
        public ActionResult ImportFile(HttpPostedFileBase file,int id_dm,int id_dv,int id_mt,DateTime? expiration_date)
        {
            // lưu file zip vào thư mục
            string pathToFiles = Server.MapPath("/ImportExcel");
            string fileName2 = Path.GetFileName(file.FileName);
            string path = Path.Combine(Server.MapPath("~/ImportExcel"), fileName2);
            file.SaveAs(path);

            if (!fileName2.EndsWith(".zip")){ return Content("File chọn không phải là file zip"); }
            var data = new List<QuestionAnswer>();
            using (ZipArchive archive = ZipFile.OpenRead(path))
            {
                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    if (entry.FullName.EndsWith(".xlsx", StringComparison.OrdinalIgnoreCase))
                    {
                        // Gets the full path to ensure that relative segments are removed.
                        string destinationPath = Path.GetFullPath(Path.Combine(pathToFiles, entry.FullName));
                        if (System.IO.File.Exists(destinationPath))
                        {
                            System.IO.File.Delete(destinationPath);
                        }
                        

                            // Ordinal match is safest, case-sensitive volumes can be mounted within volumes that
                            // are case-insensitive.
                            if (destinationPath.StartsWith(pathToFiles, StringComparison.Ordinal))
                            entry.ExtractToFile(destinationPath);


                        FileInfo newFile = new FileInfo(destinationPath);
                        using (var package = new ExcelPackage(newFile))
                        {
                            var currentSheet = package.Workbook.Worksheets;
                            var workSheet = currentSheet.First();
                            var noOfCol = workSheet.Dimension.End.Column;
                            var noOfRow = workSheet.Dimension.End.Row;
                            for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                            {
                                var codeQuestion = "QT-" + DateTime.Now.ToString("MM-dd-yyyy-HH-mm-ss"+rowIterator);
                                int count = 1;
                                var user = new QuestionAnswer();
                                user.question.ID_Cauhoi = codeQuestion;
                                user.question.ID_DM = id_dm;
                                user.question.ID_DV = id_dv;
                                user.question.ID_MT = id_mt;
                                user.question.EndDate = expiration_date.HasValue == true ? expiration_date : null;
                                if (workSheet.Cells[rowIterator, count].Value != null)
                                {
                                    user.question.Content_QS = workSheet.Cells[rowIterator, count].Value.ToString();
                                }
                                if (workSheet.Cells[rowIterator, ++count].Value != null)
                                {
                                    user.question.ID_Type = Int32.Parse(workSheet.Cells[rowIterator, count].Value?.ToString());
                                }
                                if (workSheet.Cells[rowIterator, ++count].Value != null)
                                {
                                    user.question.Id_Level = Int32.Parse(workSheet.Cells[rowIterator, count].Value?.ToString());
                                }
                                if (workSheet.Cells[rowIterator, ++count].Value != null)
                                {
                                    user.question.TypeQuestion = Int32.Parse(workSheet.Cells[rowIterator, count].Value?.ToString());
                                }

                                user.question.UrlFile = workSheet.Cells[rowIterator, ++count].Value != null ? workSheet.Cells[rowIterator, count].Value?.ToString() : null;

                                for (int i = 0; i < 4; i++)
                                {
                                    if (workSheet.Cells[rowIterator, ++count].Value != null)
                                    {
                                        TraLoi tl = new TraLoi();
                                        tl.Content_AS = workSheet.Cells[rowIterator, count].Value.ToString();

                                        tl.Explain = workSheet.Cells[rowIterator, ++count].Value != null ? workSheet.Cells[rowIterator, count].Value.ToString() : "";

                                        tl.ID_TypesAs = workSheet.Cells[rowIterator, ++count].Value != null ? Int32.Parse(workSheet.Cells[rowIterator, count].Value.ToString()) : 0;
                                        tl.ID_Cauhoi = codeQuestion;
                                        user._answers.Add(tl);

                                    }
                                    else
                                    {
                                        count += 2;
                                    }

                                }

                                data.Add(user);
                            }
                        }
                    }
                }
            }

            //save data to db

            foreach (var record in data)
            {
                service.insert_question(record.question, StoreProcedure.Insert_Question);
                foreach (TraLoi tl in record._answers)
                {
                    
                    service.Insert_Answer2(tl, StoreProcedure.Insert_Answer);
                }
            }

            return Content("success");
        }

        // tải mẫu file import
        public ActionResult DownLoadTemplate()
        {
            Handle db = new Handle();
            //
            List<QuestionAnswer> data = new List<QuestionAnswer>();
            var list_answer = db.showAllAs(StoreProcedure.getAllAs);
            var list_qs = db.getALLQuestion(StoreProcedure.showQuestion);

            foreach (var qs in list_qs)
            {
                QuestionAnswer record = new QuestionAnswer();
                record.question = qs;
                foreach (TraLoi ans in list_answer)
                {
                    if (ans.ID_Cauhoi.Equals(qs.ID_Cauhoi))
                    {
                        record._answers.Add(ans);
                    }

                }
                data.Add(record);

            }
            int x = 10;
            //

            var list_ans = db.showAllAs(StoreProcedure.getAllAs);
            ExcelPackage excel = new ExcelPackage();
            var workSheet = excel.Workbook.Worksheets.Add("Sheet1");
            workSheet.TabColor = System.Drawing.Color.Black;
            workSheet.DefaultRowHeight = 12;
            // Set default width cho tất cả column
            workSheet.DefaultColWidth = 25;
            // Tự động xuống hàng khi text quá dài
            workSheet.Cells.Style.WrapText = true;
            //Header of table  
            //  
            workSheet.Row(1).Height = 20;

            workSheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Row(1).Style.Font.Bold = true;
            workSheet.Cells[1, 1].Value = "Nội dung câu hỏi";
            workSheet.Cells[1, 2].Value = "Loại câu hỏi";
            workSheet.Cells[1, 3].Value = "Mức Độ";
            workSheet.Cells[1, 4].Value = "Link Video";

            workSheet.Cells[1, 5].Value = "Nội dung câu trả lời(1)";
            workSheet.Cells[1, 6].Value = "Giải thích(1)";
            workSheet.Cells[1, 7].Value = "Loại đáp án(1)";

            workSheet.Cells[1, 8].Value = "Nội dung câu trả lời(2)";
            workSheet.Cells[1, 9].Value = "Giải thích(2)";
            workSheet.Cells[1, 10].Value = "Loại đáp án(2)";

            workSheet.Cells[1, 11].Value = "Nội dung câu trả lời(3)";
            workSheet.Cells[1, 12].Value = "Giải thích(3)";
            workSheet.Cells[1, 13].Value = "Loại đáp án(3)";

            workSheet.Cells[1, 14].Value = "Nội dung câu trả lời(4)";
            workSheet.Cells[1, 15].Value = "Giải thích(4)";
            workSheet.Cells[1, 16].Value = "Loại đáp án(4)";
           
            //Body of table  
            //  
           
            workSheet.Column(1).AutoFit();
            workSheet.Column(2).AutoFit();
            workSheet.Column(3).AutoFit();
            workSheet.Column(4).AutoFit();
            workSheet.Column(5).AutoFit();
            workSheet.Column(6).AutoFit();
            workSheet.Column(7).Width = 12;
            workSheet.Column(8).AutoFit();
            workSheet.Column(9).AutoFit();
            workSheet.Column(10).Width = 12;
            workSheet.Column(11).AutoFit();
            workSheet.Column(12).AutoFit();
            workSheet.Column(13).Width = 12;
            workSheet.Column(14).AutoFit();
            workSheet.Column(15).AutoFit();
            workSheet.Column(16).Width = 12;

            string excelName = "Template";
            using (var memoryStream = new MemoryStream())
            {
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment; filename=" + excelName + ".xlsx");
                excel.SaveAs(memoryStream);
                memoryStream.WriteTo(Response.OutputStream);
                Response.Flush();
                Response.End();
            }
            return View();
        }


      
        private Stream CreateExcelFile(List<QuestionAnswer> data)
        {
            Stream stream = null;
            var list = data; // data
            using (var excelPackage = new ExcelPackage(stream ?? new MemoryStream()))
            {
                // Tạo author cho file Excel
                excelPackage.Workbook.Properties.Author = "Hanker";
                // Tạo title cho file Excel
                excelPackage.Workbook.Properties.Title = "EPP test background";
                // thêm tí comments vào làm màu 
                excelPackage.Workbook.Properties.Comments = "This is my fucking generated Comments";
                // Add Sheet vào file Excel
                excelPackage.Workbook.Worksheets.Add("First Sheet");
                // Lấy Sheet bạn vừa mới tạo ra để thao tác 
                var workSheet = excelPackage.Workbook.Worksheets[1];
                // Đỗ data vào Excel file
                //workSheet.Cells[1, 1].LoadFromCollection(list, true, TableStyles.Dark9);
                BindingFormatForExcel(workSheet, data);
                excelPackage.Save();
                return excelPackage.Stream;
            }
        }

        private void BindingFormatForExcel(ExcelWorksheet worksheet, List<QuestionAnswer> data)
        {
            // Set default width cho tất cả column
            worksheet.DefaultColWidth = 20;
            // Tự động xuống hàng khi text quá dài
            worksheet.Cells.Style.WrapText = true;

            worksheet.Cells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;


            worksheet.Cells[1, 1].Value = "STT";
            worksheet.Cells[1, 2].Value = "Mã câu hỏi";
            worksheet.Cells[1, 3].Value = "Danh Mục";
            worksheet.Cells[1, 4].Value = "Môn Thi";
            worksheet.Cells[1, 5].Value = "Đơn Vị";
            worksheet.Cells[1, 6].Value = "Thể loại";
            worksheet.Cells[1, 7].Value = "Mức Độ";
            worksheet.Cells[1, 8].Value = "Loại câu hỏi";
            worksheet.Cells[1, 9].Value = "Nội dung câu hỏi";

            worksheet.Cells[1, 10].Value = "Nội dung câu trả lời(1)";
            worksheet.Cells[1, 11].Value = "Giải thích(1)";
            worksheet.Cells[1, 12].Value = "Loại đáp án(1)";
            worksheet.Cells[1, 13].Value = "Nội dung câu trả lời(2)";
            worksheet.Cells[1, 14].Value = "Giải thích(2)";
            worksheet.Cells[1, 15].Value = "Loại đáp án(2)";
            worksheet.Cells[1, 16].Value = "Nội dung câu trả lời(3)";
            worksheet.Cells[1, 17].Value = "Giải thích(3)";
            worksheet.Cells[1, 18].Value = "Loại đáp án(3)";
            worksheet.Cells[1, 19].Value = "Nội dung câu trả lời(4)";
            worksheet.Cells[1, 20].Value = "Giải thích(4)";
            worksheet.Cells[1, 21].Value = "Loại đáp án(4)";

            // Lấy range vào tạo format cho range đó ở đây là từ A1 tới D1
            using (var range = worksheet.Cells["A1:U1"])
            {
                // Set PatternType
                range.Style.Fill.PatternType = ExcelFillStyle.DarkGray;
                // Set Màu cho Background
                range.Style.Fill.BackgroundColor.SetColor(Color.Aqua);
                // Canh giữa cho các text

                range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                // Set Font cho text  trong Range hiện tại
                range.Style.Font.SetFromFont(new Font("Arial", 10));
                // Set Border
                range.Style.Border.Bottom.Style = ExcelBorderStyle.Thick;
                // Set màu ch Border
                range.Style.Border.Bottom.Color.SetColor(Color.Blue);
            }

            // Đỗ dữ liệu từ list vào 
            int recordIndex = 2;
            foreach (QuestionAnswer record in data)
            {
                int count = 10;
                worksheet.Cells[recordIndex, 1].Value = (recordIndex - 1).ToString();
                worksheet.Cells[recordIndex, 2].Value = record.question.ID_Cauhoi;
                worksheet.Cells[recordIndex, 3].Value = record.question.NameDM;
                worksheet.Cells[recordIndex, 4].Value = record.question.Name_MT;
                worksheet.Cells[recordIndex, 5].Value = record.question.Name_DV;

                worksheet.Cells[recordIndex, 6].Value = record.question.Name_Type;
                worksheet.Cells[recordIndex, 7].Value = record.question.Level_Question;
                worksheet.Cells[recordIndex, 8].Value = record.question.TypeQuestion == 1 ? "Một đáp án" : "Nhiều đáp án";
                worksheet.Cells[recordIndex, 9].Value = record.question.Content_QS;
                for (int i = 0; i < record._answers.Count; i++)
                {

                    worksheet.Cells[recordIndex, count].Value = record._answers[i].Content_AS;
                    if (string.IsNullOrEmpty(record._answers[i].Explain))
                    {

                        count++;
                    }
                    else
                    {
                        worksheet.Cells[recordIndex, ++count].Value = record._answers[i].Explain;
                    }

                    worksheet.Cells[recordIndex, ++count].Value = record._answers[i].ID_TypesAs == 1 ? "Đúng" : "Sai";
                    count++;
                }

                recordIndex++;
            }



        }
        public ActionResult Test(Search search)
        {
            List<Question> questions = handleManage.Search(search);
            var list_answer = service.showAllAs(StoreProcedure.getAllAs);

            List<QuestionAnswer> data = new List<QuestionAnswer>();
            foreach (var qs in questions)
            {
                QuestionAnswer record = new QuestionAnswer();
                record.question = qs;
                foreach (TraLoi ans in list_answer)
                {
                    if (ans.ID_Cauhoi.Equals(qs.ID_Cauhoi))
                    {
                        record._answers.Add(ans);
                    }

                }
                data.Add(record);

            }


            // Gọi lại hàm để tạo file excel
            var stream = CreateExcelFile(data);
            // Tạo buffer memory strean để hứng file excel
            var buffer = stream as MemoryStream;
            // Đây là content Type dành cho file excel, còn rất nhiều content-type khác nhưng cái này mình thấy okay nhất
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            // Dòng này rất quan trọng, vì chạy trên firefox hay IE thì dòng này sẽ hiện Save As dialog cho người dùng chọn thư mục để lưu
            // File name của Excel này là ExcelDemo
            Response.AddHeader("Content-Disposition", "attachment; filename=ExcelDemo.xlsx");
            // Lưu file excel của chúng ta như 1 mảng byte để trả về response
            Response.BinaryWrite(buffer.ToArray());
            // Send tất cả ouput bytes về phía clients
            Response.Flush();
            Response.End();
            // Redirect về luôn trang index :D

            return RedirectToAction("Index","Admin");
        }
    }
}
