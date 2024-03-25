using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using PloggingApp.MVVM.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PloggingApp.MVVM.ViewModels;

public partial class BadgesPopUpViewModel : ObservableObject
{
    private readonly IPopupService _popupService;
    public ObservableCollection<Badge> Badges { get; set; }

    public BadgesPopUpViewModel(IPopupService popupService)
    {
        _popupService = popupService;
        Badges = new ObservableCollection<Badge>(); 
    }
}