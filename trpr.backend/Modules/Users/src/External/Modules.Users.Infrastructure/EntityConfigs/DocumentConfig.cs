using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Users.Domain.Entities;

namespace Modules.Users.Infrastructure.EntityConfigs
{
    public class DocumentConfig : IEntityTypeConfiguration<Document>
    {
        public void Configure(EntityTypeBuilder<Document> builder) 
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.GuideRequestId)
                .IsRequired();

            builder.Property(x => x.Type)
                .IsRequired();

            builder.Property(x => x.FileUrl)
                .IsRequired();

            
            builder.HasIndex(x => x.GuideRequestId);
            
        }
    }
}
