using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace dotnet_resources_api.Models
{
    public partial class files
    {
        [Key]
        public int rowid { get; set; }

        [Required]
        [Column(TypeName = "varchar(500)")]
        public string filename { get; set; }

        [Required]
        [Column(TypeName = "varchar(MAX)")]
        public string filedata { get; set; }

        [Required]
        [Column(TypeName = "int")]
        public int class_id { get; set; }

        [Column(TypeName = "int")]
        [DefaultValue(0)]
        public int class_order { get; set; }

        [Required]
        [Column(TypeName = "varchar(50)")]
        public string createdby { get; set; }

        [Required]
        [Column(TypeName = "datetime")]
        public DateTime createdon { get; set; }
    }
}
