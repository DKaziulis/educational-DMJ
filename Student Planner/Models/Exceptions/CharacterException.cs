namespace Student_Planner.Models.Exceptions
{
    public class CharacterException : ArgumentException
    {
        public CharacterException(string message) : base(message)
        {
        }

        public CharacterException(string message, string paramName) : base(message, paramName)
        {
        }

        public CharacterException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public CharacterException(string message, string paramName, Exception innerException) : base(message, paramName, innerException)
        {
        }
    }
}