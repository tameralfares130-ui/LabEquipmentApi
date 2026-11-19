using LabEquipmentApi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace LabEquipmentApi.Migrations;

[DbContext(typeof(LabDbContext))]
[Migration("20261119090000_InitialCreate")]
partial class InitialCreate
{
    protected override void BuildTargetModel(ModelBuilder modelBuilder)
    {

        modelBuilder
            .HasAnnotation("ProductVersion", "10.0.11")
            .HasAnnotation("Relational:MaxIdentifierLength", 63);

        NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

        modelBuilder.Entity("LabEquipmentApi.Domain.Equipment", b =>
        {
            b.Property<int>("Id")
                .ValueGeneratedOnAdd()
                .HasColumnType("integer")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            b.Property<bool>("IsAvailable")
                .HasColumnType("boolean");

            b.Property<string>("Name")
                .IsRequired()
                .HasColumnType("text");

            b.Property<string>("Room")
                .IsRequired()
                .HasColumnType("text");

            b.HasKey("Id");
            b.ToTable("EquipmentItems");

            b.HasData(
                new { Id = 1, Name = "Projector", Room = "Lab 10", IsAvailable = true },
                new { Id = 2, Name = "Laptop", Room = "Lab 10", IsAvailable = false },
                new { Id = 3, Name = "Monitor", Room = "Lab 11", IsAvailable = true });
        });

    }
}
