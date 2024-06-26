﻿using System.ComponentModel.DataAnnotations;

namespace Pilotiv.AuthorizationAPI.WebUI.Dtos.AuthController;

/// <summary>
/// Объект переноса данных команды регистрации.
/// </summary>
public class RegisterRequest
{
    /// <summary>
    /// Логин.
    /// </summary>
    [Required]
    public string? Login { get; init; }

    /// <summary>
    /// Пароль.
    /// </summary>
    [Required]
    public string? Password { get; init; }

    /// <summary>
    /// Адрес электронной почты.
    /// </summary>
    [Required]
    [EmailAddress]
    public string? Email { get; init; }
}