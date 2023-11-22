namespace Student_Planner.Exceptions
{
    public class CharacterException : ArgumentException
    {
        public CharacterException(string message) : base(message)
        {
        }

        public CharacterException(string message, string invalidCharacter) : base(message, invalidCharacter)
        {
        }

        public CharacterException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public CharacterException(string message, string invalidCharacter, Exception innerException) : base(message, invalidCharacter, innerException)
        {
        }
    }
}