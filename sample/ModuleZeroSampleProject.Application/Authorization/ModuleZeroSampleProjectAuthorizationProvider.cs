using Abp.Authorization;
using Abp.Localization;

namespace ModuleZeroSampleProject.Authorization
{
    public class ModuleZeroSampleProjectAuthorizationProvider : AuthorizationProvider
    {
        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            //TODO: Localize (Change FixedLocalizableString to LocalizableString)

            var root = context.CreateRootGroup("QuestionAndAnswerSystem", new FixedLocalizableString("QuestionAndAnswerSystem"));

            root.CreatePermission("CanCreateQuestions", new FixedLocalizableString("Can create questions"));
            root.CreatePermission("CanDeleteQuestions", new FixedLocalizableString("Can delete questions"));
            root.CreatePermission("CanDeleteAnswers", new FixedLocalizableString("Can delete answers"));
            root.CreatePermission("CanAnswerToQuestions", new FixedLocalizableString("Can answer to questions"), isGrantedByDefault: true);
        }
    }
}
