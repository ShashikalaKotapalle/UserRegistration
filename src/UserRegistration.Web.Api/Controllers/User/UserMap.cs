using AutoMapper;
using UserRegistration.Core.Models.User;

namespace UserRegistration.Web.Api.Controllers.User
{
	public class UserMap : Profile
	{
		public UserMap()
		{
			CreateMap<UserModel, UsersDto>()
				.ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.UserId))
				.ForMember(dest => dest.UserName, opts => opts.MapFrom(src => src.UserName));


			CreateMap<UsersDto, UserModel>()
				//.ForMember(dest => dest.UserId, opts => opts.MapFrom(src => src.Id))
				.ForMember(dest => dest.UserName, opts => opts.MapFrom(src => src.UserName));

		}
	}
}
