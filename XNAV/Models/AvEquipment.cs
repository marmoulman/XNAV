using System;
using System.Collections.Generic;

namespace XNAV.Models
{
    public partial class AvEquipment
    {
        public AvEquipment()
        {
            AvTagsAssociatedGear = new HashSet<AvTagsAssociatedGear>();
        }

        public int Id { get; set; }
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public string Description { get; set; }
        public decimal? Cost { get; set; }
        public string Url { get; set; }

        public ICollection<AvTagsAssociatedGear> AvTagsAssociatedGear { get; set; }
    }
}
