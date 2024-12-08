using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Data.Context;
using Data.Models.db;

namespace WebApp.Pages.Games
{
    public class EditModel : PageModel
    {
        private readonly Data.Context.GameDbContext _context;

        public EditModel(Data.Context.GameDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public SaveGame SaveGame { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var savegame =  await _context.SavedGames.FirstOrDefaultAsync(m => m.Id == id);
            if (savegame == null)
            {
                return NotFound();
            }
            SaveGame = savegame;
           ViewData["ConfigurationId"] = new SelectList(_context.SavedGameConfigurations, "Id", "JsonConfiguration");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(SaveGame).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SaveGameExists(SaveGame.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool SaveGameExists(int id)
        {
            return _context.SavedGames.Any(e => e.Id == id);
        }
    }
}
