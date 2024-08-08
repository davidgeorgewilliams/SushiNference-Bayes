namespace SushiNference.Models
{
    public class SushiItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Style { get; set; }
        public string MajorGroup { get; set; }
        public string MinorGroup { get; set; }
        public double Oiliness { get; set; }
        public bool IsEaten { get; set; }
        public double Frequency { get; set; }
    }
}