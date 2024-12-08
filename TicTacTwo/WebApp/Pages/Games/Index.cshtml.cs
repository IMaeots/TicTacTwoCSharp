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
    public class IndexModel : PageModel
    {
        private readonly Data.Context.GameDbContext _context;

        public IndexModel(Data.Context.GameDbContext context)
        {
            _context = context;
        }

        public IList<SaveGame> SaveGame { get;set; } = default!;

        public async Task OnGetAsync()
        {
            SaveGame = await _context.SavedGames
                .Include(s => s.Configuration).ToListAsync();
        }
    }
}
