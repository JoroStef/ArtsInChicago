using ArtsInChicago.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArtsInChicago.Services.Contracts
{
    public interface IArtworkTypesService
    {
        Task<int> GetCountAsync();
        Task<IEnumerable<string>> GetAllAsync(int? pageLimit);
    }
}
