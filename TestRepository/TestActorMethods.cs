using ActorRepositoryLib1.Models;
using ActorRepositoryLib1.Repositories;

namespace TestRepository
{
    [TestClass]
    public sealed class TestActorMethods
    {
        private ActorsRepository _repository;


        [TestInitialize]
        public void Initialize()
        {
            _repository = new ActorsRepository();
        }

        [TestMethod]
        public void GetAllActors_ReturnsNull()
        {

            // Act
            var actors = _repository.GetAllActors();

            // Assert
            Assert.IsNotNull(actors);
            Assert.AreEqual(0, actors.Count);
        }

        [TestMethod]
        public void GetAllActors_WithSeedData()
        {
            var actor1 = new Actor { Name = "Actor 1", BirthYear = 1980 };
            var actor2 = new Actor { Name = "Actor 2", BirthYear = 1990 };
            _repository.AddActor(actor1);
            _repository.AddActor(actor2);

            var actors = _repository.GetAllActors();

            Assert.IsNotNull(actors);
            Assert.AreEqual(2, actors.Count);
            Assert.IsTrue(actors.Any(a => a.Name == "Actor 1" && a.BirthYear == 1980));
            Assert.IsTrue(actors.Any(a => a.Name == "Actor 2" && a.BirthYear == 1990));
        }

        [TestMethod]
        public void GetActorById_ReturnsNull()
        {
            var actor = _repository.GetActorById(999); // Assuming 999 is an ID that doesn't exist
            Assert.IsNull(actor);
        }

        [TestMethod]
        public void GetActorById_ReturnsActor()
        {
            var actor = new Actor { Name = "Test Actor", BirthYear = 1985 };
            var addedActor = _repository.AddActor(actor);
            var foundActor = _repository.GetActorById(addedActor.Id);
            Assert.IsNotNull(foundActor);
            Assert.AreEqual(addedActor.Id, foundActor.Id);
            Assert.AreEqual(addedActor.Name, foundActor.Name);
            Assert.AreEqual(addedActor.BirthYear, foundActor.BirthYear);
        }

        [TestMethod]
        public void AddActor_ReturnsAddedActor()
        {

            var actor = new Actor { Name = "New Actor", BirthYear = 1995 };
            var addedActor = _repository.AddActor(actor);
            Assert.IsNotNull(addedActor);
            Assert.AreEqual(0, addedActor.Id); // First actor should have ID 0
            Assert.AreEqual("New Actor", addedActor.Name);
            Assert.AreEqual(1995, addedActor.BirthYear);
        }

        [TestMethod]
        public void DeleteActor()
        {
            var actor = new Actor { Name = "Actor1", BirthYear = 1970 };
            var addedActor = _repository.AddActor(actor);
            var deletedActor = _repository.DeleteActor(addedActor.Id);
            Assert.IsNotNull(deletedActor);
            Assert.AreEqual(addedActor.Id, deletedActor.Id);
            Assert.AreEqual(addedActor.Name, deletedActor.Name);
            Assert.AreEqual(addedActor.BirthYear, deletedActor.BirthYear);
            // Verify that the actor is no longer in the repository
            var foundActor = _repository.GetActorById(addedActor.Id);
            Assert.IsNull(foundActor);

        }

        [TestMethod]
        public void DeleteActor_ReturnsNull()
        {
            // Assuming 999 is an ID that doesn't exist
            Assert.ThrowsException<Exception>(() => _repository.DeleteActor(999));
        }

        [TestMethod]
        public void UpdateActor()
        {
            var actor = new Actor { Name = "Actor1", BirthYear = 1970 };
            var addedActor = _repository.AddActor(actor);
            var updatedData = new Actor { Name = "Updated Actor1", BirthYear = 1980 };
            var updatedActor = _repository.UpdateActor(addedActor.Id, updatedData);
            Assert.IsNotNull(updatedActor);
            Assert.AreEqual(addedActor.Id, updatedActor.Id);
            Assert.AreEqual("Updated Actor1", updatedActor.Name);
            Assert.AreEqual(1980, updatedActor.BirthYear);
        }

        [TestMethod]
        public void UpdateActor_ReturnsNull()
        {
            // Assuming 999 is an ID that doesn't exist
            var updatedData = new Actor { Name = "Updated Actor1", BirthYear = 1980 };
            Assert.ThrowsException<Exception>(() => _repository.UpdateActor(999, updatedData));
        }

        [TestMethod]
        public void Get_FilterByBirthYearBefore_ReturnsCorrectActors()
        {
            _repository.AddActor(new Actor { Name = "A", BirthYear = 1970 });
            _repository.AddActor(new Actor { Name = "B", BirthYear = 1980 });
            _repository.AddActor(new Actor { Name = "C", BirthYear = 1990 });

            // Specify all parameters to resolve ambiguity
            var result = _repository.Get(birthYearBefore: 1985, birthYearAfter: null, name: null, sortBy: "Id", descending: false);

            Assert.AreEqual(2, result.Count); // Only A and B
        }

        [TestMethod]
        public void Get_FilterByBirthYearAfter_ReturnsCorrectActors()
        {
            _repository.AddActor(new Actor { Name = "A", BirthYear = 1970 });
            _repository.AddActor(new Actor { Name = "B", BirthYear = 1980 });
            _repository.AddActor(new Actor { Name = "C", BirthYear = 1990 });

            // Specify all parameters to resolve ambiguity
            var result = _repository.Get(birthYearBefore: null, birthYearAfter: 1975, name: null, sortBy: "Id", descending: false);

            Assert.AreEqual(2, result.Count); // Only B and C
        }

        [TestMethod]
        public void Get_FilterByName_ReturnsCorrectActors()
        {
            _repository.AddActor(new Actor { Name = "Alice", BirthYear = 1970 });
            _repository.AddActor(new Actor { Name = "Bob", BirthYear = 1980 });

            // Specify all parameters to resolve ambiguity
            var result = _repository.Get(birthYearBefore: null, birthYearAfter: null, name: "Ali", sortBy: "Id", descending: false);

            Assert.AreEqual(1, result.Count); // Only Alice
        }

        [TestMethod]
        public void Get_SortByNameAscending_ReturnsActorsSortedByName()
        {
            _repository.AddActor(new Actor { Name = "Charlie", BirthYear = 1990 });
            _repository.AddActor(new Actor { Name = "Alice", BirthYear = 1980 });
            _repository.AddActor(new Actor { Name = "Bob", BirthYear = 1970 });

            // Specify all parameters to resolve ambiguity
            var result = _repository.Get(birthYearBefore: null, birthYearAfter: null, name: null, sortBy: "Name", descending: false);

            Assert.AreEqual("Alice", result[0].Name);
            Assert.AreEqual("Bob", result[1].Name);
            Assert.AreEqual("Charlie", result[2].Name);
        }

        [TestMethod]
        public void Get_SortByBirthYearDescending_ReturnsActorsSortedByBirthYearDesc()
        {
            _repository.AddActor(new Actor { Name = "Charlie", BirthYear = 1990 });
            _repository.AddActor(new Actor { Name = "Alice", BirthYear = 1980 });
            _repository.AddActor(new Actor { Name = "Bob", BirthYear = 1970 });

            // Specify all parameters to resolve ambiguity
            var result = _repository.Get(birthYearBefore: null, birthYearAfter: null, name: null, sortBy: "BirthYear", descending: true);

            Assert.AreEqual(1990, result[0].BirthYear);
            Assert.AreEqual(1980, result[1].BirthYear);
            Assert.AreEqual(1970, result[2].BirthYear);
        }


    }
}

