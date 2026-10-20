using FluentValidation;
using Microsoft.Extensions.Localization;
using SchoolProject.Core.Features.ApplicationUser.Command.Model;
using SchoolProject.Core.Resources;

namespace SchoolProject.Core.Features.ApplicationUser.Command.Validation
{
    public class EditUserValidator : AbstractValidator<EditUserCommand>
    {
        private readonly IStringLocalizer<SharedResources> _localizer;

        public EditUserValidator(IStringLocalizer<SharedResources> localizer)
        {

            _localizer = localizer;
            ApplyValidationRules();
            ApplyCustomValidationRules();
        }

        public void ApplyValidationRules()
        {
            RuleFor(x => x.FullName)
                 .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty])
                 .NotNull().WithMessage(_localizer[SharedResourcesKeys.Required])
                 .MaximumLength(100).WithMessage(_localizer[SharedResourcesKeys.MaxLengthis100]);

            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty])
                .NotNull().WithMessage(_localizer[SharedResourcesKeys.Required])
                .MaximumLength(100).WithMessage(_localizer[SharedResourcesKeys.MaxLengthis100]);

            RuleFor(x => x.Email)
                 .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty])
                 .NotNull().WithMessage(_localizer[SharedResourcesKeys.Required]);

        }
        public void ApplyCustomValidationRules()
        {


        }


    }
}
