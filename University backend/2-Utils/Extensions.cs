using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace University_backend;

public static class Extensions
{
    public static string GetFirstError(this ModelStateDictionary modelState)
    {
        return modelState.Values.Where(v => v.Errors.Any()).First().Errors.First().ErrorMessage;
    }

    public static string GetAllErrors(this ModelStateDictionary modelState)
    {
        string errors = "";

        // Running on all failed properties (e.g: ProductName, UnitPrice, ... ): 
        foreach (KeyValuePair<string, ModelStateEntry> item in modelState)
        {
            // Running on all failed attributes of item properties (e.g: Required, MaxLength...):
            foreach (ModelError err in item.Value.Errors)
                errors += err.ErrorMessage + " ";
        }

        return errors.Trim();
    }
}
