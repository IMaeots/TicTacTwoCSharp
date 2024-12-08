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

namespace WebApp.Pages.Configurations
{
    public class EditModel : PageModel
    {
        private readonly Data.Context.GameDbContext _context;

        public EditModel(Data.Context.GameDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public SaveGameConfiguration SaveGameConfiguration { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var savegameconfiguration =  await _context.SavedGameConfigurations.FirstOrDefaultAsync(m => m.Id == id);
            if (savegameconfiguration == null)
            {
                return NotFound();
            }
            SaveGameConfiguration = savegameconfiguration;
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

            _context.Attach(SaveGameConfiguration).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SaveGameConfigurationExists(SaveGameConfiguration.Id))
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

        private bool SaveGameConfigurationExists(int id)
        {
            return _context.SavedGameConfigurations.Any(e => e.Id == id);
        }
    }
}
