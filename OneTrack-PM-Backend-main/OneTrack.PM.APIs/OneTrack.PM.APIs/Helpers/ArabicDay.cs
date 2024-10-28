namespace OneTrack.PM.APIs.Helpers
{
    public static class ArabicDay
    {
        public static string Get(string day)
        {
            switch (day)
            {
                case "Tuesday":
                    return "الثلاثاء";
                case "Wednesday":
                    return "الأربعاء";
                case "Thursday":
                    return "الخميس";
                case "Friday":
                    return "الجمعة";
                case "Saturday":
                    return "السبت";
                case "Sunday":
                    return "الأحد";
                case "Monday":
                    return "الأثنين";
                default:
                    return "";
            }
        }
    }
}
