using LIT.Smabu.Domain.Base;

namespace LIT.Smabu.Domain.Common
{
    public record Unit(string Value) : SimpleValueObject<string>(Value)
    {
        public static Unit None => new("");
        public static Unit Hour => new("Hour");
        public static Unit Day => new("Day");
        public static Unit Item => new("Item");
        public static Unit Project => new("Project");

        public string Name => Value switch
        {
            "" => "",
            "Hour" => "Stunde",
            "Day" => "Tag",
            "Item" => "Stück",
            "Project" => "Projekt",
            _ => "???"
        };

        public string ShortName => Value switch
        {
            "" => "",
            "Hour" => "Std",
            "Day" => "Tag",
            "Item" => "Stk",
            "Project" => "Prj",
            _ => "???"
        };

        public static Unit[] GetAll()
        {
            return [None, Hour, Day, Item, Project];
        }
    }
}
