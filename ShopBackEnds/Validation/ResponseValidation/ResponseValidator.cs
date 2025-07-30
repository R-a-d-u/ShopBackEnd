namespace ShopBackEnd.Validators.ResponseValidator
{
    public class ResponseValidator<T>
    {
        public bool IsSuccess { get; private set; }
        public T? Result { get; private set; }
        public string? ErrorMessage { get; private set; }

        private ResponseValidator(bool isSuccess, T? result, string? errorMessage)
        {
            IsSuccess = isSuccess;
            Result = result;
            ErrorMessage = errorMessage;
        }

        public static ResponseValidator<T> Success(T result)
        {
            return new ResponseValidator<T>(true, result, null);
        }

        public static ResponseValidator<T> Failure(string errorMessage)
        {
            return new ResponseValidator<T>(false, default, errorMessage);
        }

        public void Handle(Action<T> onSuccess, Action<string> onFailure)
        {
            if (IsSuccess && Result != null)
            {
                onSuccess?.Invoke(Result);
            }
            else
            {
                onFailure?.Invoke(ErrorMessage ?? "An unknown error occurred.");
            }
        }
    }
}