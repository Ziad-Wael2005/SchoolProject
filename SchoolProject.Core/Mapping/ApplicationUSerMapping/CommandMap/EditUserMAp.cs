using SchoolProject.Core.Features.ApplicationUser.Command.Model;
using SchoolProject.Data.Entites.Identity;

namespace SchoolProject.Core.Mapping.ApplicationUSerMapping
{
    public partial class ApplicationUserProfile
    {

        public void UpdateUserMAp()
        {

            CreateMap<EditUserCommand, User>()
                 .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Country));

        }
    }
}
