﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ForTh3Win.Data;
using ForTh3Win.Models;
using System.Data;
using Microsoft.Data.SqlClient.DataClassification;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ForTh3Win.Pages.James
{
    public class IndexModel : PageModel
    {
        private readonly ForTh3Win.Data.ForTh3WinContext _context;

        public IndexModel(ForTh3Win.Data.ForTh3WinContext context)
        {
            _context = context;
        }

        public string SearchSort { get; set; }
        public ESRBEnum ESRBSort { get; set; }
        public GenreEnum GenreSort { get; set; }
        public string? SortOrder { get; set; }

        public IList<Review> Review { get;set; } = default!;

        public async Task OnGetAsync(string searchString, string esrbSearch, string genreSearch, string sortOrder)
        {
            
            IQueryable<Review> reviewList = from r in _context.Review select r;
            if (!string.IsNullOrEmpty(searchString))
            {
                SearchSort = searchString;
                reviewList = reviewList.Where(r => r.GameName.Contains(SearchSort));
            }
            if (!string.IsNullOrEmpty(esrbSearch))
            {
                ESRBSort = (ESRBEnum)Enum.Parse(typeof(ESRBEnum), esrbSearch);
                reviewList = reviewList.Where(r => r.ESRBRating == ESRBSort);
            }
            if (!string.IsNullOrEmpty(genreSearch))
            {
                GenreSort = (GenreEnum)Enum.Parse(typeof(GenreEnum), genreSearch);
                reviewList = reviewList.Where(r => r.Genre == GenreSort);
            }

            if (!string.IsNullOrEmpty(sortOrder))
            {
                SortOrder = sortOrder;
                if (SortOrder == "asc")
                {
                    reviewList = reviewList.OrderByDescending(x => x.GameName).Reverse();
                }
                else if (SortOrder == "desc")
                {
                    reviewList = reviewList.OrderByDescending(x => x.GameName);
                }
            }

            if (_context.Review != null)
            {
                Review = await reviewList.AsNoTracking().ToListAsync();
            }
        }
    } 
}
