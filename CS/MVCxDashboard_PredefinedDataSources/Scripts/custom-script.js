function switchMode(sender) {
    var control = webDashboard.GetDashboardControl();

    if (control.isDesignMode()) {
        control.switchToViewer();
        btnSwitchMode.SetText('Switch to Designer');
    }
    else {
        control.switchToDesigner();
        btnSwitchMode.SetText('Switch to Viewer');
    }
}