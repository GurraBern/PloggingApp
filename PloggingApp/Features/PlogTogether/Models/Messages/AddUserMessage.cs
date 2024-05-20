namespace PloggingApp.Features.Plogtogether;

public class AddUserMessage
{
	public AddUserMessage(PlogUser addUser)
	{
        AddUser = addUser;
    }

    public PlogUser AddUser { get; set; }
}

