using Demo.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DAL.Data.Configurations
{
    internal class DepartmentConfigurations : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            // Fluent APIs
            builder.Property(D=>D.Id).UseIdentityColumn(10,10);

            builder.HasMany(D=>D.Employees)
                   .WithOne(E=>E.Department)
                   .HasForeignKey(E=>E.DepartmentId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.Property(D=>D.Code)
                   .IsRequired(true);

            builder.Property(D => D.Name)
                   .IsRequired(true);

        }
    }
}
