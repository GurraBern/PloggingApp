using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using PloggingApp.Extensions;
using PloggingApp.MVVM.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PloggingApp.MVVM.ViewModels.Popups;

public partial class BadgesPopUpViewModel : ObservableObject
{
    public IPopupService _popupService;
    public ObservableCollection<Badge> Badges { get; set; }

    public BadgesPopUpViewModel(IPopupService popupService)
    {
        _popupService = popupService;
    }
}