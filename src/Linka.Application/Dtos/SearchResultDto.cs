using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linka.Application.Dtos
{
    public class SearchResultDto
    {
        public string Type { get; set; }      
        public string DisplayName { get; set; } 
        public Guid Id { get; set; }     
        public int MatchScore { get; set; }
    }
}
