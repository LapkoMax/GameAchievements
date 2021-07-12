using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using Core.RequestFeatures.Extensions.Utility;

namespace Core.RequestFeatures.Extensions
{
    public static class RepositoryGameExtensions
    {
        public static IQueryable<Game> FilterGames(this IQueryable<Game> games,
            double minRating, double maxRating) =>
            games.Where(g => g.Rating >= minRating && g.Rating <= maxRating);
        public static IQueryable<Game> Search(this IQueryable<Game> games,
            string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return games;
            var lowerCaseTerm = searchTerm.Trim().ToLower();
            return games.Where(g => g.Name.ToLower().Contains(lowerCaseTerm));
        }
        public static IQueryable<Game> Sort(this IQueryable<Game> games,
            string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
                return games.OrderBy(e => e.Name);
            var orderQuery = OrderQueryBuilder.CreateOrderQuery<Game>(orderByQueryString);
            if (string.IsNullOrWhiteSpace(orderQuery))
                return games.OrderBy(g => g.Name);
            return games.OrderBy(orderQuery);
        }
    }
}
