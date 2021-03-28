using ApiService.Models;
using ApiService.Models.SearchQueries;
using Refit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApiService.Resources
{
    public interface IArticleResource
    {

        /// <summary>
        /// აბრუნებს ყველა განცხადებას სათაურით გაფილტვრის ფუნქციით
        /// </summary>
        [Get("/api/articles")]
        Task<List<Article>> GetAll(ArticleSearchQuery query);


        /// <summary>
        /// აბრუნებს კონკრეტულ განცხადებას
        /// </summary>
        [Get("/api/articles/{Id}")]
        Task<Article> Get(int Id);


        /// <summary>
        /// ამატებს ახალ განცხადებას
        /// </summary>
        [Post("/api/articles")]
        Task Add(Article article);


        /// <summary>
        /// ცვლის კონკრეტულ განცხადებას
        /// </summary>
        [Put("/api/articles")]
        Task Update(Article article);


        /// <summary>
        /// შლის კონკრეტულ განცხადებას
        /// </summary>
        [Delete("/api/articles/{Id}")]
        Task Delete(int Id);


    }
}
