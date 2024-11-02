
namespace StudentRegistrationProgram
{
    public class InputChecker
    {
        public ErrorType IsValidSingleNumber(string number)
        {
            if (!int.TryParse(Console.ReadLine(), out int numberAsInt))
            {
                return ErrorType.MustBeInt;
            }

            if (number == null)
                return ErrorType.StringIsNull;
            if (number.Length < 1)
                return ErrorType.StringTooShort;
            else
                return ErrorType.NoError;
        }
        public ErrorType CheckName(string name)
        {
            if(name == null)
                return ErrorType.StringIsNull;
            if (name.Length <= 1)
                return ErrorType.StringTooShort;
            else
                return ErrorType.NoError;
        }

        public ErrorType CheckCity(string city)
        {
            if (city == null)
                return ErrorType.StringIsNull;
            if (city.Length <= 1)
                return ErrorType.StringTooShort;
            else
                return ErrorType.NoError;
        }
    }
}
