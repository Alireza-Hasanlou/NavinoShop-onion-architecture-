namespace Shared.Application
{
    public class OperationResultOrderDiscount
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string Title { get; set; }
        public int Id { get; set; }
        public int Percent { get; set; }
        public OperationResultOrderDiscount(bool success, string? message = "", string? title = "", int id = 0, int percent = 0)
        {
            Success = success;
            Message = message;
            Id = id;
            Percent = percent;
            Title = title;
        }
    }
    public class OperationResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string ModelName { get; set; }
        public object? Data { get; set; }
        public OperationResult(bool success, string? message = "", string? modelName = "", object? data = null)
        {

            Success = success;
            Message = message;
            ModelName = modelName;
            Data = data;
        }
    }
}
