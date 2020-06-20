using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XNAV.Models;

namespace XNAV.Repositories
{
    public class EquipmentRepository
    {
        private readonly XnavContext _db;

        public EquipmentRepository()
        {
            _db = new XnavContext();
        }

        public IQueryable<AvEquipment> GetAllGear()
        {
            return _db.AvEquipment.AsQueryable();
        }

        public async Task<AvEquipment> Get(int id)
        {
            return await _db.AvEquipment.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<ILookup<int, AvTags>> GetEquipmentAsync(
            IReadOnlyCollection<int> equipmentids,
            System.Threading.CancellationToken cancellationToken)
        {
            var filters = new List<int>();
            foreach (int id in equipmentids)
            {
                filters.Add(id);
            }

            var tags = await _db.AvTagsAssociatedGear
                .Where(u => filters.Contains(u.EquipmentId))
                .Select(
                y => new { y.EquipmentId,
                    tag =_db.AvTags.FirstOrDefault(z => z.Id == y.TagsId) })
                    .ToListAsync(cancellationToken);

            return tags.ToLookup(t => t.EquipmentId, q => q.tag);
        }


    }
}
