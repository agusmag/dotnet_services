using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApiAuthors.Validations;

namespace WebApiAuthors.Models
{
	public class Author : IValidatableObject
	{
		public int Id { get; set; }

		[Required(ErrorMessage = "The field {0} is required (custom)")]
		[StringLength(maximumLength:120, ErrorMessage = "The field {0} must not be greater than 10 chars")]
		//[FirstLetterUpper] // Custom validation per attribute
		public string Name { get; set; }

		//[Range(18, 120)]
		//[NotMapped] // Not mapped with the DB
		//public int Age { get; set; }

		//[CreditCard]
  //      [NotMapped]
  //      public string CreditCard { get; set; }

		//[Url]
  //      [NotMapped]
  //      public string URL { get; set; }

		public List<Book> Books { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
			// Add all the validations over the Model you want, and add the errors with yield
            if (!string.IsNullOrEmpty(Name))
			{
				var firstLetter = Name[0].ToString();

				if (firstLetter != firstLetter.ToUpper())
				{
					yield return new ValidationResult("The first letter must be in upper case",
						new string[] { nameof(Name) });
				}
			}
        }
    }
}

