using Microsoft.Maui.Controls.Platform.Compatibility;
using Microsoft.Maui.Maps;
using System.Security.Cryptography.X509Certificates;

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
        trashcollected = stats.totalWeight;
        Threshold = 5;
        if (trashcollected - Threshold > 0) { Image = ImageSource.FromFile("weightbadgebronze.png"); Level = 1; }
        else if (trashcollected - Threshold > 5) { Image = ImageSource.FromFile("weightbadgesilver.png"); Level = 2; }
        else if (trashcollected - Threshold > 10) { Image = ImageSource.FromFile("weightbadgegold.png"); Level = 3; }
        else { Image = ImageSource.FromFile("weightbadge.png"); Level = 0; }

    }
}

public class TimeSpentBadge : Badge
{
    public TimeSpentBadge()
    {
        if (Level == 1) { Image = "distancebadgebronze.svg"; }
        if (Level == 2) { Image = "distancebadgesilver.svg"; }
        if (Level == 3) { Image = "distancebadgegold.svg"; }

    }
}

public class DistanceBadge : Badge
{
    public double Distance { get; set; }
    public DistanceBadge(PloggingStatistics stats)
    {
        double Distance = stats.totalDistance;
        Threshold = 5;
        if (Distance - Threshold > 0) { Image = ImageSource.FromFile("distancebadgebronze.png"); Level = 1; }
        else if (Distance - Threshold > 5) { Image = ImageSource.FromFile("distancebadgesilver.png"); Level = 2; }
        else if (Distance - Threshold > 10) { Image = ImageSource.FromFile("distancebadgegold.png"); Level = 3; }
        else { Image = ImageSource.FromFile("distancebadge.png"); Level = 0; }

    }

}