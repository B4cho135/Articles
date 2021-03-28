using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Articles.Models.Articles
{
    public class ArticleViewModel : BaseModel<int>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Phone { get; set; }
        public byte[] Image { get; set; }

        public Error Exception { get; set; } = new Error();
    }
}
