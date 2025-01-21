using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetBath14MTZO.DBService.Services
{
    public class BlogModel
    {
        public string? BlogId { get; set; }
        public string? BlogTitle { get; set; }
        public string? BlogAuthor { get; set; }
        public string? BlogContent { get; set; }
    }

    public class BlogResponseModel
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }

    public class BlogDto
    {
        public string? BlogTitle { get; set; }
        public string? BlogAuthor { get; set; }
        public string? BlogContent { get; set; }
    }

}
