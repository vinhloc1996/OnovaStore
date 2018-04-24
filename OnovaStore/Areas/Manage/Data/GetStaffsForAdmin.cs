using System;

namespace OnovaStore.Areas.Manage.Data
{
    public class GetStaffsForAdmin
    {
        public string fullName { get; set; }
        public string staffId { get; set; }
        public DateTime addDate { get; set; }
        public double salary { get; set; }
        public string email { get; set; }
        public string role { get; set; }
    }
}