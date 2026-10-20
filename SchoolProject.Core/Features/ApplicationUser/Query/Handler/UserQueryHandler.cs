using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using SchoolProject.Core.Bases;
using SchoolProject.Core.Features.ApplicationUser.Query.Model;
using SchoolProject.Core.Features.ApplicationUser.Query.Result;
using SchoolProject.Core.Resources;
using SchoolProject.Core.Wrapper;
using SchoolProject.Data.Entites.Identity;

namespace SchoolProject.Core.Features.ApplicationUser.Query.Handler
{
    public class UserQueryHandler : ResponseHandler,
        IRequestHandler<GetUserpaginatedQuery, PaginatedResult<GetUserPaginatedListResponse>>,
        IRequestHandler<GetUserByIdQuery, Response<GetUserByIdResponse>>
    {
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;



        public UserQueryHandler(IStringLocalizer<SharedResources> stringLocalizer, IMapper mapper, UserManager<User> userManager) : base(stringLocalizer)
        {
            _stringLocalizer = stringLocalizer;
            _mapper = mapper;
            _userManager = userManager;
        }

        public Task<PaginatedResult<GetUserPaginatedListResponse>> Handle(GetUserpaginatedQuery request, CancellationToken cancellationToken)
        {
            var users = _userManager.Users.AsQueryable();
            var paginatedList = _mapper.ProjectTo<GetUserPaginatedListResponse>(users).ToPaginatedListAsync(request.PageNumber, request.PageSize);
            return paginatedList;
        }



        public async Task<Response<GetUserByIdResponse>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            //var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id==request.Id);
            var user = await _userManager.FindByIdAsync(request.Id.ToString());
            if (user == null) return NotFound<GetUserByIdResponse>(_stringLocalizer[SharedResourcesKeys.Notfound]);
            var result = _mapper.Map<GetUserByIdResponse>(user);
            return Success(result);
        }

    }
}
