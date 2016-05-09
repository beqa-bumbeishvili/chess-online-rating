namespace Online_Chess_Server.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Game
    {
        public int ID { get; set; }

        public int? FirstPlayerID { get; set; }

        public int? SecondPlayerID { get; set; }

        public int? Result { get; set; }

        [Required]
        [StringLength(100)]
        public string GameLink { get; set; }

        [Column(TypeName = "date")]
        public DateTime? PlayDate { get; set; }

        public virtual User User { get; set; }

        public virtual User User1 { get; set; }
    }
}
