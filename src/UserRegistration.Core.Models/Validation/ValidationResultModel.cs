using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserRegistration.Core.Models.Validation
{
	public class ValidationResultModel
	{
		private readonly List<ValidationFailureModel> _errors;

		public ValidationResultModel()
		{
			_errors = new List<ValidationFailureModel>();
		}

		public ValidationResultModel(List<ValidationFailureModel> errors)
		{
			_errors = errors;
		}

		public static ValidationResultModel ValidResult => new ValidationResultModel();

		/// <summary>
		/// Whether validation succeeded
		/// </summary>
		public virtual bool IsValid => Errors.Count == 0;

		/// <summary>
		/// A collection of _errors
		/// </summary>
		public List<ValidationFailureModel> Errors => _errors;

		public void AddError(string propertyName, string errorMessage)
		{
			Errors.Add(ValidationFailureModel.Create(propertyName, errorMessage));
		}

		/// <summary>
		/// Generates a string representation of the error messages separated by new lines.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return ToString(Environment.NewLine);
		}

		/// <summary>
		/// Generates a string representation of the error messages separated by the specified character.
		/// </summary>
		/// <param name="separator">The character to separate the error messages.</param>
		/// <returns></returns>
		public string ToString(string separator)
		{
			return string.Join(separator, _errors.Select(failure => failure.ErrorMessage));
		}
	}
}
