using Articles.Models.Articles;
using Articles.Models.SearchQueries;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Articles.Models.Articles
{
    public class ArticlesListViewModel
    {
        public ArticleSearchQuery ArticleSearchQuery { get; set; }

        public List<ArticleViewModel> Articles { get; set; } = new List<ArticleViewModel>();

        public Error Error { get; set; } = new Error();
    }
}
