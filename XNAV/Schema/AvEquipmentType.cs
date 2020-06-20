using System;
using System.Collections.Generic;
using XNAV.Models;
using XNAV.Repositories;
using HotChocolate.Types;
using HotChocolate;
using System.Linq;
using GreenDonut;
using HotChocolate.Resolvers;

namespace XNAV.Schema
{
    public class Query
    {
        private XnavContext _context;
        public Query()
        {
            _context = new XnavContext();
        }
        public List<AvEquipment> GetGear([GraphQLDescription("Select Individual gear")] List<int> id)
        {
            return _context.AvEquipment.Where(a=>id.Contains(a.Id)).ToList();
        }

        public List<AvEquipment> Equipment()
        {
            return _context.AvEquipment.ToList();
        }
        //public Task<AvEquipment> GetGearById(int id, [DataLoader]AvEquipmentDataLoader avEquipmentDataLoader)
        //{
        //    return avEquipmentDataLoader.LoadAsync(id);
        //}


        public List<AvTags> GetTag([GraphQLDescription("Select individual Tag")] List<int> id)
        {
            return _context.AvTags.Where(a => id.Contains(a.Id)).ToList();
        }


        public List<AvTags> Tags()
        {
            return _context.AvTags.ToList();
        }


        public List<AvTags> ConnectedTags([GraphQLDescription("Tags connected to gear")] int id)
        {
            return _context.AvTagsAssociatedGear.Where(a => a.EquipmentId == id).Select(a => a.Tags).ToList();
        }
    }


    public class AvTagType : ObjectType<AvTags>
    {
        private readonly XnavContext _context;
        public AvTagType()
        {
            _context = new XnavContext();
        }

        protected override void Configure(IObjectTypeDescriptor<AvTags> descriptor)
        {
            base.Configure(descriptor);
            
        }
    }

    public class AvEquipmentType : ObjectType<AvEquipment>
    {
        private XnavContext _context;

        public AvEquipmentType()
        {
            _context = new XnavContext();
        }

        protected override void Configure(IObjectTypeDescriptor<AvEquipment> descriptor)
        {
            base.Configure(descriptor);
            descriptor.Field("tags")
                .Type<ListType<AvTagType>>()
                .Resolver(async ctx =>
            {
                var repository = ctx.Service<EquipmentRepository>();

                IDataLoader<int, AvTags[]> dataLoader = ctx.GroupDataLoader<int, AvTags>(
                    "TagsById",
                    repository.GetEquipmentAsync);
                return await dataLoader.LoadAsync(ctx.Parent<AvEquipment>().Id);
            });
            descriptor.Ignore(f => f.AvTagsAssociatedGear);

        }
    }


}
