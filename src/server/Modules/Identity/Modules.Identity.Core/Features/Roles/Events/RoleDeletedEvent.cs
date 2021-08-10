// --------------------------------------------------------------------------------------------------
// <copyright file="RoleDeletedEvent.cs" company="FluentPOS">
// Copyright (c) FluentPOS. All rights reserved.
// The core team: Mukesh Murugan (iammukeshm), Chhin Sras (chhinsras), Nikolay Chebotov (unchase).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using FluentPOS.Modules.Identity.Core.Entities;
using FluentPOS.Shared.Core.Domain;

namespace FluentPOS.Modules.Identity.Core.Features.Roles.Events
{
    public class RoleDeletedEvent : Event
    {
        public Guid Id { get; }

        public RoleDeletedEvent(Guid id)
        {
            Id = id;
            AggregateId = id == Guid.Empty
                ? Guid.NewGuid()
                : id;
            RelatedEntities= new[] { typeof(FluentRole) };
        }
    }
}