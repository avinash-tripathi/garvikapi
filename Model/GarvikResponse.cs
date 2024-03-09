using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GARVIKService.Model
{
    public enum OperationStatus
    {
        Success,
        Inserted,
        Deleted,
        Updated,
        Error
    }
    // Override ToString method for OperationStatus enum
    public static class OperationStatusExtensions
    {
        public static string ToText(this OperationStatus status)
        {
            switch (status)
            {
                case OperationStatus.Success:
                    return "Success";
                case OperationStatus.Inserted:
                    return "Inserted";
                case OperationStatus.Deleted:
                    return "Deleted";
                case OperationStatus.Updated:
                    return "Updated";
                case OperationStatus.Error:
                    return "Error";
                default:
                    throw new ArgumentOutOfRangeException(nameof(status), status, "Invalid OperationStatus value.");
            }
        }
    }

    public class GarvikResponse
    {
        public String result { get; set; }
        public object obj { get; set; }
        public string message { get; set; }
        public string innermessage { get; set; }
        public string code { get; set; }
        public string token { get; set; }

    }
}
