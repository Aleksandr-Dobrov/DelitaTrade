namespace DelitaTrade.Common.Constants
{
    public class ApplicationMessages
    {
        public class DayReportMessages
        {
            public const string UserNotFound = "Selected user not exists!";
            public const string CreateSuccess = "The daily report for {0} of {1} has been created successfully.";
            public const string DeleteSuccess = "The daily report for {0} of {1} has been deleted successfully.";
            public const string DeleteError = "An error occurred while deleting the daily report. Please contact developer team.";
            public const string ImportPaymentsSuccess = "Successfully imported {0} payments.";
            public const string NoPaymentsToImport = "No new payments to import.";
            public const string ImportPaymentsError = "An error occurred while importing payments. Please contact developer team.";
            public const string SearchComplete = "Search completed successfully. Found {0} results.";
            public const string SearchError = "An error occurred while searching. Please try again with another parameters.";
            public const string CreateError = "An error occurred while creating the daily report. Please contact developer team.";
            public const string SearchPageError = "An error occurred while loading search page. Please contact developer team.";
        }

        public class DeliveryMessages
        {
            public const string CreateSuccess = "The delivery has been created successfully.";
            public const string CreateError = "An error occurred while creating the delivery. Please contact developer team.";
            public const string DeleteSuccess = "The delivery has been deleted successfully.";
            public const string DeleteError = "An error occurred while deleting the delivery. Please contact developer team.";
            public const string CompleteSuccess = "The delivery has been completed successfully.";
            public const string CompleteError = "An error occurred while completing the delivery. Please contact developer team.";
            public const string AddInvoiceSuccess = "The invoice {0} add successfully.";
            public const string AddInvoiceError = "An error occurred while adding the invoice. Please contact developer team.";
            public const string DeliveryNotFound = "Delivery not found.";
            public const string NotImplemented = "This functionality is not implemented yet.";
        }

        public class ExpenseMessages
        {
            public const string CreateSuccess = "The expense has been created successfully.";
            public const string CreateError = "An error occurred while creating the expense. Please try again.";
            public const string DeleteSuccess = "The expense has been deleted successfully.";
            public const string DeleteError = "An error occurred while deleting the expense. Please contact developer team.";
            public const string UpdateSuccess = "The expense has been updated successfully.";
            public const string UpdateError = "An error occurred while updating the expense. Please contact developer team.";
            public const string ExpenseNotFound = "Expense not found.";
        }

        public class InvoiceMessages
        {
            public const string CompleteSuccess = "The payment by {0} has been completed successfully.";
            public const string CompleteAllSuccess = "All payments for the delivery have been completed successfully.";
            public const string CompleteError = "An error occurred while completing the payment. Please contact developer team.";
            public const string AdvancePaymentError = "An error occurred while processing the advance payment. Please contact developer team.";
            public const string AdvancePaymentSuccess = "The advance payment has been processed successfully.";
            public const string InvoiceNotFound = "Invoice not found.";
            public const string NotImplemented = "This functionality is not implemented yet.";
        }

        public class PayDeskMessages
        {
            public const string ApplyError = "An error occurred applying changes. Please contact developer team.";
            public const string ApplySuccess = "Apply success, {0} {1} {2}";
            public const string NoChange = "No change applying";
            public const string PayDeskNotFound = "An error occurred while loading pay desk. Please contact developer team.";
        }
    }
}
