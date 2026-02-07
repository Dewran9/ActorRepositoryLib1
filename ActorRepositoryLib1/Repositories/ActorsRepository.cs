using ActorRepositoryLib1.Models;
using System;

namespace ActorRepositoryLib1.Repositories
{
    public class ActorsRepository
    {
        private List<Actor> actors = new List<Actor>(); // Initialize the list to avoid CS8618
        private int _nextId;

        public List<Actor> GetAllActors()
        {
            if (actors == null || actors.Count == 0)
            {
                return new List<Actor>();
            }
            else
            {
                return actors;
            }
        }

        // Fixing the method signature to return an Actor object or null
        public Actor? GetActorById(int id)
        {
            // Return the actor with the matching ID or null if not found
            return actors.FirstOrDefault(actor => actor.Id == id);
        }

        public Actor AddActor(Actor actor)
        {
            // Assign a unique ID to the new actor
            actor.Id = _nextId++;
            actors.Add(actor);
            return actor;
        }

        public Actor DeleteActor(int id)
        {
            var actorToDelete = actors.FirstOrDefault(actor => actor.Id == id);
            if (actorToDelete != null)
            {
                actors.Remove(actorToDelete);
                return actorToDelete;
            }
            else
            {
                throw new Exception($"Actor with ID {id} not found.");
            }
        }

        public Actor UpdateActor(int id, Actor data)
        {
            var actorToUpdate = actors.FirstOrDefault(actor => actor.Id == id);
            if (actorToUpdate != null)
            {
                // Update the properties of the existing actor with the new data
                actorToUpdate.Name = data.Name;
                actorToUpdate.BirthYear = data.BirthYear;
                return actorToUpdate;
            }
            else
            {
                throw new Exception($"Actor with ID {id} not found.");
            }
        }

        public List<Actor> Get(int? birthYearBefore = null, int? birthYearAfter = null, string? name = null)
        {
            IEnumerable<Actor> query = actors;

            if (birthYearBefore.HasValue)
                query = query.Where(a => a.BirthYear < birthYearBefore.Value);

            if (birthYearAfter.HasValue)
                query = query.Where(a => a.BirthYear > birthYearAfter.Value);

            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(a => a.Name.Contains(name, StringComparison.OrdinalIgnoreCase));

            return query.ToList();
        }

        public List<Actor> Get(int? birthYearBefore = null, int? birthYearAfter = null,
    string? name = null,
    string sortBy = "Id",
    bool descending = false)
        {
            IEnumerable<Actor> query = actors;

            if (birthYearBefore.HasValue)
                query = query.Where(a => a.BirthYear < birthYearBefore.Value);

            if (birthYearAfter.HasValue)
                query = query.Where(a => a.BirthYear > birthYearAfter.Value);

            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(a => a.Name.Contains(name, StringComparison.OrdinalIgnoreCase));

            query = sortBy switch
            {
                "Id" => descending ? query.OrderByDescending(a => a.Id) : query.OrderBy(a => a.Id),
                "Name" => descending ? query.OrderByDescending(a => a.Name) : query.OrderBy(a => a.Name),
                "BirthYear" => descending ? query.OrderByDescending(a => a.BirthYear) : query.OrderBy(a => a.BirthYear),
                _ => query.OrderBy(a => a.Id)
            };

            return query.ToList();
        }
    }
}
