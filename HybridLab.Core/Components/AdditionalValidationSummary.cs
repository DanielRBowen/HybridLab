using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;


namespace HybridLab.Core.Components
{
    /// <summary>
    /// Code taken from ChatGPT 2/23/2023
    /// </summary>
    public class AdditionalValidationSummary : ComponentBase
    {
        [Parameter]
        public Dictionary<string, string> ValidationErrors { get; set; }

        [Parameter]
        public string TextColor { get; set; } = "red";

        // This component is intended to be used with errors from a restful post api call from the server
        // but is named AdditionalValidationSummary instead of ServerValidationSummary because it could be
        // used for additional client-side errors apart from the data annotation errors which are normally
        // taken care of with use of the Blazor component library.

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            if (ValidationErrors != null && ValidationErrors.Count > 0)
            {
                builder.OpenElement(0, "ul");
                builder.AddAttribute(1, "style", $"color: {TextColor};");

                foreach (var error in ValidationErrors)
                {
                    builder.OpenElement(2, "li");
                    builder.AddContent(3, error.Value);
                    builder.CloseElement();
                }

                builder.CloseElement();
            }
        }
    }
}
