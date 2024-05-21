namespace Wave.Commerce.Domain.Shared;

public class Result
{
    protected Result(bool isSucess, Error error)
    {
        if (isSucess && error != Error.None ||
            !isSucess && error == Error.None)
        {
            throw new InvalidOperationException();
        }

        IsSuccess = isSucess;
        Error = error;
    }

    public bool IsSuccess { get; }
    public Error Error { get; }

    public static Result Success() => new(true, Error.None);
    public static Result WithError(Error error) => new(false, error);
    public static Result<TValue> Success<TValue>(TValue? value) => new(value, true, Error.None);
    public static Result<TValue> WithError<TValue>(Error error) => new(default, false, error);

    public static implicit operator Result(Error error) => WithError(error);
}

public class Result<TValue> : Result
{
    private readonly TValue? _value;

    protected internal Result(TValue? value, bool isSuccess, Error error)
        : base(isSuccess, error)
    {
        _value = value;
    }

    public TValue Value => IsSuccess
        ? _value!
        : throw new InvalidOperationException("The value of a failure result can not be accessed.");

    public bool HasValue() => _value != null;

    public static implicit operator Result<TValue>(TValue? value) => Success(value);
    public static implicit operator Result<TValue>(Error error) => WithError<TValue>(error);
}
