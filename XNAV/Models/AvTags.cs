using System;
using System.Collections.Generic;

namespace XNAV.Models
{
    public partial class AvTags
    {
        public AvTags()
        {
            AvTagsAssociatedGear = new HashSet<AvTagsAssociatedGear>();
            //NewProjectBlockRequiredTags = new HashSet<NewProjectBlockRequiredTags>();
        }

        public int Id { get; set; }
        public string Tag { get; set; }
        public string Description { get; set; }

        public ICollection<AvTagsAssociatedGear> AvTagsAssociatedGear { get; set; }
        //public ICollection<NewProjectBlockRequiredTags> NewProjectBlockRequiredTags { get; set; }
    }
}
