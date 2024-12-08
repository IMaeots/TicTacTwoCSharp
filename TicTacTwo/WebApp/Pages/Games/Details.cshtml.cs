using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Data.Context;
using Data.Models.db;

namespace WebApp.Pages.Games
{
    public class DetailsModel : PageModel
    {
        private readonly Data.Context.GameDbContext _context;

        public DetailsModel(Data.Context.GameDbContext context)
        {
            _context = context;
        }

        public SaveGame SaveGame { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var savegame = await _context.SavedGames.FirstOrDefaultAsync(m => m.Id == id);

            if (savegame is not null)
            {
                SaveGame = savegame;

                return Page();
            }

            return NotFound();
        }
    }
}
