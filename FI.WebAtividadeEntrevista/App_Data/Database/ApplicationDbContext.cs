using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using WebAtividadeEntrevista.Models;

namespace WebAtividadeEntrevista.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
            : base("BancoDeDados") 
        {
        }

        public DbSet<ClienteModel> Clientes { get; set; }
        public DbSet<BeneficiarioModel> Beneficiarios { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Map Cliente
            modelBuilder.Configurations.Add(new ClienteMap());
            modelBuilder.Configurations.Add(new BeneficiarioMap());

            base.OnModelCreating(modelBuilder);
        }
    }

    public class ClienteMap : EntityTypeConfiguration<ClienteModel>
    {
        public ClienteMap()
        {
            ToTable("Clientes");

            HasKey(c => c.Id);

            Property(c => c.CEP).IsRequired().HasMaxLength(10);
            Property(c => c.Cidade).IsRequired().HasMaxLength(100);
            Property(c => c.Email).HasMaxLength(150);
            Property(c => c.Estado).IsRequired().HasMaxLength(2);
            Property(c => c.Logradouro).IsRequired().HasMaxLength(150);
            Property(c => c.Nacionalidade).IsRequired().HasMaxLength(100);
            Property(c => c.Nome).IsRequired().HasMaxLength(100);
            Property(c => c.Cpf).IsRequired().HasMaxLength(14);
            Property(c => c.Sobrenome).IsRequired().HasMaxLength(100);
            Property(c => c.Telefone).HasMaxLength(20);

            // Ignorar propriedades não persistidas
            Ignore(c => c.BeneficiariosJson);
            Ignore(c => c.Beneficiarios);
        }
    }

    public class BeneficiarioMap : EntityTypeConfiguration<BeneficiarioModel>
    {
        public BeneficiarioMap()
        {
            ToTable("Beneficiarios");

            HasKey(b => b.Id);

            Property(b => b.Cpf).IsRequired().HasMaxLength(14);
            Property(b => b.Nome).HasMaxLength(100);

            // Relacionamento opcional com Cliente
            HasOptional<ClienteModel>(b => b.Cliente)
                .WithMany()
                .HasForeignKey(b => b.ClienteId);
        }
    }
}
