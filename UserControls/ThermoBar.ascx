<%@ control language="c#" inherits="ArenaWeb.UserControls.Custom.MITM.ThermoBar" CodeFile="ThermoBar.ascx.cs" CodeBehind="ThermoBar.ascx.cs" %>
<script src="https://ajax.googleapis.com/ajax/libs/jquery/1.4.3/jquery.min.js" type="text/javascript"></script>

<script type="text/javascript">
var barvalue = 0, barchange = 1;

function setThermoLevel(percent) {
    var totalHeight = $('#thermoBackground').height();
    var newHeight = totalHeight - (totalHeight * (percent / 100));
    
    $('#thermoContent').css('top', newHeight + 'px');
    $('#thermoFill').css('top', '-' + newHeight + 'px');
}

function updateLevel() {
    barvalue += barchange;
    
    if (barvalue == 100) {
        barchange = -1;
    }
    else if (barvalue == 0) {
        barchange = 1;
    }
    
    setThermoLevel(barvalue);
}

$(document).ready(function() {
    setInterval(updateLevel, 25);
});
</script>

<style type="text/css">
#thermoBackground {
    display: inline-block;
    position: relative;
    background-image: url(Custom/MiTM/Images/thermoEmpty.png);
    width: 120px;
    height: 400px;
}

#thermoContent {
    display: inline-blck;
    position: absolute;
    left: 0px;
    top: 400px; /* Initial value of 400px so that by default the thermo is empty. */
    right: 0px;
    bottom: 0px;
    overflow: hidden;
}

#thermoFill {
    display: inline-block;
    position: relative;
    width: 120px;
    height: 400px;
    background-image: url(Custom/MiTM/Images/thermoFull.png);
}
</style>


<span id="thermoBackground">
    <span id="thermoContent">
        <span id="thermoFill"></span>
    </span>
</span>
