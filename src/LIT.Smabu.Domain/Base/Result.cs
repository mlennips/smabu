﻿
namespace LIT.Smabu.Domain.Base
{
    public sealed class Result<TValue> : Result
    {
        internal Result(TValue? value)
            : base(value)
        {
            Value = value;
        }

        private Result(ErrorDetail error)
            : base(error)
        {
            Value = default;
        }

        public new TValue? Value { get; }

        public static implicit operator Result<TValue>(TValue value)
        {
            return new(value);
        }

        public static implicit operator Result<TValue>(ErrorDetail error)
        {
            return new(error);
        }
    }

    public class Result
    {
        protected Result(object? value = null)
        {
            Value = value;
            Error = value is ErrorDetail error ? error : ErrorDetail.None;
            IsSuccess = Error == ErrorDetail.None;
        }

        public object? Value { get; }

        public ErrorDetail Error { get; }

        public bool IsSuccess { get; }

        public bool IsFailure => !IsSuccess;

        public static Result Success()
        {
            return new();
        }

        public static Result Failure(ErrorDetail error)
        {
            return new(error);
        }

        public static Result Failure(List<ErrorDetail> errors)
        {
            return new Result(new ErrorDetail(string.Join(";",
                errors.Select(e => e.Code)), string.Join(";", errors.Select(e => e.Description))));
        }

        public static Result<TValue> Success<TValue>(TValue value)
        {
            return new(value);
        }

        public static ErrorDetail Success(object create)
        {
            throw new NotImplementedException();
        }

        public static implicit operator Result(ErrorDetail error)
        {
            return new(error);
        }
    }

    public sealed record ErrorDetail(string Code, string Description)
    {
        public static readonly ErrorDetail None = new(string.Empty, string.Empty);
    }
}
