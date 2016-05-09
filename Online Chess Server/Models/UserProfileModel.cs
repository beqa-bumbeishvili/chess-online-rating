using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Online_Chess_Server.Models
{
    public class UserProfileModel
    {
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string LichessName { get; set; }
        public int? GamesPlayedLink { get; set; }
        public int CurrentRating { get; set; }
        public string ImageName { get; set; }
        public List<ChartViewModel> Progress {get;set;}


    }
}