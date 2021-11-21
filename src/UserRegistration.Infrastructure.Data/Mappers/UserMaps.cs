using AutoMapper;
using UserRegistration.Core.Models.User;
using UserRegistration.Infrastructure.Data.Entities;

namespace UserRegistration.Infrastructure.Data.Mappers
{
	public class UserMaps : Profile
	{
		public UserMaps()
		{
			CreateMap<UserModel, User>();
			CreateMap<User, UserModel>();
		}
	}
}
