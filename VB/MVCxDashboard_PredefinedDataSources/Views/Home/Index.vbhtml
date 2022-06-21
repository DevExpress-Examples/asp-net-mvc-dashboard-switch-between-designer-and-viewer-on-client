<div id="#Button1 " style="position: absolute; left: 5px; right: 0; top:5px; bottom:40px;">
    @Html.DevExpress().Button(Sub(settings)
                                   settings.Name = "btnSwitchMode"
                                   settings.Text = "Switch to Viewer"
                                   settings.UseSubmitBehavior = True
                                   settings.ClientSideEvents.Click = "function(s) { switchMode(s); }"
                               End Sub).GetHtml()
</div>
<div id="#Dashboard" style="position: absolute; left: 0; right: 0; top:40px; bottom:0;">
    @Html.DevExpress().Dashboard(Sub(settings)
                                      settings.Name = "webDashboard"
                                      settings.ControllerName = "DefaultDashboard"
                                      settings.Width = Unit.Percentage(100)
                                      settings.Height = Unit.Percentage(100)
                                  End Sub).GetHtml()
</div>