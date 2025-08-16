

namespace Utility.Shared.Application
{
    public class OperationResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string ModelName { get; set; }
        public OperationResult(bool success, string? message = "", string? modelName = "")
        {
            Success = success;
            Message = message;
            ModelName = modelName;
        }
    }
}
