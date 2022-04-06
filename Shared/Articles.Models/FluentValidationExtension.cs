//using FluentValidation;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace Articles.Models
//{
//    public static class FluentValidationExtension
//    {
//        public static Func<T, string, Task<IEnumerable<string>>> ValidateValue<T>(this AbstractValidator<T> fv) => async (model, propertyName) =>
//        {
//            var result = await fv.ValidateAsync(ValidationContext<T>.CreateWithOptions((T)model, x => x.IncludeProperties(propertyName)));
//            return
//                result.IsValid ?
//                (IEnumerable<string>)Array.Empty<string>() :
//                result.Errors.Select(e => e.ErrorMessage);
//        };
//    }

//    public class FluentValueValidator<T> : AbstractValidator<T>
//    {
//        private readonly AbstractValidator<T> _validations;

//        public FluentValueValidator(AbstractValidator<T> validations)
//        {
//            _validations = validations;
//        }

//        private IEnumerable<string> ValidateValue(T arg)
//        {
//            var result = _validations.Validate(arg);
//            if (result.IsValid)
//                return new string[0];
//            return result.Errors.Select(e => e.ErrorMessage);
//        }

//        public Func<T, IEnumerable<string>> Validation => ValidateValue;
//    }
//}
