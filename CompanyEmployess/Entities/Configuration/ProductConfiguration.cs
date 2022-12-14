using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Configuration
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasData
            (
                new Product
                {
                    Id = new Guid("ad8bcc21-5cda-4dd0-9789-668a4512c8ea"),
                    Name = "Тарфиф Безлимит",
                    Price = 590,
                    Product_desciption = "Безлимитный интернет, 500 минут, 200 SMS",
                },

                new Product
                {
                    Id = new Guid("4d889d8a-3baf-428a-9f12-aa0a3e1a2368"),
                    Name = "Пётр Петров",
                    Price = 890,
                    Product_desciption = "Безлимитный интернет с раздачей, 650 минут, 200 SMS",
                }
            );
        }
    }
}
