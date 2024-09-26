using KBMHttpService.Helpers.Interfaces;

namespace KBMHttpService.Helpers
{
    public class ValidationHelper : IValidationHelper
    {
        public bool IsValidGuid(string guid)
        {
            return Guid.TryParse(guid, out _);
        }
    }
}
