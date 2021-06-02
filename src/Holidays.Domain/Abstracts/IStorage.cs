using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Holidays.Domain.Abstracts
{
    public interface IStorage<TE>
    {
        Task<bool> ExistsAsync(TE model);

        Task<IEnumerable<TE>> GetRangeAsync();

        Task<IEnumerable<TE>> GetRangeAsync(Expression<Func<TE, bool>> predicate);

        Task<TE> GetAsync(Expression<Func<TE, bool>> predicate);

        Task SaveAsync(TE model);

        Task SaveRangeAsync(IEnumerable<TE> models);
    }
}