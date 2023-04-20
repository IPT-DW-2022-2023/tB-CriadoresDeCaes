using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CriadoresDeCaes.Models {

   /// <summary>
   /// descrição dos Criadores dos cães
   /// </summary>
   public class Criadores {

      /*
       * Lista de anotadores da classe 
       * https://learn.microsoft.com/en-us/ef/ef6/modeling/code-first/data-annotations
       * https://www.entityframeworktutorial.net/code-first/dataannotation-in-code-first.aspx
       */
      public Criadores() {
         ListaAnimais=new HashSet<Animais>();
         ListaRacas=new HashSet<Racas>();
      }

      public int Id { get; set; }

      /// <summary>
      /// Nome do criador
      /// </summary>
      [Required(ErrorMessage ="O {0} é de preenchimento obrigatório")]
      public string Nome { get; set; }

      /// <summary>
      /// nome comercial do criador de cães
      /// </summary>
      [Display(Name ="Nome Criador")]
      public string NomeCriador { get; set; }

      /// <summary>
      /// morada
      /// </summary>
      public string Morada { get; set; }

      /// <summary>
      /// Código Postal
      /// </summary>
      [DisplayName("Código Postal")]
      public string CodPostal { get; set; }

      /// <summary>
      /// email do criador
      /// </summary>
      [EmailAddress(ErrorMessage = "O {0} não está corretamente escrito")]
      [Required(ErrorMessage = "O {0} é de preenchimento obrigatório")]
      public string Email { get; set; }

      /// <summary>
      /// telemóvel do criador
      /// </summary>
      [Required(ErrorMessage = "O {0} é de preenchimento obrigatório")]
      [Display(Name ="Telemóvel")]
      public string Telemovel { get; set; }

      /// <summary>
      /// FK para a lista de cães/cadelas, propriedade do Criador
      /// </summary>
      public ICollection<Animais> ListaAnimais { get; set; }

      /// <summary>
      /// M-N
      /// FK para a lista de Raças atribuídas aos Criadores
      /// </summary>
      public ICollection<Racas> ListaRacas { get; set; }
   }
}
