using OfficeOpenXml;
using OfficeOpenXml.Table;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WebApplication1.Models;
using WebApplication1.Service;

namespace WebApplication1.Controllers
{
    public class AdminController : Controller
    {
        Handle service = new Handle();
        HandleManageQuestion handleManage = new HandleManageQuestion();
        HandleUpdate svUpdate = new HandleUpdate();


        public ActionResult Index(int? Dm01, int? Dv01, int? Mt01, int? TypeQT01, int? Status01, DateTime? datepicker2, DateTime? datepicker1, int? page)
        {
            var date = DateTime.Now;
            int pageSize = 3;
            int search = 0;
            dynamic data = new ExpandoObject();
            data.list_dm = service.getALLDM(StoreProcedure.showAllDM);
            data.list_dv = service.getALLDV(StoreProcedure.showAllDV);
            data.list_mt = service.getALLMT(StoreProcedure.showAllMT);
            data.list_typeQuestion = service.getALLTypeQuestion(StoreProcedure.showAllTypeQuestion);
            data.list_as = service.showAllAs(StoreProcedure.getAllAs);

            List<StatusQuestion> list_status = new List<StatusQuestion>();
            var list_qs = new List<Question>();
            if (Dm01 != null || Dv01 != null || Mt01 != null || TypeQT01 != null || Status01 != null)
            {
                list_status.Add(new StatusQuestion { ID = -1, Name = "Trạng thái" });
                list_status.Add(new StatusQuestion { ID = 1, Name = "Đã duyệt" });
                list_status.Add(new StatusQuestion { ID = 0, Name = "Chưa duyệt" });
                data.list_status = list_status;
                list_qs = handleManage.Find(StoreProcedure.Search, Dm01, Dv01, Mt01, TypeQT01, Status01, datepicker2, datepicker1);
                ViewBag.Dm01 = Dm01;
                ViewBag.Dv01 = Dv01;
                ViewBag.Mt01 = Mt01;
                ViewBag.TypeQT01 = TypeQT01;
                ViewBag.Status01 = Status01;
                _ = datepicker1.HasValue == true ? ViewBag.startDate = datepicker1 : ViewBag.startDate == null;
                _ = datepicker2.HasValue == true ? ViewBag.endDate = datepicker2 : ViewBag.endDate == null;

                search++;

            }
            else
            {
                list_qs = service.getALLQuestion(StoreProcedure.showQuestion);
            }

            if (page > 0)
            {
                page = page;
            }
            else
            {
                page = 1;
            }
            int start = (int)(page - 1) * pageSize;

            ViewBag.pageCurrent = page;
            int totalPage = list_qs.Count();
            float totalNumsize = (totalPage / (float)pageSize);
            int numSize = (int)Math.Ceiling(totalNumsize);
            ViewBag.totalPage = totalPage;
            ViewBag.pageSize = pageSize;
            ViewBag.numSize = numSize;
            ViewBag.posts = list_qs.OrderByDescending(x => x.ID_Cauhoi).Skip(start).Take(pageSize);
            data.list_qs = list_qs.OrderByDescending(x => x.ID_Cauhoi).Skip(start).Take(pageSize);
            data.search = search;

            return View(data);


        }
        [HttpGet]
        public ActionResult AddQuestion(int idType)
        {
            int test = (int)idType;
            Console.WriteLine(test);
            var loai_ch = service.selectTypeQT(test);

            dynamic data = new ExpandoObject();
            data.loai_ch = loai_ch;
            data.idtype = test;
            data.list_dm = service.getALLDM(StoreProcedure.showAllDM);
            data.list_dv = service.getALLDV(StoreProcedure.showAllDV);
            data.list_mt = service.getALLMT(StoreProcedure.showAllMT);
            return View(data);
        }

        [HttpGet]
        public ActionResult HandleEdit(string id_question)
        {
            Console.WriteLine(id_question);
            var answer = handleManage.getAnswerByID(StoreProcedure.getAnswerByID, id_question);
           
            dynamic data = new ExpandoObject();
            data.question = handleManage.getQuestionByID(StoreProcedure.getQuestionByID, id_question);
            data.answer = handleManage.getAnswerByID(StoreProcedure.getAnswerByID, id_question);
            

            data.list_dm = service.getALLDM(StoreProcedure.showAllDM);
            data.list_dv = service.getALLDV(StoreProcedure.showAllDV);
            data.list_mt = service.getALLMT(StoreProcedure.showAllMT);

            return View(data);

        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult HandleUpdate(string answers, string question, string EndDate)
        {
            Question qs = (Question)Newtonsoft.Json.JsonConvert.DeserializeObject(question, typeof(Question));
            qs.EndDate = string.IsNullOrEmpty(EndDate) ? (DateTime?)null : DateTime.Parse(EndDate);
            List<TraLoi> list_answer = (List<TraLoi>)Newtonsoft.Json.JsonConvert.DeserializeObject(answers, typeof(List<TraLoi>));
            svUpdate.UpdateQs(StoreProcedure.UpdateQuestionByID, qs);
            foreach (var item in list_answer)
            {
                service.Insert_Answer2(item, StoreProcedure.Insert_Answer);
            }
            return Content("success");
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddData(string question, string answers, string endDate)
        {

            Question qt = (Question)Newtonsoft.Json.JsonConvert.DeserializeObject(question, typeof(Question));
            qt.EndDate = string.IsNullOrEmpty(endDate) ? (DateTime?)null : DateTime.Parse(endDate);
            qt.Id_Level = qt.Level_Question.Equals("Dễ") ? 1 : 2;
            //parse json sang list object
            List<Test> myDeserializedObjList = (List<Test>)Newtonsoft.Json.JsonConvert.DeserializeObject(answers, typeof(List<Test>));
            service.insert_question(qt, StoreProcedure.Insert_Question);

            foreach (Test tl in myDeserializedObjList)
            {
                tl.ID_Cauhoi = qt.ID_Cauhoi;
                service.Insert_Answer(tl, StoreProcedure.Insert_Answer);
            }
            return Content("Thành công");
        }


        public ActionResult searchByIdDm(int id_dm)
        {
            HandleManageQuestion handleDatabase = new HandleManageQuestion();


            handleDatabase.SearchByIDDM(StoreProcedure.SearchByIDDM, id_dm);


            List<Question> list_ch = new List<Question>();
            List<TraLoi> list_tl = new List<TraLoi>();
            Question qt = null;


            return View();
        }

        public ActionResult DeletedQuestion(string id_question)
        {

            string text = id_question;
            handleManage.DeletedQuestionAndAnswer(StoreProcedure.DeletedQuestionAndAnswer, id_question);
            return Content(text);
        }

        //xoa tất cả câu hỏi và câu trả lời
        [HttpPost]
        public ActionResult DeleteAll(string[] ischecktrue)
        {
            for (int i = 0; i < ischecktrue.Length; i++)
            {
                handleManage.DeletedQuestionAndAnswer(StoreProcedure.DeletedQuestionAndAnswer, ischecktrue[i]);
            }
            return Content("success");
        }

        //update Status
        public ActionResult UpdateStatus(string[] id_question, int index)
        {
            for (int i = 0; i < id_question.Length; i++)
            {
                handleManage.UpdateStatus(StoreProcedure.UpdateStatus, id_question[i], index);
            }
            return Content("success");
        }
        public ActionResult HandleUpdateStatus(string id_qs, int id_status)
        {


            handleManage.UpdateStatus(StoreProcedure.UpdateStatus, id_qs, id_status);

            return Content("success");
        }

        [HttpPost]
        public ActionResult UpFile(HttpPostedFileBase file)
        {

            //ctor.Products.where(q => { if(IDdm) returm filter... else return q}).wh
            string urlfile = "";
            if (file != null)
            {
                string fileName = Path.GetFileName(file.FileName);
                string path = Path.Combine(Server.MapPath("~/Upload"), fileName);
                file.SaveAs(path);
                urlfile = "Upload/" + fileName;
            }
            return Content(urlfile);
        }

        /// xu li file exel
       

        public ActionResult HandleExcel()
        {

            return View();
        }
        [HttpGet]
        public ActionResult TestExport()
        {
           
                return View();
           

        }


        
       

      

    }
}