﻿// --------------------------------------------------------------------------------------------------
// <copyright file="IUserService.cs" company="FluentPOS">
// Copyright (c) FluentPOS. All rights reserved.
// The core team: Mukesh Murugan (iammukeshm), Chhin Sras (chhinsras), Nikolay Chebotov (unchase).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using FluentPOS.Shared.Core.Wrapper;
using FluentPOS.Shared.DTOs.Identity.Users;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FluentPOS.Modules.Identity.Core.Abstractions
{
    public interface IUserService
    {
        Task<Result<List<UserResponse>>> GetAllAsync();

        Task<IResult<UserResponse>> GetAsync(Guid userId);

        Task<IResult<UserRolesResponse>> GetRolesAsync(Guid userId);

        Task<IResult<Guid>> UpdateAsync(UpdateUserRequest request);

        Task<IResult<Guid>> UpdateUserRolesAsync(Guid userId, UserRolesRequest request);
    }
}