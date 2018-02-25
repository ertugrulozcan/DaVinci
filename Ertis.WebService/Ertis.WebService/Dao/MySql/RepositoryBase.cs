using System;
namespace Ertis.WebService.Dao.MySql
{
    public abstract class RepositoryBase
    {
        protected IServiceProvider serviceProvider;

        protected readonly string CONNECTION_STRING;

        protected readonly bool IsRecursiveSelectionEnable = true;

        public RepositoryBase(string connectionString)
        {
            this.CONNECTION_STRING = connectionString;
        }
    }
}
