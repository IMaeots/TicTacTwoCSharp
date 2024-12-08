using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Data.Context;
using Data.Models.db;

namespace WebApp.Pages.Configurations
{
    public class DeleteModel : PageModel
    {
        private readonly Data.Context.GameDbContext _context;

        public DeleteModel(Data.Context.GameDbContext context)
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

            var savegameconfiguration = await _context.SavedGameConfigurations.FirstOrDefaultAsync(m => m.Id == id);

            if (savegameconfiguration is not null)
            {
                SaveGameConfiguration = savegameconfiguration;

                return Page();
            }

            return NotFound();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var savegameconfiguration = await _context.SavedGameConfigurations.FindAsync(id);
            if (savegameconfiguration != null)
            {
                SaveGameConfiguration = savegameconfiguration;
                _context.SavedGameConfigurations.Remove(SaveGameConfiguration);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
