using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Configuration
{
    public class ClientConfiguration : IEntityTypeConfiguration<Client>
    {
        public void Configure(EntityTypeBuilder<Client> builder)
        {
            builder.HasData
            (
                new Client
                {
                    Id = new Guid("1e4940c3-15d8-4d89-9e30-7ffdd739514b"),
                    Name = "Иван Иванов",
                    Number_Client = "79542156475",
                    Address = "Саранск,ул. Гожувская улица, 10",
                },

                new Client
                {
                    Id = new Guid("9ef303aa-1e51-43e7-8c4c-8501ba1e26ea"),
                    Name = "Пётр Петров",
                    Number_Client = "79378589546",
                    Address = "Саранск, ул. Богдана Хмельницкого, 28",
                }
            );
        }
    }
}
