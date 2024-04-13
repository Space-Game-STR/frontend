using UnityEngine;

[System.Serializable]
public class Command
{
    public TCPCommand command;
    public ObjectType objectType;
    public Data data;
    public Command(Data data, ObjectType objectType, TCPCommand command)
    {
        this.data = data;
        this.objectType = objectType;
        this.command = command;
    }

    public Command()
    {
        data = new Data();
        objectType = ObjectType.None;
        command = TCPCommand.None;
    }

    public string getString()
    {

        string json = data.toString();

        return command.ToString() + " " + objectType.ToString() + " " + json;
    }
}

[System.Serializable]
public class Data
{
    public string @object;
    public Options options;

    public Data(string @object, Options options)
    {
        this.@object = @object;
        this.options = options;
    }

    public Data(string @object)
    {
        this.@object = @object;
        options = new Options();
    }

    public Data(Options options)
    {
        @object = JsonUtility.ToJson(new Empty());
        this.options = options;
    }

    public Data()
    {
        @object = JsonUtility.ToJson(new Empty());
        options = new Options(false, false);
    }

    public string toString()
    {
        return JsonUtility.ToJson(this);
    }
}

[System.Serializable]
public class Options
{
    public bool all;
    public bool fromPlanet;

    public Options(bool all, bool fromPlanet)
    {
        this.all = all;
        this.fromPlanet = fromPlanet;
    }

    public Options()
    {
        all = false;
        fromPlanet = false;
    }
}

public enum ObjectType
{
    celestials,
    spaceships,
    journeys,
    None
}

public enum TCPCommand
{
    get,
    create,
    update,
    None
}

class Empty
{

}

[System.Serializable]
public class Response<T>
{
    public int status;
    public string statusMessage;
    public T data;
}