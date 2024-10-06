using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using SistemaDeTarefas.Models;

namespace SistemaDeTarefas.Data.Map
{
    public class LoginMap : IEntityTypeConfiguration<LoginModel>
    {
        public void Configure(EntityTypeBuilder<LoginModel> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Login).IsRequired().HasMaxLength(150);
            builder.HasIndex(x => x.Login).IsUnique();
            builder.Property(x => x.Senha).IsRequired().HasMaxLength(255);
        }
    }
}
