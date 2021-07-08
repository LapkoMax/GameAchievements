using Entities.Models;
using DataAccess.RequestFeatures.Extensions.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace DataAccess.RequestFeatures.Extensions
{
    public static class RepositoryAchievementExtensions
    {
        public static IQueryable<Achievement> Search(this IQueryable<Achievement> achievements,
            string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return achievements;
            var lowerCaseTerm = searchTerm.Trim().ToLower();
            return achievements.Where(g => g.Name.ToLower().Contains(lowerCaseTerm));
        }
        public static IQueryable<Achievement> Sort(this IQueryable<Achievement> achievements,
            string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
                return achievements.OrderBy(e => e.Name);
            var orderQuery = OrderQueryBuilder.CreateOrderQuery<Game>(orderByQueryString);
            if (string.IsNullOrWhiteSpace(orderQuery))
                return achievements.OrderBy(g => g.Name);
            return achievements.OrderBy(orderQuery);
        }
    }
}
