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

        foreach (KeyValuePair<string, ModelStateEntry> item in modelState)
        {
            foreach (ModelError err in item.Value.Errors)
                errors += err.ErrorMessage + " ";
        }

        return errors.Trim();
    }
}
