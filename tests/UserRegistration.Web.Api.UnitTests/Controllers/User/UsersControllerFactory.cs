using AutoMapper;
using Microsoft.Extensions.Logging;
using NSubstitute;
using UserRegistration.Core.Interfaces.Services.User;
using UserRegistration.Web.Api.Controllers.User;

namespace UserRegistration.Web.Api.UnitTests.Controllers.User
{
	public class UsersControllerFactory
	{
		public static UsersController Create(
			IUserService userService, ILogger<UsersController> logger, IMapper mapper)
		{
			return new UsersController(
				userService ?? Substitute.For<IUserService>(),
				logger ?? Substitute.For<ILogger<UsersController>>(),
				mapper ?? Substitute.For<IMapper>()
			);
		}
	}
}
