namespace OneTrack.PM.APIs.Helpers
{
    public static class DaysOrder
    {
        public static int Get(string day)
        {
            switch (day)
            {
                case "Tuesday":
                    return 4;
                case "Wednesday":
                    return 5;
                case "Thursday":
                    return 6;
                case "Friday":
                    return 7;
                case "Saturday":
                    return 1;
                case "Sunday":
                    return 2;
                case "Monday":
                    return 3;
                default:
                    return 0;
            }
        }
    }
}
