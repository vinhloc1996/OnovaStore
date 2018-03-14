using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnovaApi.Models.DatabaseModels
{
    public partial class OptionDetail
    {
        public OptionDetail()
        {
            ProductOptionGroup = new HashSet<ProductOptionGroup>();
        }

        [Column("OptionDetailID")]
        public int OptionDetailId { get; set; }
        [Required]
        [StringLength(100)]
        public string OptionDetailName { get; set; }
        [Column("OptionID")]
        public int? OptionId { get; set; }

        [ForeignKey("OptionId")]
        [InverseProperty("OptionDetail")]
        public Option Option { get; set; }
        [InverseProperty("OptionDetail")]
        public ICollection<ProductOptionGroup> ProductOptionGroup { get; set; }
    }
}
