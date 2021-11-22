using System.ComponentModel.DataAnnotations;

namespace UserRegistration.Core.Models.User
{
	public class UserModel
	{
		public int UserId { get; set; }
		
		public string UserName { get; set; }
	}
}
