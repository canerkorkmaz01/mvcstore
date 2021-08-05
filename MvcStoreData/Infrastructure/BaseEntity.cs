﻿using System;

namespace MvcStoreData.Infrastructure
{
    public abstract class BaseEntity : IBaseEntity
    {
        public virtual int Id { get; set; }

        public virtual int UserId { get; set; }

        public virtual DateTime DateCreated { get; set; }

        public virtual bool Enabled { get; set; }

        public virtual User User { get; set; }
    }
}