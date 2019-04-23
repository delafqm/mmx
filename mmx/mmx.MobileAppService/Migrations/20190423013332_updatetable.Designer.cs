﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using mmx.MobileAppService.Models;

namespace mmx.MobileAppService.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20190423013332_updatetable")]
    partial class updatetable
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062");

            modelBuilder.Entity("mmx.MobileAppService.Models.TestItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Desc");

                    b.Property<string>("Name");

                    b.Property<DateTime>("Uptime");

                    b.HasKey("Id");

                    b.ToTable("TestItems");
                });
#pragma warning restore 612, 618
        }
    }
}
