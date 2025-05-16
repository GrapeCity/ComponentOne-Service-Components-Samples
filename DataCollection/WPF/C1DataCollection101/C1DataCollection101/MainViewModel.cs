using System.Collections.Generic;

namespace C1DataCollection101;

public class MainViewModel
{

    public List<SampleViewModel> Samples { get; set; } = new List<SampleViewModel>
    {
        new SampleViewModel{ Name = Resources.SortingLabel, Uri = "Sorting.xaml" },
        new SampleViewModel{ Name = Resources.FilteringLabel, Uri = "Filtering.xaml" },
        new SampleViewModel{ Name = Resources.GroupingLabel, Uri = "Grouping.xaml" },
        new SampleViewModel{ Name = Resources.OnDemandLabel, Uri = "OnDemand.xaml" },
        new SampleViewModel{ Name = Resources.VirtualModeLabel, Uri = "VirtualMode.xaml" },
    };
}

public class SampleViewModel
{
    public string Name { get; set; }
    public string Uri { get; set; }
}
