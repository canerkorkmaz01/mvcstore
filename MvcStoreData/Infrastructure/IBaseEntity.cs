using System;

namespace MvcStoreData.Infrastructure
{
    public interface IBaseEntity
    {
        int Id { get; set; }

        int UserId { get; set; }

        DateTime DateCreated { get; set; }

        bool Enabled { get; set; }
    }
}
