namespace CriadoresDeCaes.Models {

   /// <summary>
   /// descrição dos Animais
   /// </summary>
   public class Animais {

      public int Id { get; set; }

      /// <summary>
      /// nome do cão/cadela
      /// </summary>
      public string Nome { get; set; }

      /// <summary>
      /// data de nascimento
      /// </summary>
      public DateTime DataNascimento { get; set; }

      /// <summary>
      /// Data em que o cão foi comprado
      /// </summary>
      public DateTime DataCompra { get; set; }

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

   }
}
