using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CriadoresDeCaes.Data;
using CriadoresDeCaes.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace CriadoresDeCaes.Controllers {

   [Authorize] // esta anotação irá forçar a
               // autenticação para acesso aos dados
   public class AnimaisController : Controller {

      /// <summary>
      /// este recurso identifica a Base de Dados do projeto
      /// </summary>
      private readonly ApplicationDbContext _bd;

      /// <summary>
      /// este recurso vai proporcionar acesso aos dados dos 
      /// recursos do servidor
      /// </summary>
      private readonly IWebHostEnvironment _webHostEnvironment;

      /// <summary>
      /// dados sobre a pessoa que está a interagir com a aplicação
      /// </summary>
      private readonly UserManager<IdentityUser> _userManager;

      public AnimaisController(
         ApplicationDbContext context,
         IWebHostEnvironment webHostEnvironment,
         UserManager<IdentityUser> userManager
         ) {
         _bd = context;
         _webHostEnvironment = webHostEnvironment;
         _userManager = userManager;
      }

      // GET: Animais
      public async Task<IActionResult> Index() {
         // var auxiliar
         string idDaPessoaAutenticada = _userManager.GetUserId(User);


         /* procurar, na base de dados, a lista dos animal existentes
          * SELECT *
          * FROM Animais a INNER JOIN Criadores c ON a.CriadorFK = c.Id
          *                INNER JOIN Racas r ON a.RacaFK = r.Id
          * WHERE c.UserId = Id da pessoa autenticada
          */
         var listaAnimais = _bd.Animais
                               .Include(a => a.Criador)
                               .Include(a => a.Raca)
                               .Where(a => a.Criador.UserId == idDaPessoaAutenticada)
                               ;

         // invoca a View, enviando uma lista de Animais
         return View(await listaAnimais.ToListAsync());
      }




      // GET: Animais/Details/5
      public async Task<IActionResult> Details(int? id) {

         if (id == null || _bd.Animais == null) {
            return NotFound();
         }

         /* procurar, na base de dados, o animal com ID igual ao parâmetro fornecido
          * SELECT *
          * FROM Animais a INNER JOIN Criadores c ON a.CriadorFK = c.Id
          *                INNER JOIN Racas r ON a.RacaFK = r.Id
          *                INNER JOIN Fotografias f ON f.AnimalFK = a.Id
          * WHERE a.Id = id 
          */
         var animal = await _bd.Animais
                                .Include(a => a.Criador)
                                .Include(a => a.Raca)
                                .Include(a => a.ListaFotografias)
                                .FirstOrDefaultAsync(m => m.Id == id);

         if (animal == null) {
            return NotFound();
         }

         return View(animal);
      }

      // GET: Animais/Create
      /// <summary>
      /// Criar as condições para a View de criação de um animal
      /// ser enviada para o browser
      /// </summary>
      /// <returns></returns>
      public IActionResult Create() {

         // prepara os dados a serem apresentados nas dropdown
         //   ViewData["CriadorFK"] = new SelectList(_bd.Criadores, "Id", "Nome");
         ViewData["RacaFK"] = new SelectList(_bd.Racas, "Id", "Nome");

         // invoca a View
         return View();
      }




      // POST: Animais/Create
      // To protect from overposting attacks, enable the specific properties you want to bind to.
      // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
      /// <summary>
      /// reage aos dados fornecidos pelo Browser
      /// </summary>
      /// <param name="animal">dados do novo animal</param>
      /// <returns></returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Create([Bind("Nome,DataNascimento,DataCompra,PrecoCompra,PrecoCompraAux,Sexo,NumLOP,RacaFK,CriadorFK")] Animais animal, IFormFile imagemAnimal) {
         /*
          * a anotação Bind serve para autorizar o controller a ler
          * o valor dos atributos que lhe chegam do browser.
          * Se não estiverem identificados, não são lidos
          */

         // vars. auxiliares
         string nomeFoto = "";
         bool existeFoto = false;

         // avaliar se temos condições para tentar adicionar o animal
         // testar se a Raça do animal != 0 e Criador =/= 0
         if (animal.RacaFK == 0) {
            // não foi escolhida uma raça
            ModelState.AddModelError("", "É obrigatório escolher uma Raça.");
         }
         else {
            // se cheguei aqui, escolhi Raça
            // será q escolhi Imagem? Vamos avaliar...

            if (imagemAnimal == null) {
               // o utilizador não fez upload de uma imagem
               // vamos adicionar uma imagem prédefinida ao animal
               animal.ListaFotografias
                     .Add(new Fotografias {
                        Data = DateTime.Now,
                        Local = "NoImage",
                        NomeFicheiro = "noAnimal.jpg"
                     });
            }
            else {
               // há ficheiro. Mas, será que é uma imagem?
               if (imagemAnimal.ContentType != "image/jpeg" &&
                   imagemAnimal.ContentType != "image/png") {
                  //  <=>  ! (imagemAnimal.ContentType == "image/jpeg" || imagemAnimal.ContentType == "image/png")
                  // o ficheiro carregado não é uma imagem
                  // o que fazer?
                  // Vamos fazer o mesmo que quando o utilizador não
                  // fornece uma imagem
                  animal.ListaFotografias
                        .Add(new Fotografias {
                           Data = DateTime.Now,
                           Local = "NoImage",
                           NomeFicheiro = "noAnimal.jpg"
                        });
               }
               else {
                  // há imagem!!!
                  // determinar o nome da imagem
                  Guid g = Guid.NewGuid();
                  nomeFoto = g.ToString();
                  // obter a extensão do ficheiro
                  string extensaoNomeFoto = Path.GetExtension(imagemAnimal.FileName).ToLower();
                  nomeFoto += extensaoNomeFoto;

                  // guardar os dados do ficheiro na BD
                  // para isso, vou associá-los ao 'animal'
                  animal.ListaFotografias
                        .Add(new Fotografias {
                           Data = DateTime.Now,
                           Local = "",
                           NomeFicheiro = nomeFoto
                        });

                  // informar a aplicação que há um ficheiro
                  // (imagem) para guardar no disco rígido
                  existeFoto = true;
               }
            } // if (imagemAnimal == null)
         } // if (animal.RacaFK == 0)


         // atribuir os dados do PrecoCompraAux ao PrecoCompra
         // se não for nulo
         if (!string.IsNullOrEmpty(animal.PrecoCompraAux)) {
            animal.PrecoCompra = Convert.ToDecimal(animal.PrecoCompraAux.Replace('.', ','));
         }

         // ********************************************************************
         // atribuir o ID do Criador 
         // ********************************************************************
         // var auxiliar
         string idDaPessoaAutenticada = _userManager.GetUserId(User);
         int idCriador = await _bd.Criadores
                                  .Where(c => c.UserId == idDaPessoaAutenticada)
                                  .Select(c => c.Id)
                                  .FirstOrDefaultAsync();
         animal.CriadorFK = idCriador;
         // ********************************************************************

         // se os dados recebidos respeitarem o modelo,
         // os dados podem ser adicionados
         if (ModelState.IsValid) {

            try {
               // adicionar os dados à BD
               _bd.Add(animal);
               // COMMIT da ação anterior
               await _bd.SaveChangesAsync();

               // se cheguei aqui, já foram guardados os dados
               // do animal na BD. Já posso guardar a imagem
               // no disco rígido do servidor
               if (existeFoto) {
                  // determinar onde guardar a imagem
                  string nomeLocalizacaoImagem = _webHostEnvironment.WebRootPath;
                  nomeLocalizacaoImagem = Path.Combine(nomeLocalizacaoImagem, "imagens");

                  // e, a pasta onde se pretende guardar a imagem existe?
                  if (!Directory.Exists(nomeLocalizacaoImagem)) {
                     Directory.CreateDirectory(nomeLocalizacaoImagem);
                  }

                  // informar o servidor do nome do ficheiro
                  string nomeDoFicheiro =
                     Path.Combine(nomeLocalizacaoImagem, nomeFoto);

                  // guardar o ficheiro
                  using var stream = new FileStream(nomeDoFicheiro, FileMode.Create);
                  await imagemAnimal.CopyToAsync(stream);
               }
               // devolver o controlo da app para a página de início
               return RedirectToAction(nameof(Index));
            }
            catch (Exception) {
               ModelState.AddModelError("",
                  "Ocorreu um erro com a adição dos dados do(a) " + animal.Nome);
               // throw;
            }

         }

         // preparar estes dados, para quando os dados a introduzir na BD não estão bons
         //        ViewData["CriadorFK"] = new SelectList(_bd.Criadores, "Id", "Nome", animal.CriadorFK);
         ViewData["RacaFK"] = new SelectList(_bd.Racas, "Id", "Nome", animal.RacaFK);

         // devolver à View os dados para correção
         return View(animal);
      }

      // GET: Animais/Edit/5
      public async Task<IActionResult> Edit(int? id) {
         if (id == null || _bd.Animais == null) {
            return NotFound();
         }

         // var auxiliar
         string idDaPessoaAutenticada = _userManager.GetUserId(User);

         var animal = await _bd.Animais
                               .Where(a => a.Id == id &&
                                           a.Criador.UserId == idDaPessoaAutenticada)
                               .FirstOrDefaultAsync();


         if (animal == null) {
            return NotFound();
         }

         ViewData["CriadorFK"] = new SelectList(_bd.Criadores, "Id", "Email", animal.CriadorFK);
         ViewData["RacaFK"] = new SelectList(_bd.Racas, "Id", "Id", animal.RacaFK);
         return View(animal);
      }

      // POST: Animais/Edit/5
      // To protect from overposting attacks, enable the specific properties you want to bind to.
      // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,DataNascimento,DataCompra,Sexo,NumLOP,RacaFK,CriadorFK")] Animais animais) {
         if (id != animais.Id) {
            return NotFound();
         }

         if (ModelState.IsValid) {
            try {
               _bd.Update(animais);
               await _bd.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) {
               if (!AnimaisExists(animais.Id)) {
                  return NotFound();
               }
               else {
                  throw;
               }
            }
            return RedirectToAction(nameof(Index));
         }
         ViewData["CriadorFK"] = new SelectList(_bd.Criadores, "Id", "Email", animais.CriadorFK);
         ViewData["RacaFK"] = new SelectList(_bd.Racas, "Id", "Id", animais.RacaFK);
         return View(animais);
      }

      // GET: Animais/Delete/5
      public async Task<IActionResult> Delete(int? id) {
         if (id == null || _bd.Animais == null) {
            return NotFound();
         }

         var animais = await _bd.Animais
             .Include(a => a.Criador)
             .Include(a => a.Raca)
             .FirstOrDefaultAsync(m => m.Id == id);
         if (animais == null) {
            return NotFound();
         }

         return View(animais);
      }

      // POST: Animais/Delete/5
      [HttpPost, ActionName("Delete")]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> DeleteConfirmed(int id) {
         if (_bd.Animais == null) {
            return Problem("Entity set 'ApplicationDbContext.Animais'  is null.");
         }
         var animais = await _bd.Animais.FindAsync(id);
         if (animais != null) {
            _bd.Animais.Remove(animais);
         }

         await _bd.SaveChangesAsync();
         return RedirectToAction(nameof(Index));
      }

      private bool AnimaisExists(int id) {
         return _bd.Animais.Any(e => e.Id == id);
      }
   }
}
