namespace Project.Services
{
    public class BusinessServiceValidationResult
    {
        public string MemberName { get; set; }

        public string Message { get; set; }

        public BusinessServiceValidationResult()
        {
        }

        public BusinessServiceValidationResult(string memberName, string message)
        {
            MemberName = memberName;
            Message = message;
        }

        public BusinessServiceValidationResult(string message)
        {
            Message = message;
        }
    }
}
