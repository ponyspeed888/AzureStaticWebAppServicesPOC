using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace AzureStaticWebAppServices.Models
{
    [Table("client")]
    public partial class Client
    {
        public Client()
        {
            FormPosteds = new HashSet<FormPosted>();
        }

        [System.ComponentModel.DataAnnotations.Schema.DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        [Key]
        [Column("ClientID")]
        public long ClientId { get; set; }
        [Required]
        [Column(TypeName = "NVARCHAR(4096)")]
        public string ClientUrl { get; set; }
        [Required]
        [Column(TypeName = "NVARCHAR(256)")]
        public string Password { get; set; }

        [InverseProperty(nameof(FormPosted.Client))]
        public virtual ICollection<FormPosted> FormPosteds { get; set; }
        public override string ToString() { return this.ClientId.ToString() ; }
    }
}
