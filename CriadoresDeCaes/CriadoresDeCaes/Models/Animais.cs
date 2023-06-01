using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CriadoresDeCaes.Models {

   /// <summary>
   /// descrição dos Animais
   /// </summary>
   public class Animais {

      public Animais() {
         ListaFotografias = new HashSet<Fotografias>();
      }

      public int Id { get; set; }

      /// <summary>
      /// nome do cão/cadela
      /// </summary>
      public string Nome { get; set; }

      /// <summary>
      /// data de nascimento
      /// </summary>
      [Required(ErrorMessage = "A {0} é de preenchimento obrigatório")]
      public DateTime DataNascimento { get; set; }

      /// <summary>
      /// Data em que o cão foi comprado
      /// </summary>
      public DateTime? DataCompra { get; set; }
      // o uso do ? transforma o atributo, tornando-o facultativo
      // se já se tiver transferido o Modelo para a BD
      // é preciso atualizar a BD com uma nova Migração


      /// <summary>
      /// Sexo do cão
      /// F - Fêmea
      /// M - Macho
      /// </summary>
      public string Sexo { get; set; }

      /// <summary>
      /// número de registo no LOP
      /// </summary>
      public string NumLOP { get; set; }

      // *************************************

      /// <summary>
      /// lista das fotografias associadas a um animal
      /// </summary>
      public ICollection<Fotografias> ListaFotografias { get; set; }

      /// <summary>
      /// FK para a Raça do cão/cadela
      /// </summary>
      [ForeignKey(nameof(Raca))]
      [Display(Name = "Raça")]
      public int RacaFK { get; set; }
      public Racas Raca { get; set; }

      /// <summary>
      /// FK para o Criador do cão/cadela
      /// </summary>
      [ForeignKey(nameof(Criador))]
      [Display(Name = "Criador")]
      public int CriadorFK { get; set; }
      public Criadores Criador { get; set; }


   }
}
