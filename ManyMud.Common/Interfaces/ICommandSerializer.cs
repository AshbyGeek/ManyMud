namespace ManyMud.Common.Interfaces
{ 
    public interface ICommandSerializer
    {
        object Deserialize(string json);

        string Serialize(object obj);
    }
}