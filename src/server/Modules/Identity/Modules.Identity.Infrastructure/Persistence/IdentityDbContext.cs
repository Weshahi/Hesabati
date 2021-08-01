﻿using System.Collections.Generic;
using System;
using System.Threading;
using System.Threading.Tasks;
using FluentPOS.Modules.Identity.Core.Abstractions;
using FluentPOS.Modules.Identity.Core.Entities;
using FluentPOS.Modules.Identity.Core.Entities.ExtendedAttributes;
using FluentPOS.Modules.Identity.Infrastructure.Extensions;
using FluentPOS.Shared.Core.Domain;
using FluentPOS.Shared.Core.EventLogging;
using FluentPOS.Shared.Core.Interfaces;
using FluentPOS.Shared.Core.Interfaces.Serialization;
using FluentPOS.Shared.Core.Settings;
using FluentPOS.Shared.Infrastructure.Extensions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace FluentPOS.Modules.Identity.Infrastructure.Persistence
{
    public sealed class IdentityDbContext : IdentityDbContext<FluentUser, FluentRole, Guid, IdentityUserClaim<Guid>, IdentityUserRole<Guid>, IdentityUserLogin<Guid>, FluentRoleClaim, IdentityUserToken<Guid>>,
        IIdentityDbContext,
        IModuleDbContext,
        IExtendedAttributeDbContext<Guid, FluentUser, UserExtendedAttribute>,
        IExtendedAttributeDbContext<Guid, FluentRole, RoleExtendedAttribute>
    {
        private readonly IMediator _mediator;
        private readonly IEventLogger _eventLogger;
        private readonly PersistenceSettings _persistenceOptions;
        private readonly IJsonSerializer _json;

        internal string Schema => "Identity";

        public IdentityDbContext(
            DbContextOptions<IdentityDbContext> options,
            IOptions<PersistenceSettings> persistenceOptions,
            IMediator mediator,
            IEventLogger eventLogger, IJsonSerializer json)
                : base(options)
        {
            _mediator = mediator;
            _eventLogger = eventLogger;
            _persistenceOptions = persistenceOptions.Value;
            _json = json;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(Schema);
            modelBuilder.Ignore<Event>();
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
            modelBuilder.ApplyIdentityConfiguration(_persistenceOptions);
        }
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var changes = OnBeforeSaveChanges();
            return await this.SaveChangeWithPublishEventsAsync(_eventLogger, _mediator, changes, cancellationToken);
        }
        private (string oldValues, string newValues) OnBeforeSaveChanges()
        {
            var previousData = new Dictionary<string, object>();
            var currentData = new Dictionary<string, object>();
            ChangeTracker.DetectChanges();
            foreach (var entry in ChangeTracker.Entries())
            {
                foreach (var property in entry.Properties)
                {
                    string propertyName = property.Metadata.Name;
                    var originalValue = entry.GetDatabaseValues()?.GetValue<object>(propertyName);
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            currentData[propertyName] = property.CurrentValue;
                            break;
                        case EntityState.Deleted:
                            previousData[propertyName] = originalValue;
                            break;

                        case EntityState.Modified:

                            if (property.IsModified && originalValue?.Equals(property.CurrentValue) == false)
                            {
                                previousData[propertyName] = originalValue;
                                currentData[propertyName] = property.CurrentValue;
                            }
                            break;
                    }
                }
            }
            var oldValues = previousData.Count == 0 ? null : _json.Serialize(previousData);
            var newValues = currentData.Count == 0 ? null : _json.Serialize(currentData);
            return (oldValues: oldValues, newValues: newValues);
        }
        public override int SaveChanges()
        {
            var changes = OnBeforeSaveChanges();
            return this.SaveChangeWithPublishEvents(_eventLogger, _mediator, changes);
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            return SaveChanges();
        }

        DbSet<FluentUser> IExtendedAttributeDbContext<Guid, FluentUser, UserExtendedAttribute>.GetEntities() => Users;

        DbSet<UserExtendedAttribute> IExtendedAttributeDbContext<Guid, FluentUser, UserExtendedAttribute>.ExtendedAttributes { get; set; }

        DbSet<FluentRole> IExtendedAttributeDbContext<Guid, FluentRole, RoleExtendedAttribute>.GetEntities() => Roles;

        DbSet<RoleExtendedAttribute> IExtendedAttributeDbContext<Guid, FluentRole, RoleExtendedAttribute>.ExtendedAttributes { get; set; }
    }
}