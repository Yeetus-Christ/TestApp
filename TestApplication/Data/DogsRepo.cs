using TestApp.Models;

namespace TestApp.Data
{
    public class DogsRepo : IDogsRepo
    {
        private readonly AppDbContext _context;

        public DogsRepo(AppDbContext context)
        {
            _context = context;
        }

        public Task<Dog> AddDog(Dog dog)
        {
            if (dog == null)
            {
                throw new ArgumentNullException(nameof(dog));
            }

            if(dog.TailLength < 0)
            {
                throw new ArgumentException("Invalid tail length", nameof(dog));
            }

            if (dog.Weight < 0)
            {
                throw new ArgumentException("Invalid weight", nameof(dog));
            }

            if (_context.Dogs.Select(x => x).Where(x => x.Name == dog.Name).Any())
            {
                throw new ArgumentException($"The dog with the name {dog.Name} already exists", nameof(dog));
            }

            _context.Dogs.Add(dog);
            _context.SaveChanges();
            return Task.FromResult(dog);
        }

        public Task<List<Dog>> GetAllDogs()
        {
            return Task.FromResult(_context.Dogs.ToList());
        }

        public Task<Dog> GetDogById(int id)
        {
            var result = _context.Dogs.FirstOrDefault(p => p.Id == id);

            return Task.FromResult(result);
        }
    }
}
