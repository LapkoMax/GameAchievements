using Core.Models;
using Core.RequestFeatures.Extensions.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace Core.RequestFeatures.Extensions
{
    public static class RepositoryGenreExtensions
    { 
        public static IQueryable<Genre> Search(this IQueryable<Genre> genres,
            string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return genres;
            var lowerCaseTerm = searchTerm.Trim().ToLower();
            return genres.Where(g => g.Name.ToLower().Contains(lowerCaseTerm));
        }
        public static IQueryable<Genre> Sort(this IQueryable<Genre> genres,
            string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
                return genres.OrderBy(e => e.Name);
            var orderQuery = OrderQueryBuilder.CreateOrderQuery<Genre>(orderByQueryString);
            if (string.IsNullOrWhiteSpace(orderQuery))
                return genres.OrderBy(g => g.Name);
            return genres.OrderBy(orderQuery);
        }
    }
}
