using System.Collections.Generic;

namespace Mohamy.Core.Helpers
{
    public static class StatusRequestConsultingHelper
    {
        public static readonly Dictionary<statusRequestConsulting, string> StatusConsultingTranslations = new Dictionary<statusRequestConsulting, string>
        {
            { statusRequestConsulting.Approved, "تم الموافقه علي طلبك" },
            { statusRequestConsulting.Rejection, "تم رفض طلبك" },
            { statusRequestConsulting.Waiting, "في انتظار الرد" },
            { statusRequestConsulting.Cancel, "تم الإلغاء" }
        };

        public static string GetArabicTranslation(statusRequestConsulting status)
        {
            return StatusConsultingTranslations.ContainsKey(status) ? StatusConsultingTranslations[status] : status.ToString();
        }
    }
}
