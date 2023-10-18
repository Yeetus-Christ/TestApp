using Moq.EntityFrameworkCore;
using Moq;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;
using TestApp.Models;
using TestApp.Data;
using System.Drawing;

namespace TestAppUnitTests
{
    public class UnitTests
    {
        private readonly List<Dog> _dogs = new List<Dog>()
        {
            new Dog(){Id = 1, Name = "Jessy1", Color = "black", TailLength = 10, Weight = 20},
            new Dog(){Id = 2, Name = "Jessy2", Color = "white", TailLength = 11, Weight = 21},
            new Dog(){Id = 3, Name = "Jessy3", Color = "grey", TailLength = 12, Weight = 22},
            new Dog(){Id = 4, Name = "Jessy4", Color = "amber", TailLength = 13, Weight = 23},
            new Dog(){Id = 5, Name = "Jessy5", Color = "orange", TailLength = 14, Weight = 24},
            new Dog(){Id = 6, Name = "Neo1", Color = "black", TailLength = 15, Weight = 25},
            new Dog(){Id = 7, Name = "Neo2", Color = "white", TailLength = 16, Weight = 26},
            new Dog(){Id = 8, Name = "Neo3", Color = "grey", TailLength = 17, Weight = 27},
            new Dog(){Id = 9, Name = "Neo4", Color = "amber", TailLength = 18, Weight = 28},
            new Dog(){Id = 10, Name = "Neo5", Color = "orange", TailLength = 19, Weight = 29},
        };

        private readonly Dog createdDog = new Dog() { Id = 11, Name = "Leo6", Color = "grey", TailLength = 15, Weight = 29};

        private readonly Dog createdDogException1 = new Dog() { Id = 11, Name = "Neo5", Color = "grey", TailLength = 15, Weight = 29 };

        [Fact]
        public async void GetAllDogs_ReturnsDogsList()
        {
            Mock<AppDbContext> _context = new Mock<AppDbContext>();

            _context.Setup(x => x.Dogs).Returns(DbContextMock.GetQueryableMockDbSet(_dogs));
            _context.Setup(p => p.SaveChanges()).Returns(1);

            IDogsRepo service = new DogsRepo(_context.Object);
            var dogs = await service.GetAllDogs();

            Assert.NotNull(dogs);
            Assert.Equal(10, dogs.Count);
        }

        [Theory]
        [MemberData(nameof(DogTestDataPagination1))]
        [MemberData(nameof(DogTestDataPagination2))]
        [MemberData(nameof(DogTestDataPagination3))]
        [MemberData(nameof(DogTestDataPagination4))]
        public async void GetAllDogsWithPagination(List<Dog> dogList, int pageNumber, int pageSize)
        {
            Mock<AppDbContext> _context = new Mock<AppDbContext>();

            _context.Setup(x => x.Dogs).Returns(DbContextMock.GetQueryableMockDbSet(_dogs));
            _context.Setup(p => p.SaveChanges()).Returns(1);

            IDogsRepo service = new DogsRepo(_context.Object);
            var dogs = await service.GetAllDogs();

            dogs = dogs
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

            Assert.NotNull(dogs);
            for(int i = 0; i < dogs.Count; i++)
            {
                Assert.Equal(dogs[i].Id, dogList[i].Id);
                Assert.Equal(dogs[i].Name, dogList[i].Name);
                Assert.Equal(dogs[i].Color, dogList[i].Color);
                Assert.Equal(dogs[i].TailLength, dogList[i].TailLength);
                Assert.Equal(dogs[i].Weight, dogList[i].Weight);
            }
        }

        [Theory]
        [MemberData(nameof(DogTestDataSorting1))]
        [MemberData(nameof(DogTestDataSorting2))]
        [MemberData(nameof(DogTestDataSorting3))]
        [MemberData(nameof(DogTestDataSorting4))]
        [MemberData(nameof(DogTestDataSorting5))]
        [MemberData(nameof(DogTestDataSorting6))]
        [MemberData(nameof(DogTestDataSorting7))]
        [MemberData(nameof(DogTestDataSorting8))]
        public async void GetAllDogsWithSorting(List<Dog> dogList, string attribute, string order)
        {
            Mock<AppDbContext> _context = new Mock<AppDbContext>();

            _context.Setup(x => x.Dogs).Returns(DbContextMock.GetQueryableMockDbSet(_dogs));
            _context.Setup(p => p.SaveChanges()).Returns(1);

            IDogsRepo service = new DogsRepo(_context.Object);
            var dogs = await service.GetAllDogs();

            switch (attribute)
            {
                case "name":
                    if (order == "asc")
                    {
                        dogs = dogs.OrderBy(x => x.Name).ToList();
                    }
                    else if (order == "desc")
                    {
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

            Assert.NotNull(dogs);
            for (int i = 0; i < dogs.Count; i++)
            {
                Assert.Equal(dogs[i].Id, dogList[i].Id);
                Assert.Equal(dogs[i].Name, dogList[i].Name);
                Assert.Equal(dogs[i].Color, dogList[i].Color);
                Assert.Equal(dogs[i].TailLength, dogList[i].TailLength);
                Assert.Equal(dogs[i].Weight, dogList[i].Weight);
            }
        }

        [Theory]
        [MemberData(nameof(DogTestDataPaginationAndSorting1))]
        [MemberData(nameof(DogTestDataPaginationAndSorting2))]
        [MemberData(nameof(DogTestDataPaginationAndSorting3))]
        [MemberData(nameof(DogTestDataPaginationAndSorting4))]
        [MemberData(nameof(DogTestDataPaginationAndSorting5))]
        [MemberData(nameof(DogTestDataPaginationAndSorting6))]
        [MemberData(nameof(DogTestDataPaginationAndSorting7))]
        [MemberData(nameof(DogTestDataPaginationAndSorting8))]
        public async void GetAllDogsWithPaginationAndSorting(List<Dog> dogList, string attribute, string order, int pageNumber, int pageSize)
        {
            Mock<AppDbContext> _context = new Mock<AppDbContext>();

            _context.Setup(x => x.Dogs).Returns(DbContextMock.GetQueryableMockDbSet(_dogs));
            _context.Setup(p => p.SaveChanges()).Returns(1);

            IDogsRepo service = new DogsRepo(_context.Object);
            var dogs = await service.GetAllDogs();

            switch (attribute)
            {
                case "name":
                    if (order == "asc")
                    {
                        dogs = dogs.OrderBy(x => x.Name).ToList();
                    }
                    else if (order == "desc")
                    {
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

            dogs = dogs
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

            Assert.NotNull(dogs);
            for (int i = 0; i < dogs.Count; i++)
            {
                Assert.Equal(dogs[i].Id, dogList[i].Id);
                Assert.Equal(dogs[i].Name, dogList[i].Name);
                Assert.Equal(dogs[i].Color, dogList[i].Color);
                Assert.Equal(dogs[i].TailLength, dogList[i].TailLength);
                Assert.Equal(dogs[i].Weight, dogList[i].Weight);
            }
        }

        [Fact]
        public async Task AddDog_AddsDog()
        {
            Mock<AppDbContext> _context = new Mock<AppDbContext>();

            _context.Setup(x => x.Dogs).Returns(DbContextMock.GetQueryableMockDbSet(_dogs));
            _context.Setup(p => p.SaveChanges()).Returns(1);

            IDogsRepo service = new DogsRepo(_context.Object);
            var result = await service.AddDog(createdDog);

            Assert.Equal(_context.Object.Dogs.Select(x => x).Where(x => x.Id == createdDog.Id).FirstOrDefault(), createdDog);
        }

        [Fact]
        public async Task AddDog_ThrowsExceptionWhenAlreadyExists()
        {
            Mock<AppDbContext> _context = new Mock<AppDbContext>();

            _context.Setup(x => x.Dogs).Returns(DbContextMock.GetQueryableMockDbSet(_dogs));
            _context.Setup(p => p.SaveChanges()).Returns(1);

            IDogsRepo service = new DogsRepo(_context.Object);

            await Assert.ThrowsAsync<ArgumentException>(() => service.AddDog(createdDogException1));
        }

        [Theory]
        [MemberData(nameof(DogTestDataAdd1))]
        public async Task AddDog_ThrowsExceptionWhenInvalidParameters(Dog dog)
        {
            Mock<AppDbContext> _context = new Mock<AppDbContext>();

            _context.Setup(x => x.Dogs).Returns(DbContextMock.GetQueryableMockDbSet(_dogs));
            _context.Setup(p => p.SaveChanges()).Returns(1);

            IDogsRepo service = new DogsRepo(_context.Object);

            await Assert.ThrowsAsync<ArgumentException>(() => service.AddDog(dog));
        }

        [Fact]
        public async Task AddDog_ThrowsExceptionWhenNullObject()
        {
            Mock<AppDbContext> _context = new Mock<AppDbContext>();

            _context.Setup(x => x.Dogs).Returns(DbContextMock.GetQueryableMockDbSet(_dogs));
            _context.Setup(p => p.SaveChanges()).Returns(1);

            IDogsRepo service = new DogsRepo(_context.Object);

            await Assert.ThrowsAsync<ArgumentNullException>(() => service.AddDog(null));
        }


        public static IEnumerable<object[]> DogTestDataPagination1()
        {
            yield return new object[]
            {
                new List<Dog>
                {
                    new Dog { Id = 1, Name = "Jessy1", Color = "black", TailLength = 10, Weight = 20 },
                    new Dog { Id = 2, Name = "Jessy2", Color = "white", TailLength = 11, Weight = 21 },
                    new Dog { Id = 3, Name = "Jessy3", Color = "grey", TailLength = 12, Weight = 22 }
                },
                1,
                3
            };
        }
        public static IEnumerable<object[]> DogTestDataPagination2()
        {
            yield return new object[]
            {
                new List<Dog>
                {
                    new Dog(){Id = 4, Name = "Jessy4", Color = "amber", TailLength = 13, Weight = 23},
                    new Dog(){Id = 5, Name = "Jessy5", Color = "orange", TailLength = 14, Weight = 24},
                    new Dog(){Id = 6, Name = "Neo1", Color = "black", TailLength = 15, Weight = 25},
                },
                2,
                3
            };
        }
        public static IEnumerable<object[]> DogTestDataPagination3()
        {
            yield return new object[]
            {
                new List<Dog>
                {
                    new Dog(){Id = 7, Name = "Neo2", Color = "white", TailLength = 16, Weight = 26},
                    new Dog(){Id = 8, Name = "Neo3", Color = "grey", TailLength = 17, Weight = 27},
                    new Dog(){Id = 9, Name = "Neo4", Color = "amber", TailLength = 18, Weight = 28},
                },
                3,
                3
            };
        }
        public static IEnumerable<object[]> DogTestDataPagination4()
        {
            yield return new object[]
            {
                new List<Dog>
                {
                    new Dog(){Id = 10, Name = "Neo5", Color = "orange", TailLength = 19, Weight = 29}
                },
                4,
                3
            };
        }

        public static IEnumerable<object[]> DogTestDataSorting1()
        {
            yield return new object[]
            {
                new List<Dog>
                {
                    new Dog(){Id = 1, Name = "Jessy1", Color = "black", TailLength = 10, Weight = 20},
                    new Dog(){Id = 2, Name = "Jessy2", Color = "white", TailLength = 11, Weight = 21},
                    new Dog(){Id = 3, Name = "Jessy3", Color = "grey", TailLength = 12, Weight = 22},
                    new Dog(){Id = 4, Name = "Jessy4", Color = "amber", TailLength = 13, Weight = 23},
                    new Dog(){Id = 5, Name = "Jessy5", Color = "orange", TailLength = 14, Weight = 24},
                    new Dog(){Id = 6, Name = "Neo1", Color = "black", TailLength = 15, Weight = 25},
                    new Dog(){Id = 7, Name = "Neo2", Color = "white", TailLength = 16, Weight = 26},
                    new Dog(){Id = 8, Name = "Neo3", Color = "grey", TailLength = 17, Weight = 27},
                    new Dog(){Id = 9, Name = "Neo4", Color = "amber", TailLength = 18, Weight = 28},
                    new Dog(){Id = 10, Name = "Neo5", Color = "orange", TailLength = 19, Weight = 29},
                },
                "name",
                "asc"
            };
        }
        public static IEnumerable<object[]> DogTestDataSorting2()
        {
            yield return new object[]
            {
                new List<Dog>
                {
                    new Dog(){Id = 4, Name = "Jessy4", Color = "amber", TailLength = 13, Weight = 23},
                    new Dog(){Id = 9, Name = "Neo4", Color = "amber", TailLength = 18, Weight = 28},
                    new Dog(){Id = 1, Name = "Jessy1", Color = "black", TailLength = 10, Weight = 20},
                    new Dog(){Id = 6, Name = "Neo1", Color = "black", TailLength = 15, Weight = 25},
                    new Dog(){Id = 3, Name = "Jessy3", Color = "grey", TailLength = 12, Weight = 22},
                    new Dog(){Id = 8, Name = "Neo3", Color = "grey", TailLength = 17, Weight = 27},
                    new Dog(){Id = 5, Name = "Jessy5", Color = "orange", TailLength = 14, Weight = 24},
                    new Dog(){Id = 10, Name = "Neo5", Color = "orange", TailLength = 19, Weight = 29},
                    new Dog(){Id = 2, Name = "Jessy2", Color = "white", TailLength = 11, Weight = 21},
                    new Dog(){Id = 7, Name = "Neo2", Color = "white", TailLength = 16, Weight = 26},
                },
                "color",
                "asc"
            };
        }
        public static IEnumerable<object[]> DogTestDataSorting3()
        {
            yield return new object[]
            {
                new List<Dog>
                {
                    new Dog(){Id = 1, Name = "Jessy1", Color = "black", TailLength = 10, Weight = 20},
                    new Dog(){Id = 2, Name = "Jessy2", Color = "white", TailLength = 11, Weight = 21},
                    new Dog(){Id = 3, Name = "Jessy3", Color = "grey", TailLength = 12, Weight = 22},
                    new Dog(){Id = 4, Name = "Jessy4", Color = "amber", TailLength = 13, Weight = 23},
                    new Dog(){Id = 5, Name = "Jessy5", Color = "orange", TailLength = 14, Weight = 24},
                    new Dog(){Id = 6, Name = "Neo1", Color = "black", TailLength = 15, Weight = 25},
                    new Dog(){Id = 7, Name = "Neo2", Color = "white", TailLength = 16, Weight = 26},
                    new Dog(){Id = 8, Name = "Neo3", Color = "grey", TailLength = 17, Weight = 27},
                    new Dog(){Id = 9, Name = "Neo4", Color = "amber", TailLength = 18, Weight = 28},
                    new Dog(){Id = 10, Name = "Neo5", Color = "orange", TailLength = 19, Weight = 29},
                },
                "tail_length",
                "asc"
            };
        }
        public static IEnumerable<object[]> DogTestDataSorting4()
        {
            yield return new object[]
            {
                new List<Dog>
                {
                    new Dog(){Id = 1, Name = "Jessy1", Color = "black", TailLength = 10, Weight = 20},
                    new Dog(){Id = 2, Name = "Jessy2", Color = "white", TailLength = 11, Weight = 21},
                    new Dog(){Id = 3, Name = "Jessy3", Color = "grey", TailLength = 12, Weight = 22},
                    new Dog(){Id = 4, Name = "Jessy4", Color = "amber", TailLength = 13, Weight = 23},
                    new Dog(){Id = 5, Name = "Jessy5", Color = "orange", TailLength = 14, Weight = 24},
                    new Dog(){Id = 6, Name = "Neo1", Color = "black", TailLength = 15, Weight = 25},
                    new Dog(){Id = 7, Name = "Neo2", Color = "white", TailLength = 16, Weight = 26},
                    new Dog(){Id = 8, Name = "Neo3", Color = "grey", TailLength = 17, Weight = 27},
                    new Dog(){Id = 9, Name = "Neo4", Color = "amber", TailLength = 18, Weight = 28},
                    new Dog(){Id = 10, Name = "Neo5", Color = "orange", TailLength = 19, Weight = 29},
                },
                "weight",
                "asc"
            };
        }
        public static IEnumerable<object[]> DogTestDataSorting5()
        {
            yield return new object[]
            {
                new List<Dog>
                {
                   new Dog { Id = 10, Name = "Neo5", Color = "orange", TailLength = 19, Weight = 29 },
                   new Dog { Id = 9, Name = "Neo4", Color = "amber", TailLength = 18, Weight = 28 },
                   new Dog { Id = 8, Name = "Neo3", Color = "grey", TailLength = 17, Weight = 27 },
                   new Dog { Id = 7, Name = "Neo2", Color = "white", TailLength = 16, Weight = 26 },
                   new Dog { Id = 6, Name = "Neo1", Color = "black", TailLength = 15, Weight = 25 },
                   new Dog { Id = 5, Name = "Jessy5", Color = "orange", TailLength = 14, Weight = 24 },
                   new Dog { Id = 4, Name = "Jessy4", Color = "amber", TailLength = 13, Weight = 23 },
                   new Dog { Id = 3, Name = "Jessy3", Color = "grey", TailLength = 12, Weight = 22 },
                   new Dog { Id = 2, Name = "Jessy2", Color = "white", TailLength = 11, Weight = 21 },
                   new Dog { Id = 1, Name = "Jessy1", Color = "black", TailLength = 10, Weight = 20 }

                },
                "name",
                "desc"
            };
        }
        public static IEnumerable<object[]> DogTestDataSorting6()
        {
            yield return new object[]
            {
                new List<Dog>
                {
                    new Dog(){Id = 2, Name = "Jessy2", Color = "white", TailLength = 11, Weight = 21},
                    new Dog(){Id = 7, Name = "Neo2", Color = "white", TailLength = 16, Weight = 26},
                    new Dog(){Id = 5, Name = "Jessy5", Color = "orange", TailLength = 14, Weight = 24},
                    new Dog(){Id = 10, Name = "Neo5", Color = "orange", TailLength = 19, Weight = 29},
                    new Dog(){Id = 3, Name = "Jessy3", Color = "grey", TailLength = 12, Weight = 22},
                    new Dog(){Id = 8, Name = "Neo3", Color = "grey", TailLength = 17, Weight = 27},
                    new Dog(){Id = 1, Name = "Jessy1", Color = "black", TailLength = 10, Weight = 20},
                    new Dog(){Id = 6, Name = "Neo1", Color = "black", TailLength = 15, Weight = 25},
                    new Dog(){Id = 4, Name = "Jessy4", Color = "amber", TailLength = 13, Weight = 23},
                    new Dog(){Id = 9, Name = "Neo4", Color = "amber", TailLength = 18, Weight = 28},                  
                },
                "color",
                "desc"
            };
        }
        public static IEnumerable<object[]> DogTestDataSorting7()
        {
            yield return new object[]
            {
                new List<Dog>
                {
                    new Dog { Id = 10, Name = "Neo5", Color = "orange", TailLength = 19, Weight = 29 },
                    new Dog { Id = 9, Name = "Neo4", Color = "amber", TailLength = 18, Weight = 28 },
                    new Dog { Id = 8, Name = "Neo3", Color = "grey", TailLength = 17, Weight = 27 },
                    new Dog { Id = 7, Name = "Neo2", Color = "white", TailLength = 16, Weight = 26 },
                    new Dog { Id = 6, Name = "Neo1", Color = "black", TailLength = 15, Weight = 25 },
                    new Dog { Id = 5, Name = "Jessy5", Color = "orange", TailLength = 14, Weight = 24 },
                    new Dog { Id = 4, Name = "Jessy4", Color = "amber", TailLength = 13, Weight = 23 },
                    new Dog { Id = 3, Name = "Jessy3", Color = "grey", TailLength = 12, Weight = 22 },
                    new Dog { Id = 2, Name = "Jessy2", Color = "white", TailLength = 11, Weight = 21 },
                    new Dog { Id = 1, Name = "Jessy1", Color = "black", TailLength = 10, Weight = 20 }
                },
                "tail_length",
                "desc"
            };
        }
        public static IEnumerable<object[]> DogTestDataSorting8()
        {
            yield return new object[]
            {
                new List<Dog>
                {
                    new Dog { Id = 10, Name = "Neo5", Color = "orange", TailLength = 19, Weight = 29 },
                    new Dog { Id = 9, Name = "Neo4", Color = "amber", TailLength = 18, Weight = 28 },
                    new Dog { Id = 8, Name = "Neo3", Color = "grey", TailLength = 17, Weight = 27 },
                    new Dog { Id = 7, Name = "Neo2", Color = "white", TailLength = 16, Weight = 26 },
                    new Dog { Id = 6, Name = "Neo1", Color = "black", TailLength = 15, Weight = 25 },
                    new Dog { Id = 5, Name = "Jessy5", Color = "orange", TailLength = 14, Weight = 24 },
                    new Dog { Id = 4, Name = "Jessy4", Color = "amber", TailLength = 13, Weight = 23 },
                    new Dog { Id = 3, Name = "Jessy3", Color = "grey", TailLength = 12, Weight = 22 },
                    new Dog { Id = 2, Name = "Jessy2", Color = "white", TailLength = 11, Weight = 21 },
                    new Dog { Id = 1, Name = "Jessy1", Color = "black", TailLength = 10, Weight = 20 }
                },
                "weight",
                "desc"
            };
        }

        public static IEnumerable<object[]> DogTestDataAdd1()
        {
            yield return new object[]
            {
                new Dog { Id = 11, Name = "Leo6", Color = "grey", TailLength = -15, Weight = 29 },
            };
        }
        public static IEnumerable<object[]> DogTestDataAdd2()
        {
            yield return new object[]
            {
                new Dog { Id = 11, Name = "Leo6", Color = "grey", TailLength = 15, Weight = -29 },
            };
        }

        public static IEnumerable<object[]> DogTestDataPaginationAndSorting1()
        {
            yield return new object[]
            {
                new List<Dog>
                {
                    new Dog { Id = 10, Name = "Neo5", Color = "orange", TailLength = 19, Weight = 29 },
                    new Dog { Id = 9, Name = "Neo4", Color = "amber", TailLength = 18, Weight = 28 },
                    new Dog { Id = 8, Name = "Neo3", Color = "grey", TailLength = 17, Weight = 27 },
                },
                "weight",
                "desc",
                1,
                3
            };
        }
        public static IEnumerable<object[]> DogTestDataPaginationAndSorting2()
        {
            yield return new object[]
            {
                new List<Dog>
                {
                    new Dog { Id = 7, Name = "Neo2", Color = "white", TailLength = 16, Weight = 26 },
                    new Dog { Id = 6, Name = "Neo1", Color = "black", TailLength = 15, Weight = 25 },
                    new Dog { Id = 5, Name = "Jessy5", Color = "orange", TailLength = 14, Weight = 24 },
                },
                "weight",
                "desc",
                2,
                3
            };
        }
        public static IEnumerable<object[]> DogTestDataPaginationAndSorting3()
        {
            yield return new object[]
            {
                new List<Dog>
                {
                    new Dog(){Id = 1, Name = "Jessy1", Color = "black", TailLength = 10, Weight = 20},
                    new Dog(){Id = 2, Name = "Jessy2", Color = "white", TailLength = 11, Weight = 21},
                    new Dog(){Id = 3, Name = "Jessy3", Color = "grey", TailLength = 12, Weight = 22},
                },
                "weight",
                "asc",
                1,
                3
            };
        }
        public static IEnumerable<object[]> DogTestDataPaginationAndSorting4()
        {
            yield return new object[]
            {
                new List<Dog>
                {
                    new Dog(){Id = 4, Name = "Jessy4", Color = "amber", TailLength = 13, Weight = 23},
                    new Dog(){Id = 5, Name = "Jessy5", Color = "orange", TailLength = 14, Weight = 24},
                    new Dog(){Id = 6, Name = "Neo1", Color = "black", TailLength = 15, Weight = 25},
                },
                "weight",
                "asc",
                2,
                3
            };
        }
        public static IEnumerable<object[]> DogTestDataPaginationAndSorting5()
        {
            yield return new object[]
            {
                new List<Dog>
                {
                    new Dog(){Id = 4, Name = "Jessy4", Color = "amber", TailLength = 13, Weight = 23},
                    new Dog(){Id = 9, Name = "Neo4", Color = "amber", TailLength = 18, Weight = 28},
                    new Dog(){Id = 1, Name = "Jessy1", Color = "black", TailLength = 10, Weight = 20},
                },
                "color",
                "asc",
                1,
                3
            };
        }
        public static IEnumerable<object[]> DogTestDataPaginationAndSorting6()
        {
            yield return new object[]
            {
                new List<Dog>
                {
                    new Dog(){Id = 6, Name = "Neo1", Color = "black", TailLength = 15, Weight = 25},
                    new Dog(){Id = 3, Name = "Jessy3", Color = "grey", TailLength = 12, Weight = 22},
                    new Dog(){Id = 8, Name = "Neo3", Color = "grey", TailLength = 17, Weight = 27},
                },
                "color",
                "asc",
                2,
                3
            };
        }
        public static IEnumerable<object[]> DogTestDataPaginationAndSorting7()
        {
            yield return new object[]
            {
                new List<Dog>
                {
                    new Dog(){Id = 2, Name = "Jessy2", Color = "white", TailLength = 11, Weight = 21},
                    new Dog(){Id = 7, Name = "Neo2", Color = "white", TailLength = 16, Weight = 26},
                    new Dog(){Id = 5, Name = "Jessy5", Color = "orange", TailLength = 14, Weight = 24},
                },
                "color",
                "desc",
                1,
                3
            };
        }
        public static IEnumerable<object[]> DogTestDataPaginationAndSorting8()
        {
            yield return new object[]
            {
                new List<Dog>
                {
                    new Dog(){Id = 10, Name = "Neo5", Color = "orange", TailLength = 19, Weight = 29},
                    new Dog(){Id = 3, Name = "Jessy3", Color = "grey", TailLength = 12, Weight = 22},
                    new Dog(){Id = 8, Name = "Neo3", Color = "grey", TailLength = 17, Weight = 27},
                },
                "color",
                "desc",
                2,
                3
            };
        }
    }
}