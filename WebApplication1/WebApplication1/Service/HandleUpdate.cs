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
    public class HandleUpdate
    {
        public string connectionString = ConfigurationManager.ConnectionStrings["connection"].ToString();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connection"].ConnectionString);

        public void UpdateQs(string nameProcedure, Question qt)
        {
            var connection = new SqlConnection(connectionString);
            try
            {
               
                connection.Open();
                
                var command = connection.CreateCommand();
                
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = nameProcedure;
                command.Parameters.Add(new SqlParameter("@ID_Cauhoi", qt.ID_Cauhoi));
                command.Parameters.Add(new SqlParameter("@ID_DM", qt.ID_DM));
                command.Parameters.Add(new SqlParameter("@ID_DV", qt.ID_DV));
                command.Parameters.Add(new SqlParameter("@ID_MT", qt.ID_MT));
                command.Parameters.Add(new SqlParameter("@TypeQuestion", qt.TypeQuestion));
                
                command.Parameters.Add(new SqlParameter("@Content_QS", qt.Content_QS));

                command.Parameters.Add(new SqlParameter("@UrlFile", string.IsNullOrEmpty(qt.UrlFile) ? (object)DBNull.Value : qt.UrlFile));
                command.Parameters.Add(new SqlParameter("@Level_Question", Int32.Parse(qt.Level_Question)));
                //command.Parameters.Add(new SqlParameter("@EndDate",qt.EndDate));
                command.Parameters.Add(new SqlParameter("@EndDate", string.IsNullOrEmpty(qt.EndDate.ToString()) ? (object)DBNull.Value : qt.EndDate));


                command.ExecuteNonQuery();
                con.Close();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
           
        }
    }
}