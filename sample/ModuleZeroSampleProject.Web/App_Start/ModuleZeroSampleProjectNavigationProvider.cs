using Abp.Application.Navigation;
using Abp.Localization;

namespace ModuleZeroSampleProject.Web
{
    /// <summary>
    /// This class defines menus for the application.
    /// It uses ABP's menu system.
    /// When you add menu items here, they are automatically appear in angular application.
    /// See .cshtml and .js files under App/Main/views/layout/header to know how to render menu.
    /// </summary>
    public class ModuleZeroSampleProjectNavigationProvider : NavigationProvider
    {
        public override void SetNavigation(INavigationProviderContext context)
        {
            context.Manager.MainMenu
                .AddItem(
                    new MenuItemDefinition(
                        "Questions",
                        new LocalizableString("Questions", ModuleZeroSampleProjectConsts.LocalizationSourceName),
                        url: "#/",
                        icon: "fa fa-question"
                        )
                ).AddItem(
                    new MenuItemDefinition(
                        "People",
                        new LocalizableString("People", ModuleZeroSampleProjectConsts.LocalizationSourceName),
                        url: "#/people",
                        icon: "fa fa-users"
                        )
                );
        }
    }
}
