using System;
using System.Collections.Generic;
using Ertis.Core.Human;

namespace Ertis.WebService.Dao.Contracts
{
    public interface ICredentialsRepository
    {
        Credentials Get(int id);
        List<Credentials> GetList();
        Credentials Add(string name, string surname, string emailAddress, string phoneNumber, DateTime? birthDate);
        bool Update(int id, string name = null, string surname = null, DateTime? birthDate = null, string emailAddress = null, string phoneNumber = null);
        bool Remove(int id);
        bool Remove(Credentials credentials);
    }
}
