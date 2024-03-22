using Microsoft.Maui.Maps;

namespace PloggingApp.MVVM.Models;
public class Badge
{
    public string Type { get; set; }   /*Distance, TrashInKilos, TimeSpent,  */
    public int Level { get; set; }          /*lvl 0,1,2,3, represents nothing, Bronze, Silver, Gold */
    public ImageSource Image { get; set; }
    public DateTime AcquiredDate { get; set; }
    public double Threshold { get; set; }

}


public class TrashCollectedBadge : Badge
{
    public double trashcollected { get; set; }
    public TrashCollectedBadge(PloggingStatistics stats)
    {
        trashcollected = stats.TotalWeight;
        Threshold = 5;
        if (trashcollected - Threshold > 10) { Image = ImageSource.FromFile("weightbadgegold.png"); Level = 3; }
        else if (trashcollected - Threshold > 5) { Image = ImageSource.FromFile("weightbadgesilver.png"); Level = 2; }
        else if (trashcollected - Threshold > 0) { Image = ImageSource.FromFile("weightbadgebronze.png"); Level = 1; }
        else { Image = ImageSource.FromFile("weightbadge.png"); Level = 0; }

    }
}

public class TimeSpentBadge : Badge
{
    public double hours = 0;
    public TimeSpentBadge(PloggingStatistics stats)
    {
        // hours = stats.TotalTime;
        Threshold = 5;
        if (hours - Threshold > 10) { Image = ImageSource.FromFile("timespentbadgegold.png"); Level = 3; }
        else if (hours - Threshold > 5) { Image = ImageSource.FromFile("timespentbadgesilver.png"); Level = 2; }
        else if (hours - Threshold > 0) { Image = ImageSource.FromFile("timespentbadgebronze.png"); Level = 1; }
        else { Image = ImageSource.FromFile("timespentbadge.png"); Level = 0; }


    }
}

public class DistanceBadge : Badge
{
    public double Distance { get; set; }
    public DistanceBadge(PloggingStatistics stats)
    {
        Distance = stats.TotalDistance;
        Threshold = 5;
        if (Distance - Threshold > 10) { Image = ImageSource.FromFile("distancebadgegold.png"); Level = 3; }
        else if (Distance - Threshold > 5) { Image = ImageSource.FromFile("distancebadgesilver.png"); Level = 2; }
        else if (Distance - Threshold > 0) { Image = ImageSource.FromFile("distancebadgebronze.png"); Level = 1; }
        else { Image = ImageSource.FromFile("distancebadge.png"); Level = 0; }

    }

}
