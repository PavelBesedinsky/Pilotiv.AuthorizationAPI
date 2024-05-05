﻿using Pilotiv.AuthorizationAPI.Domain.Models.Users.ValueObjects;
using Pilotiv.AuthorizationAPI.Domain.Primitives;

namespace Pilotiv.AuthorizationAPI.Domain.Models.Users.Events;

/// <summary>
/// Событие изменения хэша-пароля пользователя.
/// </summary>
/// <param name="UserId">Идентификатор пользователя.</param>
/// <param name="PasswordHash">Хэш-пароля пользователя.</param>
public record UserPasswordHashChangedDomainEvent(UserId UserId, UserPasswordHash PasswordHash) : IDomainEvent;