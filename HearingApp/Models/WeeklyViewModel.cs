using System;
using System.Collections.Generic;
namespace HearingApp.Models;

public class WeeklyViewModel
{

    public int WeekNumber{get; set;}
    public double AverageSeverity {get; set;}

    public double AverageCoping {get; set;}

    public List<TinnitusScore> Entries {get; set;}

    


}
