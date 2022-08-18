using Microsoft.EntityFrameworkCore;
using System;

namespace Bank.Infra.Data.UoW
{
    public interface IUnitOfWork<T> where T : DbContext, IDisposable
    {
        T GetContext();
        bool Commit();
        void BeginTransaction();
        void Rollback();
    }
}
