using Kabanosi.Entities;
using Microsoft.EntityFrameworkCore;

namespace Kabanosi.Persistence;

public class DatabaseContext(DbContextOptions<DatabaseContext> options) : DbContext(options)
{
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<Comment> Comments { get; set; }
    public virtual DbSet<Invitation> Invitations { get; set; }
    public virtual DbSet<Project> Projects { get; set; }
    public virtual DbSet<ProjectMember> ProjectMembers { get; set; }
    public virtual DbSet<ProjectMemberAssignment> ProjectMemberAssignments { get; set; }
    public virtual DbSet<Assignment> Assignments { get; set; }
    public virtual DbSet<AssignmentLabel> AssignmentLabels { get; set; }
    public virtual DbSet<AssignmentStatus> AssignmentStatuses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Assignment>()
            .HasOne(a => a.AssignmentLabel)
            .WithMany(l => l.Assignments)
            .HasForeignKey(a => a.AssignmentLabelId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}