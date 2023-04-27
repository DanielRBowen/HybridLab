using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;

namespace HybridLab.Core.Components
{
    public class PhoneInput : InputBase<string>
    {
        //[Parameter(CaptureUnmatchedValues = true)]
        //public IDictionary<string, object> AdditionalAttributes { get; set; }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            builder.OpenElement(0, "input");
            builder.AddAttribute(1, "type", "tel");
            builder.AddAttribute(2, "value", BindConverter.FormatValue(CurrentValue));
            builder.AddAttribute(3, "oninput", EventCallback.Factory.CreateBinder<string>(this, __value => CurrentValueAsString = __value, CurrentValueAsString));
            builder.AddMultipleAttributes(4, AdditionalAttributes);
            builder.CloseElement();
        }

        protected override string? FormatValueAsString(string? value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }

            var digits = new string(value.Where(char.IsDigit).ToArray());
            if (digits.Length == 10)
            {
                return string.Format("({0}) {1}-{2}", digits.Substring(0, 3), digits.Substring(3, 3), digits.Substring(6));
            }
            else
            {
                return base.FormatValueAsString(value);
            }

        }

        protected override bool TryParseValueFromString(string value, out string result, out string validationErrorMessage)
        {
            if (string.IsNullOrEmpty(value))
            {
                result = string.Empty;
                validationErrorMessage = null;
                return true;
            }

            bool canBeInt = int.TryParse(value, out int resultNumber);

            if (canBeInt) 
            {
                result = value;
                validationErrorMessage = null;
                return true;
            }
            else
            {
                result = string.Empty;
                validationErrorMessage = "The number must parse as a number.";
                return false;
            }
            //var digits = new string(value.Where(char.IsDigit).ToArray());
            //if (digits.Length == 10 && int.TryParse(digits, out _))
            //{
            //    result = string.Format("({0}) {1}-{2}", digits.Substring(0, 3), digits.Substring(3, 3), digits.Substring(6));
            //    validationErrorMessage = null;
            //    return true;
            //}
            //else
            //{
            //    result = string.Empty;
            //    validationErrorMessage = "The input must be a valid US phone number with 10 digits.";
            //    return false;
            //}
        }
    }
}
