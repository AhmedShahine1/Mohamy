namespace Mohamy.Core.Helpers
{
    public enum LawyerRegistrationStatus
    {
        // For other types of users
        NotLawyer = 0,

        // New Lawyer request received
        RequestReceived = 1,

        // License and other registration data approved
        LicenseApproved = 2 ,

        // Details added by lawyer but not approved yet
        DetailSibmitted = 3,

        // Account approved
        Approved = 4

    }
}
