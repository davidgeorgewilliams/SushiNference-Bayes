namespace SushiNference.Models
{
    public class UserPreference
    {
        public int UserId { get; set; }
        public int[] Ratings { get; set; } = Array.Empty<int>();
    }
}