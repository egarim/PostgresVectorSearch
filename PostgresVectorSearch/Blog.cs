using Microsoft.EntityFrameworkCore;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostgresVectorSearch
{
    public class Blog
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }  
        public string Content { get; set; }  
        public NpgsqlTsVector SearchVector { get; set; }
    }
}
