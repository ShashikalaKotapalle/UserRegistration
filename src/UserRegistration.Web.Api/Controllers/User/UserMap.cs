using AutoMapper;
using UserRegistration.Core.Models.User;

namespace UserRegistration.Web.Api.Controllers.User
{
	public class UserMap : Profile
	{
		public UserMap()
		{
			CreateMap<UserModel, ResponseDto>()
				.ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.UserId))
				.ForMember(dest => dest.UserName, opts => opts.MapFrom(src => src.UserName));


			CreateMap<RequestDto, UserModel>()
				.ForMember(dest => dest.UserName, opts => opts.MapFrom(src => src.UserName));

		}
	}
}
