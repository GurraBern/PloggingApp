namespace PloggingApp.MVVM.Models.Messages;

public class DeleteGroupMessage
{
	public DeleteGroupMessage(string id)
	{
		Id = id;
	}

	public string Id { get; set; }
}

