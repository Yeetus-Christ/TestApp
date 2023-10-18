using TestApp.Models;

namespace TestApp.Data
{
    public interface IDogsRepo
    {
        Task<List<Dog>> GetAllDogs();
        Task<Dog> AddDog(Dog dog);
        Task<Dog> GetDogById(int id);
    }
}
