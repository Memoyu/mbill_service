namespace Mbill.Core.AOP.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class MongoCollectionAttribute : Attribute
    {
        public string Name { get; set; }
    }
}
