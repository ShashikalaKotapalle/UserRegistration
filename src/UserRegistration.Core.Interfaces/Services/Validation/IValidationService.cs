using System.Threading.Tasks;
using UserRegistration.Core.Models.Validation;

namespace UserRegistration.Core.Interfaces.Services.Validation
{
	public interface IValidationService
	{
		ValidationResultModel Validate<T>(T instance) where T : class;
		Task<ValidationResultModel> ValidateAsync<T>(T instance) where T : class;
	}
}
