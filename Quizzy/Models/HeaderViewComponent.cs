using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Quizzy.Data.Entities.Identity;

namespace Quizzy.Models;

public class HeaderViewComponent : ViewComponent
{
    private readonly UserManager<UserEntity> _userManager;

    public HeaderViewComponent(UserManager<UserEntity> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var user = await _userManager.GetUserAsync(UserClaimsPrincipal);
        return View("~/Views/Shared/Components/Header/Default.cshtml", new HeaderViewModel
        {
            UserName = user.FirstName + " " + user.LastName,
            Image = user.Image
        });
    }
}

public class HeaderViewModel
{
    public string UserName { get; set; }
    public string Image { get; set; }
    public string Role { get; set; }
}