using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CriadoresDeCaes.Data;
using CriadoresDeCaes.Models;

namespace CriadoresDeCaes.Controllers {
   public class AnimaisController : Controller {
      private readonly ApplicationDbContext _bd;

      public AnimaisController(ApplicationDbContext context) {
         _bd=context;
      }

      // GET: Animais
      public async Task<IActionResult> Index() {
         /* procurar, na base de dados, a lista dos animal existentes
          * SELECT *
          * FROM Animais a INNER JOIN Criadores c ON a.CriadorFK = c.Id
          *                INNER JOIN Racas r ON a.RacaFK = r.Id
          */
         var listaAnimais = _bd.Animais.Include(a => a.Criador).Include(a => a.Raca);

         // invoca a View, enviando uma lista de Animais
         return View(await listaAnimais.ToListAsync());
      }




      // GET: Animais/Details/5
      public async Task<IActionResult> Details(int? id) {

         if (id==null||_bd.Animais==null) {
            return NotFound();
         }

         /* procurar, na base de dados, o animal com ID igual ao parâmetro fornecido
          * SELECT *
          * FROM Animais a INNER JOIN Criadores c ON a.CriadorFK = c.Id
          *                INNER JOIN Racas r ON a.RacaFK = r.Id
          * WHERE a.Id = id 
          */
         var animal = await _bd.Animais
                                .Include(a => a.Criador)
                                .Include(a => a.Raca)
                                .FirstOrDefaultAsync(m => m.Id==id);
         if (animal==null) {
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
         ViewData["CriadorFK"]=new SelectList(_bd.Criadores, "Id", "Nome");
         ViewData["RacaFK"]=new SelectList(_bd.Racas, "Id", "Nome");

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
      public async Task<IActionResult> Create([Bind("Id,Nome,DataNascimento,DataCompra,Sexo,NumLOP,RacaFK,CriadorFK")] Animais animal, IFormFile imagemAnimal) {

         // se os dados recebidos respeitarem o modelo,
         // os dados podem ser adicionados
         if (ModelState.IsValid) {

            try {
               // adicionar os dados à BD
               _bd.Add(animal);
               // COMMIT da ação anterior
               await _bd.SaveChangesAsync();
               // devolver o controlo da app para a página de início
               return RedirectToAction(nameof(Index));
            }
            catch (Exception) {
               ModelState.AddModelError("",
                  "Ocorreu um erro com a adição dos dados do(a) "+animal.Nome);
              // throw;
            }

         }

         // preparar estes dados, para quando os dados a introduzir na BD não estão bons
         ViewData["CriadorFK"]=new SelectList(_bd.Criadores, "Id", "Nome", animal.CriadorFK);
         ViewData["RacaFK"]=new SelectList(_bd.Racas, "Id", "Nome", animal.RacaFK);

         // devolver à View os dados para correção
         return View(animal);
      }

      // GET: Animais/Edit/5
      public async Task<IActionResult> Edit(int? id) {
         if (id==null||_bd.Animais==null) {
            return NotFound();
         }

         var animais = await _bd.Animais.FindAsync(id);
         if (animais==null) {
            return NotFound();
         }
         ViewData["CriadorFK"]=new SelectList(_bd.Criadores, "Id", "Email", animais.CriadorFK);
         ViewData["RacaFK"]=new SelectList(_bd.Racas, "Id", "Id", animais.RacaFK);
         return View(animais);
      }

      // POST: Animais/Edit/5
      // To protect from overposting attacks, enable the specific properties you want to bind to.
      // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,DataNascimento,DataCompra,Sexo,NumLOP,RacaFK,CriadorFK")] Animais animais) {
         if (id!=animais.Id) {
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
         ViewData["CriadorFK"]=new SelectList(_bd.Criadores, "Id", "Email", animais.CriadorFK);
         ViewData["RacaFK"]=new SelectList(_bd.Racas, "Id", "Id", animais.RacaFK);
         return View(animais);
      }

      // GET: Animais/Delete/5
      public async Task<IActionResult> Delete(int? id) {
         if (id==null||_bd.Animais==null) {
            return NotFound();
         }

         var animais = await _bd.Animais
             .Include(a => a.Criador)
             .Include(a => a.Raca)
             .FirstOrDefaultAsync(m => m.Id==id);
         if (animais==null) {
            return NotFound();
         }

         return View(animais);
      }

      // POST: Animais/Delete/5
      [HttpPost, ActionName("Delete")]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> DeleteConfirmed(int id) {
         if (_bd.Animais==null) {
            return Problem("Entity set 'ApplicationDbContext.Animais'  is null.");
         }
         var animais = await _bd.Animais.FindAsync(id);
         if (animais!=null) {
            _bd.Animais.Remove(animais);
         }

         await _bd.SaveChangesAsync();
         return RedirectToAction(nameof(Index));
      }

      private bool AnimaisExists(int id) {
         return _bd.Animais.Any(e => e.Id==id);
      }
   }
}
