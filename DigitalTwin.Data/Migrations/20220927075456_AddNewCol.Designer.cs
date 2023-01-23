﻿// <auto-generated />
using System;
using DigitalTwin.Data.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DigitalTwin.Data.Migrations
{
    [DbContext(typeof(DigitalTwinContext))]
    [Migration("20220927075456_AddNewCol")]
    partial class AddNewCol
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("pivot_da_middleware_digital")
                .HasAnnotation("ProductVersion", "6.0.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("DigitalTwin.Data.Entities.Entity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<decimal>("CreatedBy")
                        .HasColumnType("numeric(20,0)");

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("Depth")
                        .HasColumnType("integer");

                    b.Property<Guid>("EntityId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("EntityParentId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("EntityParentId1")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("EntityTypeId")
                        .HasColumnType("uuid");

                    b.Property<int>("EntityTypeMasterId")
                        .HasColumnType("integer");

                    b.Property<string>("KpiPath")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<decimal>("UpdatedBy")
                        .HasColumnType("numeric(20,0)");

                    b.Property<DateTime>("UpdatedTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("ValueChainTypeId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("EntityParentId1");

                    b.HasIndex("EntityTypeId");

                    b.HasIndex("ValueChainTypeId");

                    b.ToTable("Entities", "pivot_da_middleware_digital");
                });

            modelBuilder.Entity("DigitalTwin.Data.Entities.EntityType", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<decimal>("CreatedBy")
                        .HasColumnType("numeric(20,0)");

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("EntityGroupName")
                        .HasColumnType("text");

                    b.Property<int>("EntityId")
                        .HasColumnType("integer");

                    b.Property<string>("FullName")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<decimal>("UpdatedBy")
                        .HasColumnType("numeric(20,0)");

                    b.Property<DateTime>("UpdatedTime")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("EntityTypes", "pivot_da_middleware_digital");
                });

            modelBuilder.Entity("DigitalTwin.Data.Entities.ProductLink", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<decimal>("CreatedBy")
                        .HasColumnType("numeric(20,0)");

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("EntityId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("EntityMapId")
                        .HasColumnType("uuid");

                    b.Property<string>("FullName")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<decimal?>("Percentage")
                        .HasColumnType("numeric");

                    b.Property<Guid?>("UnitOfMeasureId")
                        .HasColumnType("uuid");

                    b.Property<decimal>("UpdatedBy")
                        .HasColumnType("numeric(20,0)");

                    b.Property<DateTime>("UpdatedTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<decimal?>("Value")
                        .HasColumnType("numeric");

                    b.Property<decimal?>("Variance")
                        .HasColumnType("numeric");

                    b.HasKey("Id");

                    b.HasIndex("EntityId");

                    b.HasIndex("UnitOfMeasureId");

                    b.ToTable("ProductLinks", "pivot_da_middleware_digital");
                });

            modelBuilder.Entity("DigitalTwin.Data.Entities.ProductLinkDetail", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Color")
                        .HasColumnType("text");

                    b.Property<decimal>("CreatedBy")
                        .HasColumnType("numeric(20,0)");

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("DataDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Frequency")
                        .HasColumnType("text");

                    b.Property<bool>("IsDaily")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsMonthToDate")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsMonthly")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsQuarterToDate")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsQuarterly")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsRealTime")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsWeekly")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsYearEndProjection")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsYearToDaily")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsYearToMonthly")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("NorCode")
                        .HasColumnType("text");

                    b.Property<string>("NumValues")
                        .HasColumnType("jsonb");

                    b.Property<decimal?>("Percentage")
                        .HasColumnType("numeric");

                    b.Property<Guid?>("ProductLinkId")
                        .HasColumnType("uuid");

                    b.Property<string>("UomName")
                        .HasColumnType("text");

                    b.Property<decimal>("UpdatedBy")
                        .HasColumnType("numeric(20,0)");

                    b.Property<DateTime>("UpdatedTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<decimal?>("Value")
                        .HasColumnType("numeric");

                    b.Property<decimal?>("Variance")
                        .HasColumnType("numeric");

                    b.HasKey("Id");

                    b.ToTable("ProductLinkDetails", "pivot_da_middleware_digital");
                });

            modelBuilder.Entity("DigitalTwin.Data.Entities.ProductLinkStatus", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Color")
                        .HasColumnType("text");

                    b.Property<decimal>("CreatedBy")
                        .HasColumnType("numeric(20,0)");

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("NorCode")
                        .HasColumnType("text");

                    b.Property<decimal>("UpdatedBy")
                        .HasColumnType("numeric(20,0)");

                    b.Property<DateTime>("UpdatedTime")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("ProductLinkStatuses", "pivot_da_middleware_digital");
                });

            modelBuilder.Entity("DigitalTwin.Data.Entities.UnitOfMeasure", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<decimal>("CreatedBy")
                        .HasColumnType("numeric(20,0)");

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<int>("UomId")
                        .HasColumnType("integer");

                    b.Property<decimal>("UpdatedBy")
                        .HasColumnType("numeric(20,0)");

                    b.Property<DateTime>("UpdatedTime")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("UnitOfMeasures", "pivot_da_middleware_digital");
                });

            modelBuilder.Entity("DigitalTwin.Data.Entities.ValueChainType", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<decimal>("CreatedBy")
                        .HasColumnType("numeric(20,0)");

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.Property<decimal>("UpdatedBy")
                        .HasColumnType("numeric(20,0)");

                    b.Property<DateTime>("UpdatedTime")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("ValueChainTypes", "pivot_da_middleware_digital");
                });

            modelBuilder.Entity("DigitalTwin.Data.Entities.Entity", b =>
                {
                    b.HasOne("DigitalTwin.Data.Entities.Entity", "EntityParent")
                        .WithMany()
                        .HasForeignKey("EntityParentId1");

                    b.HasOne("DigitalTwin.Data.Entities.EntityType", "EntityType")
                        .WithMany()
                        .HasForeignKey("EntityTypeId");

                    b.HasOne("DigitalTwin.Data.Entities.ValueChainType", "ValueChainType")
                        .WithMany()
                        .HasForeignKey("ValueChainTypeId");

                    b.Navigation("EntityParent");

                    b.Navigation("EntityType");

                    b.Navigation("ValueChainType");
                });

            modelBuilder.Entity("DigitalTwin.Data.Entities.ProductLink", b =>
                {
                    b.HasOne("DigitalTwin.Data.Entities.Entity", "Entity")
                        .WithMany()
                        .HasForeignKey("EntityId");

                    b.HasOne("DigitalTwin.Data.Entities.UnitOfMeasure", "UnitOfMeasure")
                        .WithMany()
                        .HasForeignKey("UnitOfMeasureId");

                    b.Navigation("Entity");

                    b.Navigation("UnitOfMeasure");
                });
#pragma warning restore 612, 618
        }
    }
}
