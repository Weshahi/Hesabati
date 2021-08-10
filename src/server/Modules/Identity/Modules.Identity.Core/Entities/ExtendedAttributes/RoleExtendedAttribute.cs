// --------------------------------------------------------------------------------------------------
// <copyright file="RoleExtendedAttribute.cs" company="FluentPOS">
// Copyright (c) FluentPOS. All rights reserved.
// The core team: Mukesh Murugan (iammukeshm), Chhin Sras (chhinsras), Nikolay Chebotov (unchase).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using FluentPOS.Shared.Core.Domain;
using System;

namespace FluentPOS.Modules.Identity.Core.Entities.ExtendedAttributes
{
    public class RoleExtendedAttribute : ExtendedAttribute<Guid, FluentRole> { }
}