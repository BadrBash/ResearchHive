

namespace Model.ModelException
{
    public class ValueCannotBeNullException : Exception
    {
        public ValueCannotBeNullException(string? message) : base(message)
        {
        }
    }
}
