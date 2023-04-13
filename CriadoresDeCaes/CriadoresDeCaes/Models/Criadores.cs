namespace CriadoresDeCaes.Models {

   /// <summary>
   /// descrição dos Criadores dos cães
   /// </summary>
   public class Criadores {

      public Criadores() {
         ListaAnimais=new HashSet<Animais>();
         ListaRacas=new HashSet<Racas>();
      }

      public int Id { get; set; }

      /// <summary>
      /// Nome do criador
      /// </summary>
      public string Nome { get; set; }

      /// <summary>
      /// nome comercial do criador de cães
      /// </summary>
      public string NomeCriador { get; set; }

      /// <summary>
      /// morada
      /// </summary>
      public string Morada { get; set; }

      /// <summary>
      /// Código Postal
      /// </summary>
      public string CodPostal { get; set; }

      /// <summary>
      /// email do criador
      /// </summary>
      public string Email { get; set; }

      /// <summary>
      /// telemóvel do criador
      /// </summary>
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
