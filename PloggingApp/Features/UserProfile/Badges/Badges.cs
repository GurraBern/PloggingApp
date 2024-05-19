using CommunityToolkit.Mvvm.ComponentModel;

namespace PloggingApp.MVVM.Models;

public class Badge : ObservableObject
{
    public  string Type { get; set; } 
    public  string Measurement { get; set; }
    public  Levels Level { get; set; }  
    public  ImageSource Image { get; set; }
    public  double ToNextLevel { get; set; } 
    public double Progression { get; set; }
    public double Percentage { get; set; }
    public string Color { get; set; }

    public void CreateBadge(double progress, string png, double th1, double th2, double th3)
    {
        if (progress >= th3)
        {
            Image = ImageSource.FromFile(png + "badgegold.png");
            Level = Levels.Gold;
            Percentage = 100; 
            Color = "Gold";
        }
        else if (progress >= th2)
        {
            Image = ImageSource.FromFile(png + "badgesilver.png");
            Level = Levels.Silver;
            ToNextLevel = Math.Round(th3 - progress,1);
            Percentage = progress / th3;
            Color = "Gold";
        }
        else if (progress >= th1)
        {
            Image = ImageSource.FromFile(png + "badgebronze.png");
            Level = Levels.Bronze;
            ToNextLevel = Math.Round(th2 - progress,1);
            Percentage = progress / th2;
            Color = "#566470";
        }
        else
        {
            Image = ImageSource.FromFile(png + "badge.png");
            Level = Levels.Locked;
            ToNextLevel = Math.Round(th1 - progress,1);
            Percentage = progress / th1;
            Color = "#CD7F32";
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
        Progression = stats.TotalWeight;
        CreateBadge(Progression, "weight", 5, 10, 15);
    }
}

public class TimeSpentBadge : Badge
{
    public TimeSpentBadge(PloggingStatistics stats)
    {
        Measurement = "hour(s)";
        Type = "Time Spent";
        Progression = stats.TotalTime.TotalHours;
        Progression = 0;
        CreateBadge(Progression, "timespent", 5, 10, 15);
    }
}

public class DistanceBadge : Badge
{
    public double Distance { get; set; }
    public DistanceBadge(PloggingStatistics stats)
    {
        Measurement = "kilometre(s)";
        Type = "Distance Traveled";
        Progression = stats.TotalDistance/1000;
        CreateBadge(Progression, "distance", 5, 10, 15);
    }
}

public class CO2Badge : Badge
{
    public CO2Badge(PloggingStatistics stats)
    {
        Measurement = "kilograms(s)";
        Type = "CO2 Saved";
        Progression = stats.TotalCO2Saved;
        CreateBadge(Progression, "co2", 5, 10, 15);
    }
}

public class StreakBadge : Badge
{
    public StreakBadge(int streak)
    {
        Measurement = "week(s)";
        Type = "Weekly Streak";
        Progression = streak;
        CreateBadge(Progression, "streak", 5, 10, 15);
    }
}

