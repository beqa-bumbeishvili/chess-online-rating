namespace Online_Chess_Server.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Rating
    {
        public int ID { get; set; }

        public int? UserID { get; set; }

        [Column(TypeName = "date")]
        public DateTime? RatingChangeDate { get; set; }

        public int RatingValue { get; set; }

        public virtual User User { get; set; }
    }
}
