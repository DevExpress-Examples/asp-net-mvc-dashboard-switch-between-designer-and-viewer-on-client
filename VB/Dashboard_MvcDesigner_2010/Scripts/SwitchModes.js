function switchWorkingModes(s, e) {
    var workingMode = DashboardDesigner.GetWorkingMode();
    if (workingMode == 'designer') {
        DashboardDesigner.SwitchToViewer();
        SwitchWorkingModesButton.SetText('Switch to Designer');
    }
    else {
        DashboardDesigner.SwitchToDesigner();
        SwitchWorkingModesButton.SetText('Switch to Viewer');
    }
}