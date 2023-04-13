using CriadoresDeCaes.Models;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CriadoresDeCaes.Data {

   /// <summary>
   /// esta classe representa a Base de Dados do nosso projeto
   /// </summary>
   public class ApplicationDbContext : IdentityDbContext {

      public ApplicationDbContext(
         DbContextOptions<ApplicationDbContext> options)
          : base(options) {
      }

      /* *********************************************
       * Criação das Tabelas
       * ********************************************* */
      public DbSet<Animais> Animais { get; set; }
      public DbSet<Racas> Racas { get; set; }
      public DbSet<Criadores> Criadores { get; set; }
      public DbSet<Fotografias> Fotografias { get; set; }

   }
}