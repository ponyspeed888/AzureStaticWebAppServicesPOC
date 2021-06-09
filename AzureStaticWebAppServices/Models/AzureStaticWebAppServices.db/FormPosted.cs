using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace AzureStaticWebAppServices.Models
{
    [Table("FormPosted")]
    public partial class FormPosted
    {
        [System.ComponentModel.DataAnnotations.Schema.DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        [Key]
        [Column("FormPostedID")]
        public long FormPostedId { get; set; }
        [Column("ClientID")]
        public long ClientId { get; set; }
        [Required]
        [Column(TypeName = "NVARCHAR(32767)")]
        public string FormData { get; set; }
        [Required]
        [Column(TypeName = "DATETIME")]
        public DateTime PostedTime { get; set; }

        [ForeignKey(nameof(ClientId))]
        [InverseProperty("FormPosteds")]
        public virtual Client Client { get; set; }
        public override string ToString() { return this.FormPostedId.ToString() ; }
    }
}
