﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using P01_StudentSystem.Data;

#nullable disable

namespace P01_StudentSystem.Migrations
{
    [DbContext(typeof(StudentSystemContext))]
    [Migration("20240624221512_EnumFix")]
    partial class EnumFix
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("P01_StudentSystem.Data.Models.Course", b =>
                {
                    b.Property<int>("CourseId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CourseId"), 1L, 1);

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(80)");

                    b.Property<decimal>("Price")
                        .HasColumnType("DECIMAL (18,2)");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.HasKey("CourseId");

                    b.ToTable("Courses");
                });

            modelBuilder.Entity("P01_StudentSystem.Data.Models.Homework", b =>
                {
                    b.Property<int>("HomeworkId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("HomeworkId"), 1L, 1);

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("varchar");

                    b.Property<int>("ContentType")
                        .HasColumnType("int");

                    b.Property<int>("CourseId")
                        .HasColumnType("int");

                    b.Property<int>("StudentId")
                        .HasColumnType("int");

                    b.Property<DateTime>("SubmissionTime")
                        .HasColumnType("datetime2");

                    b.HasKey("HomeworkId");

                    b.HasIndex("CourseId");

                    b.HasIndex("StudentId");

                    b.ToTable("Homeworks");
                });

            modelBuilder.Entity("P01_StudentSystem.Data.Models.Resource", b =>
                {
                    b.Property<int>("ResourceId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ResourceId"), 1L, 1);

                    b.Property<int>("CourseId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("ResourceType")
                        .HasColumnType("int");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("varchar");

                    b.HasKey("ResourceId");

                    b.HasIndex("CourseId");

                    b.ToTable("Resources");
                });

            modelBuilder.Entity("P01_StudentSystem.Data.Models.Student", b =>
                {
                    b.Property<int>("StudentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("StudentId"), 1L, 1);

                    b.Property<DateTime?>("Birthday")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("char(10)");

                    b.Property<DateTime>("RegisteredOn")
                        .HasColumnType("datetime2");

                    b.HasKey("StudentId");

                    b.ToTable("Students");
                });

            modelBuilder.Entity("P01_StudentSystem.Data.Models.StudentCourse", b =>
                {
                    b.Property<int>("StudentId")
                        .HasColumnType("int");

                    b.Property<int>("CourseId")
                        .HasColumnType("int");

                    b.HasKey("StudentId", "CourseId");

                    b.HasIndex("CourseId");

                    b.ToTable("StudentsCourses");
                });

            modelBuilder.Entity("P01_StudentSystem.Data.Models.Homework", b =>
                {
                    b.HasOne("P01_StudentSystem.Data.Models.Course", "Course")
                        .WithMany("Homeworks")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("P01_StudentSystem.Data.Models.Student", "Student")
                        .WithMany("Homeworks")
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Course");

                    b.Navigation("Student");
                });

            modelBuilder.Entity("P01_StudentSystem.Data.Models.Resource", b =>
                {
                    b.HasOne("P01_StudentSystem.Data.Models.Course", "Course")
                        .WithMany("Resources")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Course");
                });

            modelBuilder.Entity("P01_StudentSystem.Data.Models.StudentCourse", b =>
                {
                    b.HasOne("P01_StudentSystem.Data.Models.Course", "Course")
                        .WithMany("StudentsCourses")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("P01_StudentSystem.Data.Models.Student", "Student")
                        .WithMany("StudentsCourses")
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Course");

                    b.Navigation("Student");
                });

            modelBuilder.Entity("P01_StudentSystem.Data.Models.Course", b =>
                {
                    b.Navigation("Homeworks");

                    b.Navigation("Resources");

                    b.Navigation("StudentsCourses");
                });

            modelBuilder.Entity("P01_StudentSystem.Data.Models.Student", b =>
                {
                    b.Navigation("Homeworks");

                    b.Navigation("StudentsCourses");
                });
#pragma warning restore 612, 618
        }
    }
}
