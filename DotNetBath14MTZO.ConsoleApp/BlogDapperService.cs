using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetBath14MTZO.ConsoleApp
{
    public class BlogDapperService
    {
        private readonly SqlConnectionStringBuilder _sqlConnectionStringBuilder = new SqlConnectionStringBuilder()
        {
            DataSource = ".",
            InitialCatalog = "WalletDB",
            UserID = "sa",
            Password = "mtzoo@123",
            TrustServerCertificate = true
        };

        public async Task<List<BlogModel>> GetBlogs()
        {
            string query = "select * from Tbl_Blog";
            using IDbConnection dbConnection = new SqlConnection(_sqlConnectionStringBuilder.ConnectionString);
            List<BlogModel> lst = (await dbConnection.QueryAsync<BlogModel>(query)).ToList();
            return lst;

        }

        public async Task<BlogModel> GetBlog(string id)
        {
            string query = @"SELECT [BlogId]
      ,[BlogTitle]
      ,[BlogAuthor]
      ,[BlogContent]
  FROM [dbo].[Tbl_Blog] where [BlogId] = @BlogId";
            using IDbConnection connection = new SqlConnection(_sqlConnectionStringBuilder.ConnectionString);
            var item = await connection.QueryFirstOrDefaultAsync<BlogModel>(query);
            return item;
        }

        public async Task<BlogResponseModel> CreateBlog(BlogModel requestModel)
        {
            using IDbConnection connection = new SqlConnection(_sqlConnectionStringBuilder.ConnectionString);
            string query = @"INSERT INTO [dbo].[Tbl_Blog]
           ([BlogTitle]
           ,[BlogAuthor]
           ,[BlogContent])
     VALUES
           (@BlogTitle
           ,@BlogAuthor
           ,@BlogContent)";

            var result = await connection.ExecuteAsync(query, requestModel);
            string message = result > 0 ? "Saving Successful" : "Saving Failed";
            BlogResponseModel responseModel = new BlogResponseModel();
            responseModel.IsSuccess = result > 0;
            responseModel.Message = message;
            return responseModel;

        }

        public async Task<BlogResponseModel> UpdateBlog(BlogModel requestModel)
        {
            BlogResponseModel model = new BlogResponseModel();
            var item = await GetBlog(requestModel.BlogId!);
            if (item is null)
            {
                model.IsSuccess = false;
                model.Message = "Data not found";
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
                throw new Exception("Invaild Parameter");
            }

            conditions = conditions.Substring(conditions.Length - 2);

            string query = $@"UPDATE [dbo].[Tbl_Blog]
   SET {conditions}
WHERE [BlogId] = @BlogId";
            using IDbConnection dbConnection = new SqlConnection(_sqlConnectionStringBuilder.ConnectionString);
            int result = await dbConnection.ExecuteAsync(query, requestModel);
            string message = result > 0 ? "Updating Successful" : "Updating Failed";
            model.IsSuccess = result > 0;
            model.Message = message;
            return model;

        }

        public async Task<BlogResponseModel> UpsertBlog(BlogModel requestModel)
        {
            BlogResponseModel model = new BlogResponseModel();
            var item = await GetBlog(requestModel.BlogId!);
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
                throw new Exception("Invaild Parameter");
            }

            conditions = conditions.Substring(conditions.Length - 2);

            string query = $@"UPDATE [dbo].[Tbl_Blog]
   SET {conditions}
WHERE [BlogId] = @BlogId";
            using IDbConnection dbConnection = new SqlConnection(_sqlConnectionStringBuilder.ConnectionString);
            int result = await dbConnection.ExecuteAsync(query, requestModel);
            string message = result > 0 ? "Updating Successful" : "Updating Failed";
            model.IsSuccess = result > 0;
            model.Message = message;
            return model;

        }

        public async Task<BlogResponseModel> DeleteBlog(string id)
        {
            string query = @"DELETE FROM [dbo].[Tbl_Blog]
      WHERE [BlogId] = @BlogId";

            using IDbConnection db = new SqlConnection(_sqlConnectionStringBuilder.ConnectionString);
            int result = await db.ExecuteAsync(query, new BlogModel
            {
                BlogId = id
            });

            string message = result > 0 ? "Deleting Successful" : "Deleting Failed";
            BlogResponseModel model = new BlogResponseModel();
            model.IsSuccess = result > 0;
            model.Message = message;
            return model;
        }


    }
}
