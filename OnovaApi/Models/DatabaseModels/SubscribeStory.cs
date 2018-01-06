using System;
using System.Collections.Generic;

namespace OnovaApi.Models.DatabaseModels
{
    public partial class SubscribeStory
    {
        public string SubscribeEmail { get; set; }
        public int StoryId { get; set; }

        public Story Story { get; set; }
        public Subscriber SubscribeEmailNavigation { get; set; }
    }
}
