namespace PloggingApp.MVVM.Models.Messages;

public class UpdateStreakMessage
{
	public UpdateStreakMessage(int count)
	{
		Count = count;
	}

	public int Count { get; set; }
}

