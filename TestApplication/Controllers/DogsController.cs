using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using TestApp.Data;
using TestApp.Models;

namespace TestApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableRateLimiting("fixed")]
    public class DogsController : ControllerBase
    {
        private readonly IDogsRepo _repository;

        public DogsController(IDogsRepo repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [EnableRateLimiting("fixed")]
        public async Task<ActionResult<IEnumerable<Dog>>> GetAllDogs(int page, int pageSize, string? attribute, string? order)
        {
            var dogs = await _repository.GetAllDogs();
            switch (attribute)
            {
                case "name":
                    if(order == "asc")
                    {
                        dogs = dogs.OrderBy(x => x.Name).ToList();
                    }
                    else if(order == "desc"){
                        dogs = dogs.OrderByDescending(x => x.Name).ToList();
                    }
                    else
                    {
                        break;
                    }
                    break;
                case "color":
                    if (order == "asc")
                    {
                        dogs = dogs.OrderBy(x => x.Color).ToList();
                    }
                    else if (order == "desc")
                    {
                        dogs = dogs.OrderByDescending(x => x.Color).ToList();
                    }
                    else
                    {
                        break;
                    }
                    break;
                case "tail_length":
                    if (order == "asc")
                    {
                        dogs = dogs.OrderBy(x => x.TailLength).ToList();
                    }
                    else if (order == "desc")
                    {
                        dogs = dogs.OrderByDescending(x => x.TailLength).ToList();
                    }
                    else
                    {
                        break;
                    }
                    break;
                case "weight":
                    if (order == "asc")
                    {
                        dogs = dogs.OrderBy(x => x.Weight).ToList();
                    }
                    else if (order == "desc")
                    {
                        dogs = dogs.OrderByDescending(x => x.Weight).ToList();
                    }
                    else
                    {
                        break;
                    }
                    break;
                default: break;
            }

            if(page == 0 || pageSize == 0)
            {
                return Ok(dogs);
            }
            else
            {
                var dogsPerPage = dogs
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                return Ok(dogsPerPage);
            }
        }

        [HttpPost]
        [EnableRateLimiting("fixed")]
        public async Task<ActionResult<Dog>> AddDog(Dog dog)
        {
            try
            {
                await _repository.AddDog(dog);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return CreatedAtRoute(nameof(GetDogById), new { Id = dog.Id }, dog);
        }

        [HttpGet("{id}", Name = "GetDogById")]
        [EnableRateLimiting("fixed")]
        public async Task<ActionResult<Dog>> GetDogById(int id)
        {
            var dog = await _repository.GetDogById(id);
            if (dog != null)
            {
                return Ok(dog);
            }

            return NotFound();
        }
    }
}
