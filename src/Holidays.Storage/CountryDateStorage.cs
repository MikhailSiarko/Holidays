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
    public class CountryDateStorage : IStorage<CountryDate>
    {
        private readonly StorageContext _storageContext;

        public CountryDateStorage(StorageContext storageContext)
        {
            _storageContext = storageContext;
        }

        public void Dispose()
        {
            _storageContext?.Dispose();
        }

        public async Task<bool> ExistsAsync(CountryDate model)
        {
            return await _storageContext
                .CountryDates
                .AnyAsync(x => x.CountryCode == model.Country && x.Date == model.Date);
        }

        public async Task<IEnumerable<CountryDate>> GetRangeAsync()
        {
            return await _storageContext
                .CountryDates
                .Select(x => new CountryDate
                {
                    Id = x.Id,
                    Country = x.CountryCode,
                    Date = x.Date,
                    DayStatus = x.Status
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<CountryDate>> GetRangeAsync(Expression<Func<CountryDate, bool>> predicate)
        {
            return await _storageContext
                .CountryDates
                .Select(x => new CountryDate
                {
                    Id = x.Id,
                    Country = x.CountryCode,
                    Date = x.Date,
                    DayStatus = x.Status
                })
                .Where(predicate)
                .ToListAsync();
        }

        public async Task<CountryDate> GetAsync(Expression<Func<CountryDate, bool>> predicate)
        {
            return await _storageContext
                .CountryDates
                .Select(x => new CountryDate
                {
                    Id = x.Id,
                    Country = x.CountryCode,
                    Date = x.Date,
                    DayStatus = x.Status
                })
                .SingleOrDefaultAsync(predicate);
        }

        public async Task SaveAsync(CountryDate model)
        {
            var entity = new CountryDateEntity
            {
                CountryCode = model.Country,
                Date = model.Date,
                Status = model.DayStatus
            };

            await _storageContext.CountryDates.AddAsync(entity);

            await _storageContext.SaveChangesAsync();

            model.Id = entity.Id;
        }

        public async Task SaveRangeAsync(IEnumerable<CountryDate> models)
        {
            var entities = models
                .Select(x => new 
                {
                    Entity = new CountryDateEntity
                    {
                        CountryCode = x.Country,
                        Date = x.Date,
                        Status = x.DayStatus
                    },
                    Model = x
                })
                .ToList();

            await _storageContext.CountryDates.AddRangeAsync(entities.Select(x => x.Entity));

            await _storageContext.SaveChangesAsync();

            foreach (var item in entities)
            {
                item.Model.Id = item.Entity.Id;
            }
        }
    }
}