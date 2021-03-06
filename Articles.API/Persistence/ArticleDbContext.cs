using Articles.API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Articles.API.Persistence
{
    public class ArticleDbContext : DbContext
    {
        public ArticleDbContext(DbContextOptions<ArticleDbContext> options) : base(options)
        {

        }

        public DbSet<Article> Articles { get; set; }
    }
}
