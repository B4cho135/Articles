using ApiService;
using ApiService.Models;
using Articles.Models;
using Articles.Models.Articles;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Refit;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Articles.Controllers
{
    public class ArticlesController : Controller
    {
        private readonly ILogger<ArticlesController> _logger;
        private readonly IMapper _mapper;

        private ApiClient apiClient;

        public ArticlesController(
            ILogger<ArticlesController> logger, 
            ApiClient apiClient, 
            IMapper mapper)
        {
            _logger = logger;
            this.apiClient = apiClient;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index(ArticlesListViewModel articlesListViewModel)
        {
            var articles = new List<Article>();
            try
            {
                articles = await apiClient.Articles.GetAll(new ApiService.Models.SearchQueries.ArticleSearchQuery
                {
                    Title = articlesListViewModel.ArticleSearchQuery != null ? articlesListViewModel.ArticleSearchQuery.Title : null
                });
            }
            catch(ApiException ex)
            {
                if(ex.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return View(articlesListViewModel);
                }

                articlesListViewModel.Error = articlesListViewModel.Error = new Error { Message = "მოხდა შეცდომა, გთხოვთ სცადოთ მოგვიანებით" };
                return View(articlesListViewModel);
            }


            if(articles != null && articles.Count > 0)
            {
                var result = _mapper.Map<List<Article>, List<ArticleViewModel>>(articles);
                articlesListViewModel.Articles = result;
            }
            return View(articlesListViewModel);
        }

        public async Task<IActionResult> Details(int Id)
        {
            var articleViewModel = new ArticleViewModel();
            try
            {
                var article = await apiClient.Articles.Get(Id);

                articleViewModel = _mapper.Map<Article, ArticleViewModel>(article);
            }
            catch(ApiException ex)
            {
                if(ex.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    articleViewModel.Exception.Message = "განცხადება ვერ მოიძებნა";
                    return View(articleViewModel);
                }

                articleViewModel.Exception = articleViewModel.Exception = new Error { Message = "მოხდა შეცდომა, გთხოვთ სცადოთ მოგვიანებით" };
                return View(articleViewModel);
            }

            return View(articleViewModel);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new ArticleViewModel());
        }


        [HttpPost]
        public async Task<IActionResult> Create(ArticleViewModel article, IFormFile Image)
        {
            if(string.IsNullOrEmpty(article.Title))
            {
                article.Exception.Message = "სათაური აუცილებელია!";

                return View(article);
            }

            if (string.IsNullOrEmpty(article.Phone) || (!string.IsNullOrEmpty(article.Phone) && article.Phone.Any(x => !char.IsDigit(x))))
            {
                article.Exception.Message = "ტელეფონის ნომერი აუცილებელია და უნდა შეიცავდეს მხოლოდ ციფრებს";

                return View(article);
            }
            if (Image != null)
            {
                using (var stream = new MemoryStream())
                {
                    await Image.CopyToAsync(stream);
                    article.Image = stream.ToArray();

                    try
                    {
                        var result = _mapper.Map<ArticleViewModel, Article>(article);

                        await apiClient.Articles.Add(result);

                        return RedirectToAction("Index");

                    }
                    catch
                    {
                        article.Exception.Message = "განცხადების ატვირთვისას მოხდა შეცდომა, გთხოვთ სცადოთ მოგვიანებით";

                        return View(article);
                    }
                }


            }


            article.Exception.Message = "სურათი აუცილებელია!";

            return View(article);
        }


        [HttpGet]
        public async Task<IActionResult> Update(int Id)
        {
            try
            {
                var existingArticle = await apiClient.Articles.Get(Id);

                var result = _mapper.Map<Article, ArticleViewModel>(existingArticle);

                if(existingArticle != null)
                {
                    return View(result);
                }

                var articleListViewModel = new ArticlesListViewModel();
                articleListViewModel.Error.Message = $"განცხადება {Id} აიდით არ არსებობს";
                return RedirectToAction("Index", articleListViewModel);
            }
            catch
            {
                var articleListViewModel = new ArticlesListViewModel();
                articleListViewModel.Error.Message = "მოხდა შეცდომა";
                return RedirectToAction("Index", articleListViewModel);
            }
            
        }

        [HttpPost]
        public async Task<IActionResult> Update(ArticleViewModel article, IFormFile Image)
        {

            if (string.IsNullOrEmpty(article.Title))
            {
                article.Exception.Message = "სათაური აუცილებელია!";

                return View(article);
            }

            if (string.IsNullOrEmpty(article.Phone) || (!string.IsNullOrEmpty(article.Phone) && article.Phone.Any(x => !char.IsDigit(x))))
            {
                article.Exception.Message = "ტელეფონის ნომერი აუცილებელია და უნდა შეიცავდეს მხოლოდ ციფრებს";

                return View(article);
            }

            try
            {
                var existingArticle = await apiClient.Articles.Get(article.Id);


                if (existingArticle != null)
                {
                    var result = _mapper.Map<ArticleViewModel, Article>(article);


                    if (Image != null)
                    {
                        using (var stream = new MemoryStream())
                        {
                            await Image.CopyToAsync(stream);
                            result.Image = stream.ToArray();

                        }

                    }
                    else
                    {
                        result.Image = existingArticle.Image != null ? existingArticle.Image : null;
                    }

                    await apiClient.Articles.Update(result);

                    return RedirectToAction("Index");
                }

                var articleListViewModel = new ArticlesListViewModel();
                articleListViewModel.Error.Message = $"განცხადება {article.Id} აიდით არ არსებობს";
                return RedirectToAction("Index", articleListViewModel);
            }
            catch(ApiException ex)
            {
                var articleListViewModel = new ArticlesListViewModel();
                if(ex.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    articleListViewModel.Error.Message = $"განცხადება {article.Id} აიდით არ არსებობს";
                    return RedirectToAction("Index", articleListViewModel);
                }
                articleListViewModel.Error.Message = "მოხდა შეცდომა";
                return RedirectToAction("Index", articleListViewModel);
            }

            
        }

        public async Task<IActionResult> Delete(int Id)
        {
            try
            {
                await apiClient.Articles.Delete(Id);
                return RedirectToAction("Index");
            }
            catch(ApiException ex)
            {
                var articleListViewModel = new ArticlesListViewModel();
                if(ex.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    articleListViewModel.Error.Message = $"განცხადება {Id} აიდით არ არსებობს";
                    return RedirectToAction("Index", articleListViewModel);
                }
                articleListViewModel.Error.Message = "მოხდა უცნობი შეცდომა, გთხოვთ სცადოთ მოგვიანებით";
                return RedirectToAction("Index", articleListViewModel);
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
