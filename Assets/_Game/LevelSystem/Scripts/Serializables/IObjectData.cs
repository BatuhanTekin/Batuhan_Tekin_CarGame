namespace _game.LevelSystem.Serializables
{
    public interface IObjectData
    {
       public ObjectData GetObjectData();

       public void SetData(string data);

       public void SetData();

       public void LoadData(string data);
    }
}