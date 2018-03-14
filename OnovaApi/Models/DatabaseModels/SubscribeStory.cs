using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnovaApi.Models.DatabaseModels
{
    public partial class SubscribeStory
    {
        [StringLength(254)]
        public string SubscribeEmail { get; set; }
        [Column("StoryID")]
        public int StoryId { get; set; }

        [ForeignKey("StoryId")]
        [InverseProperty("SubscribeStory")]
        public Story Story { get; set; }
        [ForeignKey("SubscribeEmail")]
        [InverseProperty("SubscribeStory")]
        public Subscriber SubscribeEmailNavigation { get; set; }
    }
}
