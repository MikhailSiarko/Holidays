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
    public class CountryHolidaysStorage : IStorage<CountryHoliday>
    {
        private readonly StorageContext _storageContext;

        public CountryHolidaysStorage(StorageContext storageContext)
        {
            _storageContext = storageContext;
        }

        public async Task<bool> ExistsAsync(CountryHoliday model)
        {
            return await _storageContext
                .CountryHolidays
                .AnyAsync(x =>
                    x.CountryDate.CountryCode == model.CountryCode && x.CountryDate.Date == model.Date);
        }

        public async Task<IEnumerable<CountryHoliday>> GetRangeAsync()
        {
            return await _storageContext
                .CountryHolidays
                .Select(x => new CountryHoliday
                {
                    Id = x.Id,
                    CountryDateId = x.CountryDateId,
                    CountryCode = x.CountryDate.CountryCode,
                    Date = x.CountryDate.Date,
                    Name = x.Name
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<CountryHoliday>> GetRangeAsync(Expression<Func<CountryHoliday, bool>> predicate)
        {
            return await _storageContext
                .CountryHolidays
                .Select(x => new CountryHoliday
                {
                    Id = x.Id,
                    CountryDateId = x.CountryDateId,
                    CountryCode = x.CountryDate.CountryCode,
                    Date = x.CountryDate.Date,
                    Name = x.Name
                })
                .Where(predicate)
                .ToListAsync();
        }

        public async Task<CountryHoliday> GetAsync(Expression<Func<CountryHoliday, bool>> predicate)
        {
            return await _storageContext
                .CountryHolidays
                .Select(x => new CountryHoliday
                {
                    Id = x.Id,
                    CountryDateId = x.CountryDateId,
                    CountryCode = x.CountryDate.CountryCode,
                    Date = x.CountryDate.Date,
                    Name = x.Name
                })
                .SingleOrDefaultAsync(predicate);
        }

        public async Task SaveAsync(CountryHoliday model)
        {
            var entity = new CountryHolidayEntity
            {
                CountryDateId = model.CountryDateId,
                Name = model.Name
            };

            await _storageContext.CountryHolidays.AddAsync(entity);

            await _storageContext.SaveChangesAsync();

            model.Id = entity.Id;
        }

        public async Task SaveRangeAsync(IEnumerable<CountryHoliday> models)
        {
            var entities = models.Select(x => new
                {
                    Entity = new CountryHolidayEntity
                    {
                        CountryDateId = x.CountryDateId,
                        Name = x.Name
                    },
                    Model = x
                })
                .ToList();

            await _storageContext.CountryHolidays.AddRangeAsync(entities.Select(x => x.Entity));

            await _storageContext.SaveChangesAsync();

            foreach (var item in entities)
            {
                item.Model.Id = item.Entity.Id;
            }
        }
    }
}