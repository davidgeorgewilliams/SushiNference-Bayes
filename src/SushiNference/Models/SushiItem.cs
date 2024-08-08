namespace SushiNference.Models
{
    public class SushiItem
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Style { get; set; } = string.Empty;
        public string MajorGroup { get; set; } = string.Empty;
        public string MinorGroup { get; set; } = string.Empty;
        public double Oiliness { get; set; }
        public bool IsEaten { get; set; }
        public double Frequency { get; set; }
    }
}