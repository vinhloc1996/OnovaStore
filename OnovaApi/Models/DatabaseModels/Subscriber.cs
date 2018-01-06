using System;
using System.Collections.Generic;

namespace OnovaApi.Models.DatabaseModels
{
    public partial class Subscriber
    {
        public Subscriber()
        {
            SubscribeStory = new HashSet<SubscribeStory>();
        }

        public string SubscribeEmail { get; set; }
        public bool StillSubscribe { get; set; }
        public DateTime SubscribedDate { get; set; }
        public byte[] LastUpdateDate { get; set; }
        public string UnsubscribeToken { get; set; }
        public DateTime? UnsubscribeTokenExpire { get; set; }

        public ICollection<SubscribeStory> SubscribeStory { get; set; }
    }
}
