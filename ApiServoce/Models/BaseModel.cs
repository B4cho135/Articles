using System;
using System.Collections.Generic;
using System.Text;

namespace ApiService.Models
{
    public class BaseModel<T>
    {
        public T Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}
