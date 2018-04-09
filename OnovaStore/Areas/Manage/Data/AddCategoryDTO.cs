namespace OnovaStore.Areas.Manage.Data
{
    public class AddCategoryDTO
    {
        public string Name { get; set; }
        public int? ParentCategoryID { get; set; }
        public string Slug { get; set; }
        public string CategoryImage { get; set; }
    }
}