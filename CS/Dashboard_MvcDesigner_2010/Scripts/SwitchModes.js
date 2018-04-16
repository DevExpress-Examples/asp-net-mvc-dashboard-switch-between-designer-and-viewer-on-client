function switchWorkingModes(s, e) {
    var workingMode = Dashboard.GetWorkingMode();
    if (workingMode == 'designer') {
        Dashboard.SwitchToViewer();
        SwitchWorkingModesButton.SetText('Switch to Designer');
    }
    else {
        Dashboard.SwitchToDesigner();
        SwitchWorkingModesButton.SetText('Switch to Viewer');
    }
}