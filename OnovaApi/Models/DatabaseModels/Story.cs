using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnovaApi.Models.DatabaseModels
{
    public partial class Story
    {
        public Story()
        {
            SubscribeStory = new HashSet<SubscribeStory>();
        }

        [Column("StoryID")]
        public int StoryId { get; set; }
        [Required]
        [StringLength(200)]
        public string StoryTitle { get; set; }
        [Required]
        [Column(TypeName = "ntext")]
        public string StoryDescription { get; set; }
        [Required]
        public byte[] LastUpdateDate { get; set; }

        [InverseProperty("Story")]
        public ICollection<SubscribeStory> SubscribeStory { get; set; }
    }
}
