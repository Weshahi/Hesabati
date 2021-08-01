using FluentPOS.Modules.Identity.Core.Entities;
using FluentPOS.Shared.Core.Features.ExtendedAttributes.Queries.Validators;
using Microsoft.Extensions.Localization;
using System;

namespace FluentPOS.Modules.Identity.Core.Features.ExtendedAttributes.Validators.Roles
{
    public class RolePaginatedExtendedAttributeFilterValidator : PaginatedExtendedAttributeFilterValidator<Guid, FluentRole>
    {
        public RolePaginatedExtendedAttributeFilterValidator(IStringLocalizer<RolePaginatedExtendedAttributeFilterValidator> localizer) : base(localizer)
        {
            // you can override the validation rules here
        }
    }
}