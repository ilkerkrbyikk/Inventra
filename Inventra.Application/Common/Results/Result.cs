namespace Inventra.Application.Common.Results
{
    public class Result
    {
        public bool IsSuccess { get; }
        public string Message { get; }
        public IReadOnlyList<string> Errors { get; }

        protected Result(bool isSuccess, string message, IReadOnlyList<string> errors)
        {
            IsSuccess = isSuccess;
            Message = message;
            Errors = errors;
        }

        public static Result Success(string message = "Operation completed successfully.")
            => new(true, message, []);

        public static Result Failure(string error)
            => new(false, string.Empty, [error]);

        public static Result Failure(IEnumerable<string> errors)
            => new(false, string.Empty, errors.ToList());

        public static Result<TData> Success<TData>(TData data, string message = "Operation completed successfully.")
            => new(true, data, message, []);

        public static Result<TData> Failure<TData>(string error)
            => new(false, default!, string.Empty, [error]);

        public static Result<TData> Failure<TData>(IEnumerable<string> errors)
            => new(false, default!, string.Empty, errors.ToList());
    }

    public class Result<TData> : Result
    {
        public TData? Data { get; }

        protected internal Result(bool isSuccess, TData? data, string message, IReadOnlyList<string> errors)
            : base(isSuccess, message, errors)
        {
            Data = data;
        }
    }
}