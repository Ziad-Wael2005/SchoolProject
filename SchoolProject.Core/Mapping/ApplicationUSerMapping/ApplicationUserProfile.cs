using AutoMapper;

namespace SchoolProject.Core.Mapping.ApplicationUSerMapping
{
    public partial class ApplicationUserProfile : Profile
    {
        public ApplicationUserProfile()
        {

            AddUserMap();
            GetUserPaginatedMap();
            GetUserByIdMap();
            UpdateUserMAp();
        }
    }
}
