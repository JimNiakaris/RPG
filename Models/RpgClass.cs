using System.Text.Json.Serialization;

namespace DotNet_RPG.Models
{
	[JsonConverter(typeof(JsonStringEnumConverter))] // use this to see the actuall names of the rpg class instead of the numbers of the enum
	public enum Rpgclass
	{
		Warrior = 1,
		Rogue = 2,
		Mage=3
	}
}
