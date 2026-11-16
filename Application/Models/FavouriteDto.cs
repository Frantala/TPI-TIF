using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Models
{
    public sealed class FavouriteDto
    {
        public int Id { get; set; }
        public int TeamId { get; set; }
        public string TeamName { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
    }
 
}
