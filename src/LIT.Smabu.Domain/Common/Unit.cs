using LIT.Smabu.Domain.Base;

namespace LIT.Smabu.Domain.Common
{
    public record Unit(string Value) : EnumValueObject(Value)
    {
        public override string Name => Value switch
        {
            "None" => "",
            "Hour" => "Stunde",
            "Day" => "Tag",
            "Item" => "Stück",
            "Project" => "Projekt",
            _ => "???"
        };

        public override string ShortName => Value switch
        {
            "None" => "",
            "Hour" => "Std",
            "Day" => "Tag",
            "Item" => "Stk",
            "Project" => "Prj",
            _ => "???"
        };

        public static readonly Unit None = new("None");
        public static readonly Unit Hour = new("Hour");
        public static readonly Unit Day = new("Day");
        public static readonly Unit Item = new("Item");
        public static readonly Unit Project = new("Project");

        public static Unit[] GetAll()
        {
            return [None, Hour, Day, Item, Project];
        }
    }
}
