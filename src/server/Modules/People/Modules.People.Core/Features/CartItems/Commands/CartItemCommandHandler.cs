﻿using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentPOS.Modules.People.Core.Abstractions;
using FluentPOS.Modules.People.Core.Constants;
using FluentPOS.Modules.People.Core.Entities;
using FluentPOS.Modules.People.Core.Exceptions;
using FluentPOS.Modules.People.Core.Features.CartItems.Events;
using FluentPOS.Shared.Core.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Localization;

namespace FluentPOS.Modules.People.Core.Features.CartItems.Commands
{
    internal class CartItemCommandHandler :
        IRequestHandler<AddCartItemCommand, Result<Guid>>,
        IRequestHandler<UpdateCartItemCommand, Result<Guid>>,
        IRequestHandler<RemoveCartItemCommand, Result<Guid>>
    {
        private readonly IPeopleDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<CartItemCommandHandler> _localizer;
        private readonly IDistributedCache _cache;

        public CartItemCommandHandler(
            IPeopleDbContext context,
            IMapper mapper,
            IStringLocalizer<CartItemCommandHandler> localizer,
            IDistributedCache cache)
        {
            _context = context;
            _mapper = mapper;
            _localizer = localizer;
            _cache = cache;
        }

        public async Task<Result<Guid>> Handle(AddCartItemCommand command, CancellationToken cancellationToken)
        {
            var cart = await _context
                .Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(b => b.Id == command.CartId, cancellationToken);

            if (cart == null)
            {
                throw new PeopleException(_localizer["Cart Not Found!"], HttpStatusCode.NotFound);
            }
            if (cart.CartItems.Any(i => i.ProductId == command.ProductId))
            {
                throw new PeopleException(_localizer["Product already added to the Cart."], HttpStatusCode.BadRequest);
            }

            //TODO - how to check if product does not exist (placed in another module)?

            var cartItem = _mapper.Map<CartItem>(command);
            cart.AddDomainEvent(new CartItemAddedEvent(cartItem.Id, command));
            _context.CartItems.Add(cartItem);
            await _context.SaveChangesAsync(cancellationToken);
            return await Result<Guid>.SuccessAsync(cartItem.Id, _localizer["Cart Item Saved"]);
        }

        public async Task<Result<Guid>> Handle(UpdateCartItemCommand command, CancellationToken cancellationToken)
        {
            var cartItem = await _context.CartItems.Where(i => i.Id == command.Id).AsNoTracking().FirstOrDefaultAsync(cancellationToken);
            if (cartItem == null)
            {
                throw new PeopleException(_localizer["Cart Item Not Found!"], HttpStatusCode.NotFound);
            }
            if (await _context.CartItems.AsNoTracking().AnyAsync(i => i.Id != command.Id && i.CartId == command.CartId && i.ProductId == command.ProductId, cancellationToken))
            {
                throw new PeopleException(_localizer["Cart Item with the same Product already exists in the Cart."], HttpStatusCode.BadRequest);
            }

            cartItem = _mapper.Map<CartItem>(command);
            cartItem.AddDomainEvent(new CartItemUpdatedEvent(command));
            _context.CartItems.Update(cartItem);
            await _context.SaveChangesAsync(cancellationToken);
            await _cache.RemoveAsync(PeopleCacheKeys.GetCartItemByIdCacheKey(command.Id), cancellationToken);
            return await Result<Guid>.SuccessAsync(cartItem.Id, _localizer["Cart Item Updated"]);
        }

        public async Task<Result<Guid>> Handle(RemoveCartItemCommand command, CancellationToken cancellationToken)
        {
            var cartItem = await _context.CartItems.FirstOrDefaultAsync(b => b.Id == command.Id, cancellationToken);
            if (cartItem == null)
            {
                throw new PeopleException(_localizer["Cart Item Not Found!"], HttpStatusCode.NotFound);
            }
            cartItem.AddDomainEvent(new CartItemRemovedEvent(cartItem.Id));
            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync(cancellationToken);
            await _cache.RemoveAsync(PeopleCacheKeys.GetCartItemByIdCacheKey(command.Id), cancellationToken);
            return await Result<Guid>.SuccessAsync(cartItem.Id, _localizer["Cart Item Deleted"]);
        }
    }
}