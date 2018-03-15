namespace OnovaApi.Data
{
    public class Seed
    {
        private readonly OnovaContext _context;

        public Seed(OnovaContext context)
        {
            _context = context;
        }

        public void SeedUser()
        {
        }
    }
}