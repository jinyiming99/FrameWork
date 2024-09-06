namespace GameFrameWork
{
    public class Cron
    {
        private int[] seconds = new int[60];
        private int[] minutes = new int[60];
        private int[] hours = new int[24];
        private int[] days = new int[31];
        private int[] month = new int[12];
        private int[] weeks = new int[7];
        //2019-2099年
        private int[] year = new int[80];
 
        public int[] Seconds { get => seconds; set => seconds = value; }
        public int[] Minutes { get => minutes; set => minutes = value; }
        public int[] Hours { get => hours; set => hours = value; }
        public int[] Days { get => days; set => days = value; }
        public int[] Month { get => month; set => month = value; }
        public int[] Weeks { get => weeks; set => weeks = value; }
        public int[] Year { get => year; set => year = value; }
 
        public Cron()
        {
            for (int i = 0; i < 60; i++)
            {
                seconds[i] = 0;
                minutes[i] = 0;
            }
            for (int i = 0; i < 24; i++)
            {
                hours[i] = 0;
            }
            for (int i = 0; i < 31; i++)
            {
                days[i] = 0;
            }
            for (int i = 0; i < 12; i++)
            {
                month[i] = 0;
            }
            for (int i = 0; i < 7; i++)
            {
                weeks[i] = 0;
            }
            for (int i = 0; i < 80; i++)
            {
                year[i] = 0;
            }
        }
 
        public void Init()
        {
            for (int i = 0; i < 7; i++)
            {
                weeks[i] = 0;
            }
            for (int i = 0; i < 31; i++)
            {
                days[i] = 0;
            }
        }
    }
}