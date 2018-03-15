using OnovaApi.Data;

namespace OnovaApi.Services
{
    public class AuthRepository
    {
        private readonly OnovaContext _context;
        public AuthRepository(OnovaContext context)
        {
            _context = context;
        }


    }
}