namespace SpeechCodingHandler
{
    public class StringHelper
    {
        public static string GetNotEmptyValue(params string[] values)
        {
            if (values != null)
            {
                foreach(string value in values)
                {
                    if(!string.IsNullOrEmpty(value))
                    {
                        return value;
                    }
                }
            }
            return null;
        }
    }
}
