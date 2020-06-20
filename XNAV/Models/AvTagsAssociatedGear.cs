using System;
using System.Collections.Generic;

namespace XNAV.Models
{
    public partial class AvTagsAssociatedGear
    {
        public int Id { get; set; }
        public int TagsId { get; set; }
        public int EquipmentId { get; set; }

        public AvEquipment Equipment { get; set; }
        public AvTags Tags { get; set; }
    }
}
