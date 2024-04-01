namespace PloggingApp.MVVM.Models.Messages;

public class AddFirstUserMessage
{
	public AddFirstUserMessage(List<PlogUser> plogUsers)
	{
		PlogUsers = plogUsers;
	}

	public List<PlogUser> PlogUsers { get; set; }
}

