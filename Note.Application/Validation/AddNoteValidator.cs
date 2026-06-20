using FluentValidation;
using Note.Domain.Models.Note;

namespace Note.Application.Validation
{
    public class AddNoteValidator : AbstractValidator<AddNoteModel>
    {
        public AddNoteValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("عنوان اجباری است")
                .MaximumLength(300).WithMessage("عنوان نباید بیشتر از 300 کاراکتر باشد");

            RuleFor(x => x.Text)
                .NotEmpty().WithMessage("متن اجباری است");

            RuleFor(x => x.CategoryId)
                .GreaterThan(0).WithMessage("شناسه دسته بندی نامعتبر است");

            RuleFor(x => x.Tags)
                .Must(tags => tags.Distinct().Count() == tags.Count).WithMessage("تگ‌ها نباید تکراری باشند"); 
        }
    }
}