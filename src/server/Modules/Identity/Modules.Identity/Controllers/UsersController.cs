﻿// --------------------------------------------------------------------------------------------------
// <copyright file="UsersController.cs" company="FluentPOS">
// Copyright (c) FluentPOS. All rights reserved.
// The core team: Mukesh Murugan (iammukeshm), Chhin Sras (chhinsras), Nikolay Chebotov (unchase).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System.Threading.Tasks;
using FluentPOS.Modules.Identity.Core.Abstractions;
using FluentPOS.Shared.Core.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace FluentPOS.Modules.Identity.Controllers
{
    [ApiVersion("1")]
    internal sealed class UsersController : BaseController
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Authorize(Policy = Permissions.Users.View)]
        public async Task<IActionResult> GetAllAsync()
        {
            var users = await _userService.GetAllAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = Permissions.Users.View)]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var user = await _userService.GetAsync(id);
            return Ok(user);
        }

        [HttpGet("roles/{id}")]
        [Authorize(Policy = Permissions.Users.View)]
        public async Task<IActionResult> GetRolesAsync(Guid id)
        {
            var userRoles = await _userService.GetRolesAsync(id);
            return Ok(userRoles);
        }
    }
}