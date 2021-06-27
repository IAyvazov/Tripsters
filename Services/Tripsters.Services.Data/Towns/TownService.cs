using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tripsters.Data.Common.Repositories;
using Tripsters.Data.Models;

namespace Tripsters.Services.Data.Towns
{
    public class TownService : ITownService
    {
        private readonly IDeletableEntityRepository<Town> townRepository;

        public TownService(IDeletableEntityRepository<Town> townRepository)
        {
            this.townRepository = townRepository;
        }

        public Town GetTown(string townId)
        {
            var town = this.townRepository.All().FirstOrDefault(x => x.Id == townId);

            return town;
        }

        public async Task<Town> AddTown(Town town)
        {
            if (this.townRepository.All().Any(x => x.Id == town.Id))
            {
                throw new InvalidOperationException("Town already exists.");
            }

            await this.townRepository.AddAsync(town);
            await this.townRepository.SaveChangesAsync();

            return town;
        }

        public Town GetTownByName(string name)
        {
            var town = this.townRepository.All().FirstOrDefault(x => x.Name == name);

            return town;
        }
    }
}
