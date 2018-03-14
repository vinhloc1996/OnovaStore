using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnovaApi.Models.DatabaseModels
{
    public partial class Subscriber
    {
        public Subscriber()
        {
            SubscribeStory = new HashSet<SubscribeStory>();
        }

        [Key]
        [StringLength(254)]
        public string SubscribeEmail { get; set; }
        public bool StillSubscribe { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime SubscribedDate { get; set; }
        [Required]
        public byte[] LastUpdateDate { get; set; }
        [StringLength(100)]
        public string UnsubscribeToken { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UnsubscribeTokenExpire { get; set; }

        [InverseProperty("SubscribeEmailNavigation")]
        public ICollection<SubscribeStory> SubscribeStory { get; set; }
    }
}
