using PloggingApp.MVVM.Models;

namespace PloggingApp.MVVM.Models.Messages;

public class AddUserMessage
{
	public AddUserMessage(PlogUser addUser)
	{
        AddUser = addUser;
    }

    public PlogUser AddUser { get; set; }
}

