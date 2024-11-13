namespace LIT.Smabu.Domain.Base
{
    public interface IHasBusinessNumber<T>
        where T : BusinessNumber
    {
        T Number { get; }
    }
}
