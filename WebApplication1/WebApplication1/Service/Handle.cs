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
    public class Handle
    {
       public string connectionString = ConfigurationManager.ConnectionStrings["connection"].ToString();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connection"].ConnectionString);
       
        

        /// <summary>
        /// get all table DanhMuc
        /// </summary>
        /// <param name="sql"></param>
        /// <returns>List model Danh muc</returns>
        public List<DanhMuc> getALLDM(string sql)
        {
           
            var connection = new SqlConnection(connectionString);
            var list = new List<DanhMuc>();
            try
            {
                connection.Open();
                var command = connection.CreateCommand();

                //command.CommandText = "Select * From Category";
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = sql;

                
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    DanhMuc ob = new DanhMuc();
                    ob.ID_DM = reader["ID_DM"] != null ? Int32.Parse(reader["ID_DM"].ToString()) : 0 ;
                    ob.NameDM = reader["NameDM"]!= null ? reader["NameDM"].ToString(): "";

                    list.Add(ob);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                connection.Close();
            }
            return list;
        }


        
        /// <summary>
        /// get all table DonVi
        /// </summary>
        /// <param name="sql"></param>
        /// <returns>List Donvi</returns>
        public List<DonVi> getALLDV(string sql)
        {
            var connection = new SqlConnection(connectionString);
            var list = new List<DonVi>();
            try
            {
                connection.Open();
                var command = connection.CreateCommand();

                //command.CommandText = "Select * From Category";
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = sql;

                string id = null;
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    DonVi ob = new DonVi();
                    ob.ID_DV = reader["ID_DV"] != null ? Int32.Parse(reader["ID_DV"].ToString()): 0;
                    ob.NameDV = reader["NameDV"] != null ? reader["NameDV"].ToString(): "";

                    list.Add(ob);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                connection.Close();
            }
            return list;
        }

       /// <summary>
       /// Get all table MonThi in DB
       /// </summary>
       /// <param name="sql"></param>
       /// <returns></returns>
        public List<MonThi> getALLMT(string sql)
        {
            var connection = new SqlConnection(connectionString);
            var list = new List<MonThi>();
            try
            {
                connection.Open();
                var command = connection.CreateCommand();

                
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText =sql;

                string id = null;
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    MonThi ob = new MonThi();
                    ob.ID_MT = reader["ID_MT"] != null ? Int32.Parse(reader["ID_MT"].ToString()) : 0;
                    ob.Name_MT = reader["Name_MT"] != null ? reader["Name_MT"].ToString() : "";

                    list.Add(ob);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                connection.Close();
            }
            return list;
        }

        
        /// <summary>
        /// Get all table TypeQuestion in DB
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public List<TypeAnswer> getALLTypeQuestion(string sql)
        {
            var connection = new SqlConnection(connectionString);
            var list = new List<TypeAnswer>();
            try
            {
                connection.Open();
                var command = connection.CreateCommand();


                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = sql;

                string id = null;
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    TypeAnswer ob = new TypeAnswer();
                    ob.ID_Type = reader["ID_Type"] != null ? Int32.Parse(reader["ID_Type"].ToString()): 0;
                    ob.Name_Type = reader["Name_Type"] != null ? reader["Name_Type"].ToString() : "";

                    list.Add(ob);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                connection.Close();
            }
            return list;
        }

        
        /// <summary>
        /// Get All Table Question
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public List<Question> getALLQuestion(string sql)
        {
            var connection = new SqlConnection(connectionString);
            var list = new List<Question>();
            try
            {
                connection.Open();
                var command = connection.CreateCommand();

                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = sql;

                string id = null;
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Question ob = new Question();
                    ob.ID_Cauhoi = reader["ID_Cauhoi"] != null ? reader["ID_Cauhoi"].ToString() : "";
                    ob.TypeQuestion = Int32.Parse(reader["Type_question"].ToString());
                    ob.Content_QS = reader["Content_QS"] != null ? reader["Content_QS"].ToString(): "";
                    ob.Name_Type = reader["Name_Type"] != null ? reader["Name_Type"].ToString() : "" ;
                    ob.Level_Question =Int32.Parse(reader["Level_Question"].ToString()) ==1 ? "Dễ" : "Khó" ;
                    ob.NameDM = reader["NameDM"] != null ? reader["NameDM"].ToString() : "" ;
                    ob.Name_MT = reader["Name_MT"] != null ?  reader["Name_MT"].ToString() : "" ;
                    ob.Name_DV = reader["NameDV"] != null ? reader["NameDV"].ToString() : "";
                    ob.Status = reader["Status"] != null ? (Int32.Parse(reader["Status"].ToString()) == 1 ? "Đã duyệt" : "chưa duyệt") :"";



                    list.Add(ob);


                }
                Console.WriteLine(id);


            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                connection.Close();
            }
            return list;
        }

        /// <summary>
        /// Get ALl Table Answer
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public List<TraLoi> showAllAs(string sql)
        {

            var connection = new SqlConnection(connectionString);
            var list = new List<TraLoi>();

            connection.Open();
            var command = connection.CreateCommand();

            command.CommandType = CommandType.StoredProcedure;
            command.CommandText =sql;


            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                TraLoi tl = new TraLoi();
                tl.ID_AS = reader["ID_AS"] != null ? int.Parse(reader["ID_AS"].ToString()) : 0 ;
                tl.Content_AS = reader["Content_AS"] != null ? reader["Content_AS"].ToString() : "";
                tl.Explain = reader["Explain"] != null ? reader["Explain"].ToString() : "";
                tl.ID_TypesAs = reader["ID_TypeAs"] != null ? Int32.Parse(reader["ID_TypeAs"].ToString()) : 0;
                tl.ID_Cauhoi = reader["ID_Cauhoi"] != null ? reader["ID_Cauhoi"].ToString() : "";
                list.Add(tl);
            }
                return list;
        }

        /// <summary>
        /// Get model TypeQuetion By ID
        /// </summary>
        /// <param name="id_type"></param>
        /// <returns></returns>
        public LoaiCauHoi selectTypeQT(int id_type)
        {

            var connection = new SqlConnection(connectionString);
            var loai_ch = new LoaiCauHoi();

            connection.Open();
            var command = connection.CreateCommand();
            
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "selectTypeQuestion";
            command.Parameters.Add(new SqlParameter("@ID_Type", id_type));
            


            var reader = command.ExecuteReader();
            
            while (reader.Read())
            {
                loai_ch.ID_Type = int.Parse(reader["ID_Type"].ToString());
                loai_ch.Name_type = reader["Name_Type"].ToString();


            }
            return loai_ch;
        }

        /// <summary>
        /// Insert Question To DB
        /// </summary>
        /// <param name="qt"></param>
        /// <param name="sql"></param>
        public void insert_question(Question qt,string sql)
        {

            var connection = new SqlConnection(connectionString);
            //string linkvideo = (qt.UrlFile == String.Empty) ? DBNull : qt.UrlFile; 
            try
            {
                connection.Open();
                var command = connection.CreateCommand();

                //int leverquestion = qt.Level_Question.Equals("Dễ") ? 1 : 2;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = sql;
                command.Parameters.Add(new SqlParameter("@ID_Cauhoi", qt.ID_Cauhoi));
                command.Parameters.Add(new SqlParameter("@ID_DM", qt.ID_DM));
                command.Parameters.Add(new SqlParameter("@ID_DV", qt.ID_DV));
                command.Parameters.Add(new SqlParameter("@ID_MT", qt.ID_MT));
                command.Parameters.Add(new SqlParameter("@ID_Type", qt.ID_Type));
                command.Parameters.Add(new SqlParameter("@Content_QS", qt.Content_QS));
                command.Parameters.Add(new SqlParameter("@TypeAnswer", qt.TypeQuestion));
                command.Parameters.Add(new SqlParameter("@UrlFile", string.IsNullOrEmpty(qt.UrlFile) ? (object)DBNull.Value : qt.UrlFile));
                command.Parameters.Add(new SqlParameter("@Level_Question", qt.Id_Level));
                command.Parameters.Add(new SqlParameter("@EndDate", string.IsNullOrEmpty(qt.EndDate.ToString()) ? (object)DBNull.Value : qt.EndDate));

                int ID = command.ExecuteNonQuery();
                connection.Close();
            }
            catch(Exception e)
            {
               
                Console.WriteLine(e.Message);
            }
           
           
        }

        /// <summary>
        /// Insert Answer To DB
        /// </summary>
        /// <param name="tl"></param>
        /// <param name="sql"></param>
        public void Insert_Answer(Test tl,string sql)
        {

            var connection = new SqlConnection(connectionString);

            try
            {
                connection.Open();
                var command = connection.CreateCommand();

                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = sql;
                command.Parameters.Add(new SqlParameter("@Content_AS", tl.cautl));
                command.Parameters.Add(new SqlParameter("@Explain", tl.explain));
                command.Parameters.Add(new SqlParameter("@ID_TypeAs", tl.type_As));
                command.Parameters.Add(new SqlParameter("@ID_Cauhoi", tl.ID_Cauhoi));


                int ID = command.ExecuteNonQuery();
                connection.Close();
            }
            
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void Insert_Answer2(TraLoi tl, string sql)
        {

            var connection = new SqlConnection(connectionString);


            connection.Open();
            var command = connection.CreateCommand();

            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = sql;
            command.Parameters.Add(new SqlParameter("@Content_AS", tl.Content_AS));
            command.Parameters.Add(new SqlParameter("@Explain", tl.Explain));
            command.Parameters.Add(new SqlParameter("@ID_TypeAs", tl.ID_TypesAs));
            command.Parameters.Add(new SqlParameter("@ID_Cauhoi", tl.ID_Cauhoi));


            int ID = command.ExecuteNonQuery();
            connection.Close();

        }
    }
}