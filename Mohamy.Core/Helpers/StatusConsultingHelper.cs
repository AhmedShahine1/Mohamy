using System.Collections.Generic;

namespace Mohamy.Core.Helpers
{
    public static class StatusConsultingHelper
    {
        public static readonly Dictionary<statusConsulting, string> StatusConsultingTranslations = new Dictionary<statusConsulting, string>
        {
            { statusConsulting.UserRequestedNotPaid, "تم طلب الاستشارة ولم يتم الدفع بعد" },
            { statusConsulting.InProgress, "قيد التنفيذ" },
            { statusConsulting.Completed, "تم الانتهاء" },
            { statusConsulting.Cancelled, "تم الإلغاء" }
        };

        public static string GetArabicTranslation(statusConsulting status)
        {
            return StatusConsultingTranslations.ContainsKey(status) ? StatusConsultingTranslations[status] : status.ToString();
        }
    }
}
