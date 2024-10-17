namespace DotNet_RPG.Models
{
    public class Character
    {
        public int Id { get; set; }
        public string Name { get; set; } = "Bogort";
        public int HitPoints { get; set; } = 10;
        public int Strenght { get; set; } = 10;
        public int Deffence { get; set; } = 10;
        public int Intelligence { get; set; } = 10;
        public Rpgclass Class { get; set; } = Rpgclass.Warrior;

    }
}
