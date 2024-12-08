using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Data.Context;
using Data.Models.db;

namespace WebApp.Pages.Games
{
    public class CreateModel : PageModel
    {
        private readonly Data.Context.GameDbContext _context;

        public CreateModel(Data.Context.GameDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["ConfigurationId"] = new SelectList(_context.SavedGameConfigurations, "Id", "JsonConfiguration");
            return Page();
        }

        [BindProperty]
        public SaveGame SaveGame { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.SavedGames.Add(SaveGame);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
