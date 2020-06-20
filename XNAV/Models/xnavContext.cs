using System;
using System.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace XNAV.Models
{
    public partial class XnavContext : DbContext
    {
        public XnavContext()
        {
        }

        public XnavContext(DbContextOptions<XnavContext> options)
            : base(options)
        {
        }


        public virtual DbSet<AvEquipment> AvEquipment { get; set; }
        public virtual DbSet<AvTags> AvTags { get; set; }
        public virtual DbSet<AvTagsAssociatedGear> AvTagsAssociatedGear { get; set; }



        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            if (!optionsBuilder.IsConfigured)
            {                
                optionsBuilder.UseMySQL("server=djangodb.cbwbevwh38ox.us-east-2.rds.amazonaws.com;port=3306;user=admin;password=XNAVPassword;database=xnav");
            }
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<AvEquipment>(entity =>
            {
                entity.ToTable("av_equipment", "xnav");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Cost)
                    .HasColumnName("cost")
                    .HasColumnType("decimal(10,2)");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnName("description")
                    .HasColumnType("longtext");

                entity.Property(e => e.Manufacturer)
                    .IsRequired()
                    .HasColumnName("manufacturer")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Model)
                    .IsRequired()
                    .HasColumnName("model")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Url)
                    .HasColumnName("url")
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<AvTags>(entity =>
            {
                entity.ToTable("av_tags", "xnav");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .HasColumnType("longtext");

                entity.Property(e => e.Tag)
                    .IsRequired()
                    .HasColumnName("tag")
                    .HasColumnType("longtext");
            });

            modelBuilder.Entity<AvTagsAssociatedGear>(entity =>
            {
                entity.ToTable("av_tags_associatedGear", "xnav");

                entity.HasIndex(e => e.EquipmentId)
                    .HasName("av_tags_associatedGear_equipment_id_fa437080_fk_av_equipment_id");

                entity.HasIndex(e => new { e.TagsId, e.EquipmentId })
                    .HasName("av_tags_associatedGear_tags_id_equipment_id_0c36fe18_uniq")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.EquipmentId)
                    .HasColumnName("equipment_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.TagsId)
                    .HasColumnName("tags_id")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.Equipment)
                    .WithMany(p => p.AvTagsAssociatedGear)
                    .HasForeignKey(d => d.EquipmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("av_tags_associatedGear_equipment_id_fa437080_fk_av_equipment_id");

                entity.HasOne(d => d.Tags)
                    .WithMany(p => p.AvTagsAssociatedGear)
                    .HasForeignKey(d => d.TagsId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("av_tags_associatedGear_tags_id_f3ff6f52_fk_av_tags_id");
            });

        }
    }
}
