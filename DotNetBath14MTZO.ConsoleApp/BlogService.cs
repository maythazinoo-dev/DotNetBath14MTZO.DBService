using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace DotNetBath14MTZO.ConsoleApp
{
    public class BlogService
    {
        private readonly SqlConnectionStringBuilder _sqlConnectionStringBuilder = new SqlConnectionStringBuilder()
        {
            DataSource = ".",
            InitialCatalog = "WalletDB",
            UserID = "sa",
            Password = "mtzoo@123",
            TrustServerCertificate = true
        };

        public void Read()
        {
            string query = "select * from Tbl_Blog";
            SqlConnection sqlConnection = new SqlConnection(_sqlConnectionStringBuilder.ConnectionString);
            sqlConnection.Open();
            SqlCommand cmd = new SqlCommand(query, sqlConnection);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            sqlConnection.Close();

           

            foreach (DataRow row in dt.Rows)
            {

                Console.WriteLine(row["BlogId"]);
                Console.WriteLine(row["BlogTitle"]);
                Console.WriteLine(row["BlogAuthor"]);
                Console.WriteLine(row["BlogContent"]);
            }
            

        }

        public async Task<BlogModel> GetBlog(string id)
        {
            string query = @"SELECT [BlogId]
      ,[BlogTitle]
      ,[BlogAuthor]
      ,[BlogContent]
  FROM [dbo].[Tbl_Blog] where [BlogId] = @BlogId";
            SqlConnection sqlConnection = new SqlConnection(_sqlConnectionStringBuilder.ConnectionString);
            sqlConnection.Open();
            SqlCommand cmd = new SqlCommand(query, sqlConnection);
            cmd.Parameters.AddWithValue("@BlogId", id);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);
            sqlConnection.Close();

            if (dataTable.Rows.Count == 0) return null;

            DataRow row = dataTable.Rows[0];
            BlogModel item = new BlogModel();
            item.BlogId = row["BlogId"].ToString()!;
            item.BlogTitle = row["BlogTitle"].ToString()!;
            item.BlogAuthor = row["BlogAuthor"].ToString()!;
            item.BlogContent = row["BlogContent"].ToString()!;
            return item;

        }

        public async Task<BlogResponseModel> CreateBlog(BlogModel requestModel)
        {
            string query = @"INSERT INTO [dbo].[Tbl_Blog]
           ([BlogTitle]
           ,[BlogAuthor]
           ,[BlogContent])
     VALUES
           (@BlogTitle
           ,@BlogAuthor
           ,@BlogContent)";
            SqlConnection sqlConnection = new SqlConnection(_sqlConnectionStringBuilder.ConnectionString);
            sqlConnection.Open();
            SqlCommand cmd = new SqlCommand(query);
            cmd.Parameters.AddWithValue("@BlogTitle", requestModel.BlogTitle);
            cmd.Parameters.AddWithValue("@BlogAuthor", requestModel.BlogAuthor);
            cmd.Parameters.AddWithValue("@BlogContent", requestModel.BlogContent);

            int result = cmd.ExecuteNonQuery();
            var message = result > 0 ? "Saving Successful" : "Saving Failed";
            BlogResponseModel model = new BlogResponseModel();
            model.IsSuccess = result > 0;
            model.Message = message;
            return model;
        }
        public async Task<BlogResponseModel> UpdateBlog(BlogModel requestModel)
        {

            BlogResponseModel model = new BlogResponseModel();

            var item = GetBlog(requestModel.BlogId);
            if (item is null)
            {
                model.IsSuccess = false;
                model.Message = "Not Found Data";
                return model;
            }
            string conditions = string.Empty;

            if (!string.IsNullOrEmpty(requestModel.BlogTitle))
            {
                conditions += " [BlogTitle] = @BlogTitle, ";
            }
            if (!string.IsNullOrEmpty(requestModel.BlogAuthor))
            {
                conditions += " [BlogAuthor] = @BlogAuthor, ";
            }
            if (!string.IsNullOrEmpty(requestModel.BlogContent))
            {
                conditions += " [BlogContent] = @BlogContent, ";
            }

            if (conditions.Length == 0)
            {
                throw new Exception("Invalid Parameter.");
            }
            conditions = conditions.Substring(0, conditions.Length - 2);
            string query = $@"UPDATE [dbo].[Tbl_Blog]
   SET {conditions}
WHERE [BlogId] = @BlogId";

            SqlConnection sqlConnection = new SqlConnection(_sqlConnectionStringBuilder.ConnectionString);
            sqlConnection.Open();
            SqlCommand cmd = new SqlCommand(query);
            cmd.Parameters.AddWithValue("@BlogTitle", requestModel.BlogTitle);
            cmd.Parameters.AddWithValue("@BlogAuthor", requestModel.BlogAuthor);
            cmd.Parameters.AddWithValue("@BlogContent", requestModel.BlogContent);

            int result = cmd.ExecuteNonQuery();

            var message = result > 0 ? "Saving Successful" : "Saving Failed";

            model.IsSuccess = result > 0;
            model.Message = message;
            return model;
        }

        public async Task<BlogResponseModel> UpsertBlog(BlogModel requestModel)
        {
            BlogResponseModel model = new BlogResponseModel();

            var item = GetBlog(requestModel.BlogId);
            if (item is null)
            {
                model = await CreateBlog(requestModel);
            }

            string conditions = string.Empty;

            if (!string.IsNullOrEmpty(requestModel.BlogTitle))
            {
                conditions += " [BlogTitle] = @BlogTitle, ";
            }
            if (!string.IsNullOrEmpty(requestModel.BlogAuthor))
            {
                conditions += " [BlogAuthor] = @BlogAuthor, ";
            }
            if (!string.IsNullOrEmpty(requestModel.BlogContent))
            {
                conditions += " [BlogContent] = @BlogContent, ";
            }

            if (conditions.Length == 0)
            {
                throw new Exception("Invalid Parameter.");
            }
            conditions = conditions.Substring(0, conditions.Length - 2);
            string query = $@"UPDATE [dbo].[Tbl_Blog]
   SET {conditions}
WHERE [BlogId] = @BlogId";

            SqlConnection sqlConnection = new SqlConnection(_sqlConnectionStringBuilder.ConnectionString);
            sqlConnection.Open();
            SqlCommand cmd = new SqlCommand(query);
            cmd.Parameters.AddWithValue("@BlogTitle", requestModel.BlogTitle);
            cmd.Parameters.AddWithValue("@BlogAuthor", requestModel.BlogAuthor);
            cmd.Parameters.AddWithValue("@BlogContent", requestModel.BlogContent);

            int result = cmd.ExecuteNonQuery();

            var message = result > 0 ? "Saving Successful" : "Saving Failed";

            model.IsSuccess = result > 0;
            model.Message = message;
            return model;
        }

        public async Task<BlogResponseModel> DeleteBlog(string id)
        {
            var item = GetBlog(id);
            if (item == null)
            {
                return new BlogResponseModel()
                {
                    IsSuccess = false,
                    Message = "Data not found"
                };
            }
            string query = @"DELETE FROM [dbo].[Tbl_Blog]
      WHERE [BlogId] = @BlogId";
            SqlConnection sqlConnection = new SqlConnection(_sqlConnectionStringBuilder.ConnectionString);
            sqlConnection.Open();
            SqlCommand cmd = new SqlCommand(query);
            int result = cmd.ExecuteNonQuery();
            var message = result > 0 ? "Deleting Successful" : "Deleting Failed";
            BlogResponseModel model = new BlogResponseModel();
            model.IsSuccess = result > 0;
            model.Message = message;
            return model;

        }

    }
}