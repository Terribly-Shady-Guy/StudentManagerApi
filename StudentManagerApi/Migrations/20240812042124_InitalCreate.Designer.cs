﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using StudentManagerApi.Models;

#nullable disable

namespace StudentManagerApi.Migrations
{
    [DbContext(typeof(StudentManagerDbContext))]
    [Migration("20240812042124_InitalCreate")]
    partial class InitalCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("StudentManagerApi.Models.Course", b =>
                {
                    b.Property<string>("CourseNumber")
                        .HasMaxLength(9)
                        .IsUnicode(false)
                        .HasColumnType("char(9)")
                        .IsFixedLength();

                    b.Property<string>("CourseName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("date");

                    b.Property<string>("Instructor")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("date");

                    b.HasKey("CourseNumber")
                        .HasName("PK__tmp_ms_x__A98290ECBE263371");

                    b.ToTable("Courses");
                });

            modelBuilder.Entity("StudentManagerApi.Models.Registration", b =>
                {
                    b.Property<int>("RegisterId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RegisterId"));

                    b.Property<string>("AttendanceType")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("BookFormat")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("CourseNumber")
                        .IsRequired()
                        .HasMaxLength(9)
                        .IsUnicode(false)
                        .HasColumnType("char(9)")
                        .IsFixedLength();

                    b.Property<int>("Credits")
                        .HasColumnType("int");

                    b.Property<int>("StudentId")
                        .HasColumnType("int");

                    b.HasKey("RegisterId")
                        .HasName("PK__Registra__B91FAB7941B2E08F");

                    b.HasIndex("CourseNumber");

                    b.HasIndex("StudentId");

                    b.ToTable("Registration", (string)null);
                });

            modelBuilder.Entity("StudentManagerApi.Models.Student", b =>
                {
                    b.Property<int>("StudentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("StudentId"));

                    b.Property<DateTime>("ExpectedGradDate")
                        .HasColumnType("date");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<decimal>("Gpa")
                        .HasColumnType("decimal(4, 2)")
                        .HasColumnName("GPA");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Major")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.HasKey("StudentId")
                        .HasName("PK__Students__32C52B9950577A5A");

                    b.ToTable("Students");
                });

            modelBuilder.Entity("StudentManagerApi.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserId"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.HasKey("UserId")
                        .HasName("PK__Users__1788CC4C00445930");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("StudentManagerApi.Models.Registration", b =>
                {
                    b.HasOne("StudentManagerApi.Models.Course", "CourseNumberNavigation")
                        .WithMany("Registrations")
                        .HasForeignKey("CourseNumber")
                        .IsRequired()
                        .HasConstraintName("FK_Registration_ToCourses");

                    b.HasOne("StudentManagerApi.Models.Student", "Student")
                        .WithMany("Registrations")
                        .HasForeignKey("StudentId")
                        .IsRequired()
                        .HasConstraintName("FK_Registration_ToStudents");

                    b.Navigation("CourseNumberNavigation");

                    b.Navigation("Student");
                });

            modelBuilder.Entity("StudentManagerApi.Models.Course", b =>
                {
                    b.Navigation("Registrations");
                });

            modelBuilder.Entity("StudentManagerApi.Models.Student", b =>
                {
                    b.Navigation("Registrations");
                });
#pragma warning restore 612, 618
        }
    }
}