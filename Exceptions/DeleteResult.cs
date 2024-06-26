namespace FinancialManagementApp.Exceptions
{
    public class DeleteAccountResult
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
    }
}