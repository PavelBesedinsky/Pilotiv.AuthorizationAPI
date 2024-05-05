using FluentResults;
using Pilotiv.AuthorizationAPI.Domain.Models.Users.ValueObjects;

namespace Pilotiv.AuthorizationAPI.Application.Requests.Auth.Commands.Register.Errors;

public static class RegisterErrors
{
    public static Error EmailIsOccupied(UserEmail email) => new($"Электронный адрес почты ({email.Value}) уже используется.");
    public static Error LoginIsOccupied(UserLogin login) => new($"Логин ({login.Value}) уже используется.");
}