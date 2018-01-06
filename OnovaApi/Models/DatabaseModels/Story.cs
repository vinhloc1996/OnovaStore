using System;
using System.Collections.Generic;

namespace OnovaApi.Models.DatabaseModels
{
    public partial class Story
    {
        public Story()
        {
            SubscribeStory = new HashSet<SubscribeStory>();
        }

        public int StoryId { get; set; }
        public string StoryTitle { get; set; }
        public string StoryDescription { get; set; }
        public byte[] LastUpdateDate { get; set; }

        public ICollection<SubscribeStory> SubscribeStory { get; set; }
    }
}
