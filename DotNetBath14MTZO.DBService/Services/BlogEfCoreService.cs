using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetBath14MTZO.DBService.Services
{
    public class BlogEfCoreService
    {
        private readonly AppDbContext _appDbContext;
        public BlogEfCoreService()
        {
            _appDbContext = new AppDbContext();
        }


        public async Task<List<BlogModel>> GetBlogs()
        {
            List<BlogModel> model = await _appDbContext.Blogs.AsNoTracking().ToListAsync();
            return model;
        }

        public async Task<BlogModel> GetBlog(string id)
        {
            var item = await _appDbContext.Blogs.AsNoTracking().FirstOrDefaultAsync(x => x.BlogId == id);
            return item;
        }

        public async Task<BlogResponseModel> CreateBlog(BlogModel requestModel)
        {
            await _appDbContext.Blogs.AddAsync(requestModel);
            int result = await _appDbContext.SaveChangesAsync();
            string message = result > 0 ? "Saving successful" : "Saving Failed";
            BlogResponseModel responseModel = new BlogResponseModel();
            responseModel.IsSuccess = result > 0;
            responseModel.Message = message;
            return responseModel;
        }

        public async Task<BlogResponseModel> UpdateBlog(BlogModel requestModel)
        {
            var item = await _appDbContext.Blogs.AsNoTracking().FirstOrDefaultAsync(x => x.BlogId == requestModel.BlogId);
            if (item == null)
            {
                BlogResponseModel model = new BlogResponseModel();
                model.IsSuccess = false;
                model.Message = "No Data Found";
                return model;
            }

            if (!string.IsNullOrEmpty(requestModel.BlogTitle))
            {
                item.BlogTitle = requestModel.BlogTitle;
            }
            if (!string.IsNullOrEmpty(requestModel.BlogAuthor))
            {
                item.BlogAuthor= requestModel.BlogAuthor;
            }
            if (!string.IsNullOrEmpty(requestModel.BlogContent))
            {
                item.BlogContent = requestModel.BlogContent;
            }

            _appDbContext.Entry(item).State= EntityState.Modified;
            var result = _appDbContext.SaveChanges();
            string message = result > 0 ? "Updating Successful" : "Updating Failed";
            BlogResponseModel responseModel = new BlogResponseModel();
            responseModel.IsSuccess = result > 0;
            responseModel.Message = message;
            return responseModel;


        }

        public async Task<BlogResponseModel> UpsertBlog(BlogModel requestModel)
        {
            BlogResponseModel responseModel = new BlogResponseModel();
            var item = await _appDbContext.Blogs.AsNoTracking().FirstOrDefaultAsync(x => x.BlogId == requestModel.BlogId);
            if (item == null)
            {
                responseModel = await CreateBlog(requestModel);
            }

            if (!string.IsNullOrEmpty(requestModel.BlogTitle))
            {
                item.BlogTitle = requestModel.BlogTitle;
            }
            if (!string.IsNullOrEmpty(requestModel.BlogAuthor))
            {
                item.BlogAuthor = requestModel.BlogAuthor;
            }
            if (!string.IsNullOrEmpty(requestModel.BlogContent))
            {
                item.BlogContent = requestModel.BlogContent;
            }

            _appDbContext.Entry(item).State = EntityState.Modified;
            var result = _appDbContext.SaveChanges();
            string message = result > 0 ? "Updating Successful" : "Updating Failed";
          
            responseModel.IsSuccess = result > 0;
            responseModel.Message = message;
            return responseModel;


        }

        public async Task<BlogResponseModel> DeleteBlog(string id)
        {
            var item = _appDbContext.Blogs.FirstOrDefault(x => x.BlogId == id);
            if (item == null) {
                return new BlogResponseModel()
                {
                    IsSuccess = false,
                    Message = "Deleting Failed"
                };
            }

            _appDbContext.Entry(item).State = EntityState.Deleted;
            var result = _appDbContext.SaveChanges();
            string message = result > 0 ? "Deleting Successful" : "Deleting Failed";
            BlogResponseModel responseModel = new BlogResponseModel();
            responseModel.IsSuccess = result > 0;   
            responseModel.Message = message;
            return responseModel;

        }

    }
}
