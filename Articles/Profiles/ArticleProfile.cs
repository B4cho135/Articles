using ApiService.Models;
using Articles.Models.Articles;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Articles.Profiles
{
    public class ArticleProfile : Profile
    {
        public ArticleProfile()
        {
            CreateMap<Article, ArticleViewModel>();
            CreateMap<ArticleViewModel, Article>();
        }
    }
}
