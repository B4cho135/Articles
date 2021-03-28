using System;
using System.Collections.Generic;
using System.Text;

namespace ApiService.Models
{
    public class Article : BaseModel<int>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Phone { get; set; }
        public byte[] Image { get; set; }
    }
}
