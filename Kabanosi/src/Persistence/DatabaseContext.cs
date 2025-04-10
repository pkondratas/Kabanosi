﻿using Kabanosi.Entities;
using Microsoft.EntityFrameworkCore;

namespace Kabanosi.Persistence;

public class DatabaseContext(DbContextOptions<DatabaseContext> options) : DbContext(options)
{
    public virtual DbSet<Comment> Comments { get; set; }
    public virtual DbSet<Invitation> Invitations { get; set; }
    public virtual DbSet<Project> Projects { get; set; }
    public virtual DbSet<ProjectMember> ProjectMembers { get; set; }
    public virtual DbSet<ProjectMemberAssignment> ProjectMemberAssignments { get; set; }
    public virtual DbSet<Assignment> Assignments { get; set; }
    public virtual DbSet<AssignmentLabel> AssignmentLabels { get; set; }
    public virtual DbSet<AssignmentStatus> AssignmentStatuses { get; set; }
}