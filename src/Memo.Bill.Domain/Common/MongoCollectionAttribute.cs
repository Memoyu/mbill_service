namespace Memo.Bill.Domain.Common;

[AttributeUsage(AttributeTargets.Class)]
public class MongoCollectionAttribute : Attribute
{
    public MongoCollectionAttribute(string name)
    {
        Name = name;
    }

    public string Name { get; set; }
}
