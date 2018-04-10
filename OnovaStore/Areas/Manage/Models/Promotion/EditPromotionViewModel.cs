using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace OnovaStore.Areas.Manage.Models.Promotion
{
    public class EditPromotionViewModel
    {
        [Required]
        [HiddenInput]
        [ReadOnly(true)]
        public int PromotionId { get; set; }
        [Required]
        [DisplayName("Promotion Status")]
        public string PromotionStatus { get; set; }
        [Required]
        [DisplayName("Promotion Name")]
        public string PromotionName { get; set; }
        [DisplayName("Promotion Description")]
        public string PromotionDescription { get; set; }
        [Required]
        [DisplayName("Promotion Code")]
        public string PromotionCode { get; set; }
        [Required]
        [HiddenInput]
        [ReadOnly(true)]
        public string TargetApply { get; set; }
        [Required]
        [DisplayName("Start Date")]
        public DateTime StartDate { get; set; }
        [Required]
        [DisplayName("End Date")]
        public DateTime EndDate { get; set; }
        [Required]
        [DisplayName("Discount")]
        public decimal PercentOff { get; set; }
        [Required]
        [HiddenInput]
        [ReadOnly(true)]
        public byte[] LastUpdateDate { get; set; }
    }
}