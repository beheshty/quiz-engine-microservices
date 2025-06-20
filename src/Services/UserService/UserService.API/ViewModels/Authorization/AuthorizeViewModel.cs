using System.ComponentModel.DataAnnotations;
using OpenIddict.Abstractions;

namespace UserService.API.ViewModels.Authorization;

public class AuthorizeViewModel
{
    [Display(Name = "Application")]
    public string? ApplicationName { get; set; }

    [Display(Name = "Scope")]
    public string? Scope { get; set; }

    public IReadOnlyDictionary<string, OpenIddictParameter> Parameters { get; set; } = new Dictionary<string, OpenIddictParameter>();
} 