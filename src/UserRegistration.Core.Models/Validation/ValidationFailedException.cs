using System;
using System.Collections.Generic;
using System.Linq;

namespace UserRegistration.Core.Models.Validation
{
	public class ValidationFailedException : Exception
	{
		/// <summary>
		/// Creates a new ValidationFailedException
		/// </summary>
		/// <param name="message"></param>
		public ValidationFailedException(string message) : this(message, Enumerable.Empty<ValidationFailureModel>())
		{
		}

		/// <summary>
		/// Creates a new ValidationException
		/// </summary>
		/// <param name="message"></param>
		/// <param name="errors"></param>
		public ValidationFailedException(string message, IEnumerable<ValidationFailureModel> errors) : base(message)
		{
			Errors = errors;
		}

		/// <summary>
		/// Creates a new ValidationException
		/// </summary>
		/// <param name="errors"></param>
		public ValidationFailedException(IEnumerable<ValidationFailureModel> errors) : base(BuildErrorMessage(errors))
		{
			Errors = errors;
		}

		public ValidationFailedException(string propertyName, string message) : base(message)
		{
			Errors = new List<ValidationFailureModel>
			{
				ValidationFailureModel.Create(propertyName, message)
			};
		}

		/// <summary>
		/// Validation errors
		/// </summary>
		public IEnumerable<ValidationFailureModel> Errors { get; private set; }

		private static string BuildErrorMessage(IEnumerable<ValidationFailureModel> errors)
		{
			var arr = errors.Select(x => $"{Environment.NewLine} -- {x.PropertyName}: {x.ErrorMessage}");
			return "Validation failed: " + string.Join(string.Empty, arr);
		}
	}
}
