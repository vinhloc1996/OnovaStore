using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnovaApi.Models.DatabaseModels
{
    public partial class Option
    {
        public Option()
        {
            OptionDetail = new HashSet<OptionDetail>();
            ProductOptionGroup = new HashSet<ProductOptionGroup>();
        }

        [Column("OptionID")]
        public int OptionId { get; set; }
        [Required]
        [StringLength(50)]
        public string OptionName { get; set; }

        [InverseProperty("Option")]
        public ICollection<OptionDetail> OptionDetail { get; set; }
        [InverseProperty("Option")]
        public ICollection<ProductOptionGroup> ProductOptionGroup { get; set; }
    }
}
