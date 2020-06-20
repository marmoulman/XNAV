using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XNAV.Models;
using Microsoft.EntityFrameworkCore;
using HotChocolate.Resolvers;
using HotChocolate;

namespace XNAV.Repositories
{
    public class TagRepository
    {
        private readonly XnavContext _db;

        public TagRepository(XnavContext db)
        {
            _db = db;
        }

        public IQueryable<AvTags> GetAllTags()
        {
            return _db.AvTags.AsQueryable();
        }

        public async Task<ILookup<int, AvTagsAssociatedGear>> GetTagsByEquipment(
            IReadOnlyList<int> equipment,
            System.Threading.CancellationToken cancellationToken)
        {
            var filters = new List<int>();
            foreach(int gear in equipment)
            {
                filters.Add(gear);
            }

            List<AvTagsAssociatedGear> tags = await _db.AvTagsAssociatedGear.Where(a => filters.Contains(a.EquipmentId)).ToListAsync();
            return tags.ToLookup(t => t.EquipmentId);
        } 

        public async Task<AvTags> Get(int id)
        {
            return await _db.AvTags.FirstOrDefaultAsync(p => p.Id == id);
        }

        // I need a list of gearIds such that every element in the first level of the query has its gearId added. Then I need to do an async load where I check if the AvTagAssociatedGear list contains any of those IDs and return it. In theory, dataloader will sort by that?

        //public Task<IEnumerable<AvTags>> GetTagsByGear(
        //    string gear, IResolverContext db, 
        //    [Service]ITagRepository repository)
        //{
        //    System.Threading.CancellationToken cancellationToken;
        //    return db.GroupDataLoader<string, AvTags>("tagsByGear", keys => repository.GetTagsByGear(keys).ToLookup(t => t.EquipmentId)).LoadAsync(gear, cancellationToken);
        //}




    }
}
