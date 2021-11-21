using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation.Results;
using UserRegistration.Core.Interfaces.Services.Validation;
using UserRegistration.Core.Models.Validation;
using UserRegistration.Core.Enums;

namespace UserRegistration.Core.Services.Validation
{
	public class ValidationService : IValidationService
	{
		private readonly IServiceProvider _services;

		public ValidationService(IServiceProvider services)
		{
			_services = services;
		}

		public ValidationResultModel Validate<T>(T instance) where T : class
		{
			var result = GetValidator<T>().Validate(new ValidationContext<T>(instance));
			return new ValidationResultModel(MapErrors(result.Errors));
		}

		public async Task<ValidationResultModel> ValidateAsync<T>(T instance) where T : class
		{
			var result = await GetValidator<T>().ValidateAsync(new ValidationContext<T>(instance));
			return new ValidationResultModel(MapErrors(result.Errors));
		}

		public IValidator GetValidator<T>() where T : class
		{
			return GetValidator(typeof(T));
		}

		public IValidator GetValidator(Type type)
		{
			return (IValidator) _services.GetService(typeof(IValidator<>).MakeGenericType(type));
		}

		public List<ValidationFailureModel> MapErrors(IList<ValidationFailure> errors)
		{
			return errors.Select(e => new ValidationFailureModel
			{
				AttemptedValue = e.AttemptedValue,
				CustomState = e.CustomState,
				ErrorCode = e.ErrorCode,
				ErrorMessage = e.ErrorMessage,
				FormattedMessagePlaceholderValues = e.FormattedMessagePlaceholderValues,
				PropertyName = e.PropertyName,
				Severity = (ValidationErrorSeverity) Enum.Parse(typeof(ValidationErrorSeverity), e.Severity.ToString())
			}).ToList();
		}
	}
}
