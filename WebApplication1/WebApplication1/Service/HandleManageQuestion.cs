using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using WebApplication1.Models;

namespace WebApplication1.Service
{
    public class HandleManageQuestion
    {
        public string connectionString = ConfigurationManager.ConnectionStrings["connection"].ToString();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connection"].ConnectionString);

        /// <summary>
        /// get table danh muc
        /// </summary>
        /// <param name="nameProcedure"></param>
        /// <param name="id_dm"></param>
        /// <returns></returns>
        public DataTable SearchByIDDM(string nameProcedure,int id_dm)
        {
            SqlCommand com = new SqlCommand(nameProcedure, con);
            com.CommandType = CommandType.StoredProcedure;
           
            com.Parameters.Add(new SqlParameter("@ID_DM", id_dm));
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable ds = new DataTable();
            da.Fill(ds);
            List<DanhMuc> list_data = new List<DanhMuc>();
            foreach (DataRow item in ds.Rows)
            {
                DanhMuc record = new DanhMuc();
                record.ID_DM = Convert.ToInt32(item["ID_DM"]);
                record.NameDM = item["NameDM"].ToString();

                list_data.Add(record);

            }
            Console.WriteLine(list_data.Count);
            return ds;

        }

        /// <summary>
        /// delete question& answer
        /// </summary>
        /// <param name="nameProcedure"></param>
        /// <param name="id_question"></param>
        public void DeletedQuestionAndAnswer(string nameProcedure, string id_question)
        {
            SqlCommand com = new SqlCommand(nameProcedure, con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@ID_Cauhoi", id_question);
            con.Open();
            com.ExecuteNonQuery();
            con.Close();

        }

       
        /// <summary>
        /// handle status question
        /// </summary>
        /// <param name="nameProcedure"></param>
        /// <param name="id_question"></param>
        /// <param name="index"></param>
        public void UpdateStatus(string nameProcedure, string id_question,int index)
        {
            SqlCommand com = new SqlCommand(nameProcedure, con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@ID_Cauhoi", id_question);
            com.Parameters.AddWithValue("@value", index);
            con.Open();
            com.ExecuteNonQuery();
            con.Close();

        }

        /// <summary>
        ///Search
        /// </summary>
        /// <param name="nameProcedure"></param>
        /// <param name="id_dm"></param>
        /// <param name="id_dv"></param>
        /// <param name="id_mt"></param>
        /// <param name="id_typeQt"></param>
        /// <param name="status"></param>
        /// <param name="enddate"></param>
        /// <param name="startdate"></param>
        /// <returns> ListQuestion</returns>
        public List<Question> Find(string nameProcedure, int? id_dm,int? id_dv,int? id_mt,int? id_typeQt,int? status,DateTime? enddate,DateTime? startdate)
        {
            
            SqlCommand com = new SqlCommand(nameProcedure, con);
            com.CommandType = CommandType.StoredProcedure;
            //com.Parameters.AddWithValue("@ID_DM", (System.Data.SqlDbType.Int).id_dm = 0 ? DBNull.Value : id_dm);
            com.Parameters.AddWithValue("@ID_DM", System.Data.SqlDbType.Int).Value = id_dm == 0 ? DBNull.Value : (object)id_dm;
            com.Parameters.AddWithValue("@ID_DV", System.Data.SqlDbType.Int).Value = id_dv == 0 ? DBNull.Value : (object)id_dv;
            com.Parameters.AddWithValue("@ID_MT", System.Data.SqlDbType.Int).Value = id_mt == 0 ? DBNull.Value : (object)id_mt;
            com.Parameters.AddWithValue("@ID_Type", System.Data.SqlDbType.Int).Value = id_typeQt == 0 ? DBNull.Value : (object)id_typeQt;
            com.Parameters.AddWithValue("@Status", System.Data.SqlDbType.Int).Value = status == -1 ? DBNull.Value : (object)status;
            com.Parameters.AddWithValue("@End_Date", System.Data.SqlDbType.DateTime).Value = enddate == null ? DBNull.Value : (object)enddate;
            com.Parameters.AddWithValue("@StartDate", System.Data.SqlDbType.DateTime).Value = startdate == null ? DBNull.Value : (object)startdate;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable ds = new DataTable();
            da.Fill(ds);
            List<Question> list_data = new List<Question>();
            foreach (DataRow item in ds.Rows)
            {
                Question record = new Question();
                
                record.ID_Cauhoi = item["ID_Cauhoi"].ToString();
                record.Content_QS = item["Content_QS"].ToString();
                record.TypeQuestion = Int32.Parse(item["Type_question"].ToString());
                record.Name_Type = item["Name_Type"].ToString();
                record.Level_Question = Int32.Parse(item["Level_Question"].ToString()) == 1 ? "Dễ" : "Khó";
                record.NameDM = item["NameDM"].ToString();
                record.Name_MT = item["Name_MT"].ToString();
                record.Status = Int32.Parse(item["Status"].ToString()) == 1 ? "Đã duyệt" : "chưa duyệt";
                record.EndDate = string.IsNullOrEmpty(item["EndDate"].ToString()) ? (DateTime?)null : DateTime.Parse(item["EndDate"].ToString());

                list_data.Add(record);

            }
            return list_data;
            
        }
       

        /// <summary>
        /// get Question by ID
        /// </summary>
        /// <param name="nameProcedure"></param>
        /// <param name="id_question"></param>
        /// <returns></returns>
        public Question getQuestionByID(string nameProcedure, string id_question)
        {
            SqlCommand com = new SqlCommand(nameProcedure, con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@ID_Cauhoi", id_question);
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable ds = new DataTable();
            da.Fill(ds);
            Question record = new Question();
            foreach (DataRow item in ds.Rows)
            {
                
                record.ID_Cauhoi = item["ID_Cauhoi"] != null ? item["ID_Cauhoi"].ToString() : "";
                record.ID_DM = item["ID_DM"] != null ? Int32.Parse(item["ID_DM"].ToString()): 0;
                record.TypeQuestion = item["Type_question"] != null ? Int32.Parse(item["Type_question"].ToString()) : 0;
                record.ID_DV = item["ID_DV"]  != null ? Int32.Parse(item["ID_DV"].ToString()): 0;
               
                record.ID_MT = item["ID_MT"] != null ? Int32.Parse(item["ID_MT"].ToString()) : 0;
                
                record.ID_Type = Int32.Parse(item["ID_Type"].ToString());
               
                record.Content_QS = item["Content_QS"].ToString();
                record.UrlFile = item["UrlFile"].ToString();
                record.Level_Question = Int32.Parse(item["Level_Question"].ToString()) == 1 ? "Dễ" : "Khó";
                record.EndDate = string.IsNullOrEmpty(item["EndDate"].ToString()) ? (DateTime ?)null : DateTime.Parse(item["EndDate"].ToString());
                  
            }
            
            return record;

        }

        /// <summary>
        /// get Answer by id
        /// </summary>
        /// <param name="nameProcedure"></param>
        /// <param name="id_question"></param>
        /// <returns></returns>
        public List<TraLoi> getAnswerByID(string nameProcedure, string id_question)
        {
            SqlCommand com = new SqlCommand(nameProcedure, con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@ID_Cauhoi", id_question);
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable ds = new DataTable();
            da.Fill(ds);
            List<TraLoi> list_ans = new List<TraLoi>();
            foreach (DataRow item in ds.Rows)
            {
                TraLoi record = new TraLoi();
                record.ID_Cauhoi = item["ID_Cauhoi"].ToString();
                record.ID_AS = Int32.Parse(item["ID_AS"].ToString());
                record.Content_AS = item["Content_AS"].ToString();
                record.Explain = item["Explain"].ToString();
                record.ID_TypesAs = Int32.Parse(item["ID_TypeAs"].ToString());
                list_ans.Add(record);
            }

            return list_ans;

        }

        public List<Question> Search(Search search)
        {

            SqlCommand com = new SqlCommand("Search2", con);
            com.CommandType = CommandType.StoredProcedure;
            //com.Parameters.AddWithValue("@ID_DM", (System.Data.SqlDbType.Int).id_dm = 0 ? DBNull.Value : id_dm);
            com.Parameters.AddWithValue("@ID_DM", System.Data.SqlDbType.Int).Value = search.Dm01 == 0 ? DBNull.Value : (object)search.Dm01;
            com.Parameters.AddWithValue("@ID_DV", System.Data.SqlDbType.Int).Value = search.Dv01 == 0 ? DBNull.Value : (object)search.Dv01;
            com.Parameters.AddWithValue("@ID_MT", System.Data.SqlDbType.Int).Value = search.Mt01 == 0 ? DBNull.Value : (object)search.Mt01;
            com.Parameters.AddWithValue("@ID_Type", System.Data.SqlDbType.Int).Value = search.TypeQT01 == 0 ? DBNull.Value : (object)search.TypeQT01;
            com.Parameters.AddWithValue("@Status", System.Data.SqlDbType.Int).Value = search.Status01 == -1 ? DBNull.Value : (object)search.Status01;
            com.Parameters.AddWithValue("@End_Date", System.Data.SqlDbType.DateTime).Value = search.datepicker2 == null ? DBNull.Value : (object)search.datepicker2;
            com.Parameters.AddWithValue("@StartDate", System.Data.SqlDbType.DateTime).Value = search.datepicker1 == null ? DBNull.Value : (object)search.datepicker1;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable ds = new DataTable();
            da.Fill(ds);
            List<Question> list_data = new List<Question>();
            foreach (DataRow item in ds.Rows)
            {
                Question record = new Question();

                record.ID_Cauhoi = item["ID_Cauhoi"].ToString();
                record.Content_QS = item["Content_QS"].ToString();
                record.TypeQuestion = string.IsNullOrEmpty(item["Type_question"].ToString()) ? 0 : Int32.Parse(item["Type_question"].ToString());
                record.Name_Type = item["Name_Type"].ToString();
                record.Level_Question = Int32.Parse(item["Level_Question"].ToString()) == 1 ? "Dễ" : "Khó";
                record.NameDM = item["NameDM"].ToString();
                record.Name_DV = string.IsNullOrEmpty(item["NameDV"].ToString()) ? "" : item["NameDV"].ToString();
                record.Name_MT = item["Name_MT"].ToString();
                record.Status = Int32.Parse(item["Status"].ToString()) == 1 ? "Đã duyệt" : "chưa duyệt";
                record.EndDate = string.IsNullOrEmpty(item["EndDate"].ToString()) ? (DateTime?)null : DateTime.Parse(item["EndDate"].ToString());

                list_data.Add(record);

            }
            return list_data;

        }


    }
}