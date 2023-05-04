using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CriadoresDeCaes.Models {

   /// <summary>
   /// descrição dos Criadores dos cães
   /// </summary>
   public class Criadores {

      /*
       * Lista de anotadores da classe e outros recursos
       * https://learn.microsoft.com/en-us/ef/ef6/modeling/code-first/data-annotations
       * https://www.entityframeworktutorial.net/code-first/dataannotation-in-code-first.aspx
       * 
       * https://pt.wikipedia.org/wiki/Express%C3%A3o_regular
       * https://learn.microsoft.com/pt-br/dotnet/standard/base-types/regular-expression-language-quick-reference
       * https://developer.mozilla.org/pt-BR/docs/Web/JavaScript/Guide/Regular_expressions
       */
      public Criadores() {
         ListaAnimais=new HashSet<Animais>();
         ListaRacas=new HashSet<Racas>();
      }

      public int Id { get; set; }

      /// <summary>
      /// Nome do criador
      /// </summary>
      [Required(ErrorMessage = "O {0} é de preenchimento obrigatório")]
      [StringLength(50)]
      public string Nome { get; set; }

      /// <summary>
      /// nome comercial do criador de cães
      /// </summary>
      [Display(Name = "Nome Criador")]
      [StringLength(50)]
      public string NomeCriador { get; set; }

      /// <summary>
      /// morada
      /// </summary>
      [StringLength(100)]
      public string Morada { get; set; }

      /// <summary>
      /// Código Postal
      /// </summary>
      [DisplayName("Código Postal")]
      [RegularExpression("[1-9][0-9]{3}-[0-9]{3} [A-ZÇÁÉÍÓÚÊÂÎÔÛÀÃÕ ]+",
                         ErrorMessage = "O {0} tem de ser da forma XXXX-XXX NOME DA TERRA")]
      [StringLength(25)]
      public string CodPostal { get; set; }

      /// <summary>
      /// email do criador
      /// </summary>
      [EmailAddress(ErrorMessage = "O {0} não está corretamente escrito")]
      [Required(ErrorMessage = "O {0} é de preenchimento obrigatório")]
      [RegularExpression("[a-z._0-9]+@gmail.com", ErrorMessage = "O {0} tem de ser do GMail")]
      [StringLength(40)]
      public string Email { get; set; }

      /// <summary>
      /// telemóvel do criador
      /// </summary>
      [Required(ErrorMessage = "O {0} é de preenchimento obrigatório")]
      [Display(Name = "Telemóvel")]
      [StringLength(9, MinimumLength = 9,
                    ErrorMessage = "O {0} deve ter {1} dígitos")]
      [RegularExpression("9[1236][0-9]{7}",
                    ErrorMessage = "O número de {0} deve começar por 91, 92, 93 ou 96, e ter 9 dígitos")]
      //            ((+|00)[0-9]{2,5})?[0-9]{5,9}
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
