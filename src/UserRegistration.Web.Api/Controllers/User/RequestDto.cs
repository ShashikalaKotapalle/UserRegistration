using System.ComponentModel.DataAnnotations;

namespace UserRegistration.Web.Api.Controllers.User
{
	public class RequestDto
	{
		[Required][MaxLength(100)]
		public string UserName { get; set; }
	}
}
