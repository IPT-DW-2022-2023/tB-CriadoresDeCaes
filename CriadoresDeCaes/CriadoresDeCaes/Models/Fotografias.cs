using System.ComponentModel.DataAnnotations.Schema;

namespace CriadoresDeCaes.Models {

   /// <summary>
   /// Fotografias associadas aos cães
   /// </summary>
   public class Fotografias {

      public int Id { get; set; }

      /// <summary>
      /// Nome do documento com a fotografia do cão/cadela
      /// </summary>
      public string NomeFicheiro { get; set; }

      /// <summary>
      /// data em que a fotografia foi tirada
      /// </summary>
      public DateTime Data { get; set; }

      /// <summary>
      /// local onde a fotografia foi tirada
      /// </summary>
      public string Local { get; set; }

      //**********************************************
    
      
      /// <summary>
      /// FK para identificar o Animal a quem a Fotografia pertence 
      /// </summary>
      [ForeignKey(nameof(Animal))]  // <=>  [ForeignKey("Animal")]
      public int AnimalFK { get; set; }
      public Animais Animal { get; set; }
      /*
       * o uso de [anotadores] permite alterar o comportamento
       * dos 'objetos' do nosso programa:
       *    - atributos
       *    - funções (métodos)
       *    - classes
       */



   }
}
