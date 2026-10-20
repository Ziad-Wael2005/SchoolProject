using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using SchoolProject.Core.Bases;
using SchoolProject.Core.Features.ApplicationUser.Command.Model;
using SchoolProject.Core.Resources;
using SchoolProject.Data.Entites.Identity;

namespace SchoolProject.Core.Features.ApplicationUser.Command.Handler
{
    public class UserCommandHandler : ResponseHandler
        , IRequestHandler<AddUserCommand, Response<string>>
        , IRequestHandler<EditUserCommand, Response<string>>
        , IRequestHandler<DeleteUserCommand, Response<string>>
        , IRequestHandler<ChangeUserPasswordCommand, Response<string>>

    {
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        private readonly UserManager<User> _userManager;
        public UserCommandHandler(IStringLocalizer<SharedResources> stringLocalizer, IMapper mapper, UserManager<User> userManager) : base(stringLocalizer)
        {
            _stringLocalizer = stringLocalizer;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<Response<string>> Handle(AddUserCommand request, CancellationToken cancellationToken)
        {
            //if email is exist
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user != null)
            {
                return BadRequest<string>(_stringLocalizer[SharedResourcesKeys.EmailIsExist]);
            }

            var userByName = await _userManager.FindByNameAsync(request.UserName);
            if (userByName != null)
            {
                return BadRequest<string>(_stringLocalizer[SharedResourcesKeys.UserNameIsExist]);
            }
            // mapping

            var UserAfterMap = _mapper.Map<User>(request);
            var createResult = await _userManager.CreateAsync(UserAfterMap, request.Password);

            if (!createResult.Succeeded)
            {
                return BadRequest<string>(createResult.Errors.FirstOrDefault().Description);

            }

            return Created("");


        }

        public async Task<Response<string>> Handle(EditUserCommand request, CancellationToken cancellationToken)
        {
            // Check if user is exist
            var olduser = await _userManager.FindByIdAsync(request.Id.ToString());
            // notfound
            if (olduser == null)
            {
                return NotFound<string>();

            }
            //mapping
            var newUser = _mapper.Map(request, olduser);

            var userByName = await _userManager.Users.FirstOrDefaultAsync(x=>x.UserName==request.UserName&&x.Id!=newUser.Id);
            if (userByName != null)
            {
                return BadRequest<string>(_stringLocalizer[SharedResourcesKeys.UserNameIsExist]);
            }
            //update
            var result = await _userManager.UpdateAsync(newUser);
            //result is not success
            if (!result.Succeeded)
            {
                return BadRequest<string>(_stringLocalizer[SharedResourcesKeys.UpdateFailed]);
            }

            return Success((string)_stringLocalizer[SharedResourcesKeys.Updated]);
        }

        public async Task<Response<string>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            // Check if user is exist
            var user = await _userManager.FindByIdAsync(request.Id.ToString());
            // notfound
            if (user == null)
            {
                return NotFound<string>();

            }
            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {

                return BadRequest<string>(_stringLocalizer[SharedResourcesKeys.DeletedFailed]);
            }

            return Success((string)_stringLocalizer[SharedResourcesKeys.Deleted]);

        }

        public async Task<Response<string>> Handle(ChangeUserPasswordCommand request, CancellationToken cancellationToken)
        {
            // Check if user is exist
            var user = await _userManager.FindByIdAsync(request.Id.ToString());
            // notfound
            if (user == null)
            {
                return NotFound<string>();

            }

            var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
            // another way

            //var user1=await _userManager.HasPasswordAsync(user);
            //await _userManager.RemovePasswordAsync(user);
            //await _userManager.AddPasswordAsync(user,request.NewPassword);

            if (!result.Succeeded)
            {

                return BadRequest<string>(result.Errors.FirstOrDefault().Description);
            }

            return Success((string)_stringLocalizer[SharedResourcesKeys.Success]);

        }
    }
}
