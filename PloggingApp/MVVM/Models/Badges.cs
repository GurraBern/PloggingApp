
using RestSharp.Authenticators;

namespace PloggingApp.MVVM.Models;
public class Badge
{
    public string Type { get; set; }   /*Distance, TrashInKilos, TimeSpent,  */

    public string Measurement { get; set; }
    public string Level { get; set; }          /*lvl 0,1,2,3, represents nothing, Bronze, Silver, Gold */
    public ImageSource Image { get; set; }
    public DateTime AcquiredDate { get; set; }
    public double Threshold { get; set; }
    public double ToNextLevel { get; set; } /* A double dispalying how much left to reach next level */

    public double progression { get; set; }

    public void createBadge(double progress, string png, double th1, double th2, double th3)
    {
        if (progress >= th3)
        {
            Image = ImageSource.FromFile(png + "badgegold.png");
            Level = "Gold";
        }
        else if (progress >= th2)
        {
            Image = ImageSource.FromFile(png + "badgesilver.png");
            Level = "Silver";
            ToNextLevel = th3 - progress;
        }
        else if (progress >= th1)
        {
            Image = ImageSource.FromFile(png + "badgebronze.png");
            Level = "Bronze";
            ToNextLevel = th2 - progress;
        }
        else
        {
            Image = ImageSource.FromFile(png + "badge.png");
            Level = "null";
            ToNextLevel = th1 - progress;
        }

        
    }
}


public class TrashCollectedBadge : Badge
{
    public TrashCollectedBadge(PloggingStatistics stats)
    {
        Measurement = "kilogram(s)";
        Type = "Trash Weight Badge";
        progression = stats.TotalWeight;
        createBadge(progression, "weight", 5, 10, 15);


    }
}

public class TimeSpentBadge : Badge
{
    public TimeSpentBadge(PloggingStatistics stats)
    {
        Measurement = "hour(s)";
        Type = "Time Spent Badge";
        // progression = stats.TotalTime;
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
        Type = "Distance Traveled Badge";
        progression = stats.TotalDistance;
        createBadge(progression, "distance", 5, 10, 1500000);

    }

}


public class CO2Badge : Badge
{
    public double CO2 { get; set; }
    public CO2Badge(PloggingStatistics stats)
    {
        Measurement = "kilograms(s)";
        Type = "CO2 Saved Badge";
        progression = stats.totalCO2Saved;
        createBadge(progression, "co2", 5, 10, 15);


    }


}

