using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Holidays.Domain.Abstracts;
using Holidays.Domain.Models;
using Holidays.Storage.Entities;
using Microsoft.EntityFrameworkCore;

namespace Holidays.Storage
{
    public class CountryStorage : IStorage<Country>
    {
        private readonly StorageContext _storageContext;

        public CountryStorage(StorageContext storageContext)
        {
            _storageContext = storageContext;
        }

        public async Task<bool> ExistsAsync(Country model)
        {
            return await _storageContext.Countries.AnyAsync(x => x.Code == model.CountryCode);
        }

        public async Task<IEnumerable<Country>> GetRangeAsync()
        {
            return await _storageContext
                .Countries
                .Select(x => new Country
                {
                    CountryCode = x.Code,
                    FullName = x.FullName
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<Country>> GetRangeAsync(Expression<Func<Country, bool>> predicate)
        {
            return await _storageContext
                .Countries
                .Select(x => new Country
                {
                    CountryCode = x.Code,
                    FullName = x.FullName
                })
                .Where(predicate)
                .ToListAsync();
        }

        public async Task<Country> GetAsync(Expression<Func<Country, bool>> predicate)
        {
            return await _storageContext
                .Countries
                .Select(x => new Country
                {
                    CountryCode = x.Code,
                    FullName = x.FullName
                })
                .SingleOrDefaultAsync(predicate);
        }

        public async Task SaveAsync(Country model)
        {
            var entity = new CountryEntity
            {
                Code = model.CountryCode,
                FullName = model.FullName
            };

            await _storageContext.Countries.AddAsync(entity);

            await _storageContext.SaveChangesAsync();
        }

        public async Task SaveRangeAsync(IEnumerable<Country> models)
        {
            var entities = models
                .Select(x => new CountryEntity
                {
                    Code = x.CountryCode,
                    FullName = x.FullName
                })
                .ToList();

            await _storageContext.AddRangeAsync(entities);
            
            await _storageContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            _storageContext?.Dispose();
        }
    }
}