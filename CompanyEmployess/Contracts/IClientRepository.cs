using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IClientRepository
    {
        Task<IEnumerable<Client>> GetAllClientsAsync(bool trackChanges);
        Task<Client> GetClientAsync(Guid clientId, bool trackChanges);
        void CreateClient(Client client);
        Task<IEnumerable<Client>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges);
        void DeleteClient(Client client);
        void DeleteClient(Task<Client> client);
    }
}
