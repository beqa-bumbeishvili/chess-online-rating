namespace Online_Chess_Server.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class User
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public User()
        {
            Games = new HashSet<Game>();
            Games1 = new HashSet<Game>();
            Ratings = new HashSet<Rating>();
        }

        public int ID { get; set; }

        [Required]
        [StringLength(70)]
        public string FullName { get; set; }

        [StringLength(50)]
        public string UserName { get; set; }

        [StringLength(30)]
        public string LichessName { get; set; }

        [StringLength(500)]
        public string FacebookLink { get; set; }

        [StringLength(30)]
        public string PictureID { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Game> Games { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Game> Games1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Rating> Ratings { get; set; }
    }
}
