
using CommunityToolkit.Mvvm.ComponentModel;


namespace PloggingApp.MVVM.Models;
public class Badge : ObservableObject
{
    public  string Type { get; set; } 
    public  string Measurement { get; set; }
    public  Levels Level { get; set; }  
    public  ImageSource Image { get; set; }
    public  double ToNextLevel { get; set; } 
    public double progression { get; set; }
    public double percentage { get; set; }
    public string color { get; set; }
    public void createBadge(double progress, string png, double th1, double th2, double th3)
    {
        if (progress >= th3)
        {
            Image = ImageSource.FromFile(png + "badgegold.png");
            Level = Levels.Gold;
            percentage = 100; 
            color = "Gold";
        }
        else if (progress >= th2)
        {
            Image = ImageSource.FromFile(png + "badgesilver.png");
            Level = Levels.Silver;
            ToNextLevel = Math.Round(th3 - progress,1);
            percentage = progress / th3;
            color = "Gold";
        }
        else if (progress >= th1)
        {
            Image = ImageSource.FromFile(png + "badgebronze.png");
            Level = Levels.Bronze;
            ToNextLevel = Math.Round(th2 - progress,1);
            percentage = progress / th2;
            color = "#566470";
        }
        else
        {
            Image = ImageSource.FromFile(png + "badge.png");
            Level = Levels.Locked;
            ToNextLevel = Math.Round(th1 - progress,1);
            percentage = progress / th1;
            color = "#CD7F32";
        }
    }
}

public enum Levels
{
    Locked,
    Bronze,
    Silver,
    Gold

}


public class TrashCollectedBadge : Badge
{
    public TrashCollectedBadge(PloggingStatistics stats)
    {
        Measurement = "kilogram(s)";
        Type = "Trash Weight";
        progression = stats.TotalWeight;
        createBadge(progression, "weight", 5, 10, 15);


    }
}

public class TimeSpentBadge : Badge
{
    public TimeSpentBadge(PloggingStatistics stats)
    {
        Measurement = "hour(s)";
        Type = "Time Spent";
        progression = stats.TotalTime.TotalHours;
        progression = 0;
        createBadge(progression, "timespent", 5, 10, 15);


    }
}

public class DistanceBadge : Badge
{
    public double Distance { get; set; }
    public DistanceBadge(PloggingStatistics stats)
    {
        Measurement = "kilometre(s)";
        Type = "Distance Traveled";
        progression = stats.TotalDistance/1000;
        createBadge(progression, "distance", 5, 10, 15);

    }

}


public class CO2Badge : Badge
{
    public CO2Badge(PloggingStatistics stats)
    {
        Measurement = "kilograms(s)";
        Type = "CO2 Saved";
        progression = stats.TotalCO2Saved;
        createBadge(progression, "co2", 5, 10, 15);


    }


}

public class StreakBadge : Badge
{
    public StreakBadge(int streak)
    {
        Measurement = "week(s)";
        Type = "Weekly Streak";
        progression = streak;
        createBadge(progression, "streak", 5, 10, 15);
    }


}

