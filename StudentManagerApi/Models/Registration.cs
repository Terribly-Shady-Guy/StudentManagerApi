﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace StudentManagerApi.Models;

public partial class Registration
{
    public int RegisterId { get; set; }

    public int StudentId { get; set; }

    public string CourseNumber { get; set; }

    public string AttendanceType { get; set; }

    public int Credits { get; set; }

    public string BookFormat { get; set; }

    public virtual Course CourseNumberNavigation { get; set; }

    public virtual Student Student { get; set; }
}