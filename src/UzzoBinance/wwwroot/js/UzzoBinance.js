var idInterval;

$(document).ready(function () {
    returnTableValues();
    $(".loader").hide();
    $("#pageBeginCountdown").hide();
    $("#pageBeginCountdownText").hide();
    $("#paddingBeginCountdownText").hide();
    
});

$(function () {
    $("#idbtnStart").click(function () {
        $(".loader").show();
        $("#pageBeginCountdown").show();
        $("#pageBeginCountdownText").show();
        $("#paddingBeginCountdownText").show();

        ProgressCountdown(30, 'pageBeginCountdown', 'pageBeginCountdownText');
        idInterval = setInterval(formSubmit, 30000);
    }
    );
});


$(function () {
    $("#idbtnStop").click(function () {
        stopProcess(idInterval);
        returnTableValues;
        document.getElementById("pageBeginCountdown").value = 30;
        document.getElementById("pageBeginCountdownText").textContent = 30;

        $(".loader").hide();
        $("#pageBeginCountdown").hide();
        $("#pageBeginCountdownText").hide();
        $("#paddingBeginCountdownText").hide();
        
    }
    );
});

function returnTableValues() {
    $(function () {
        $.ajax({
            type: 'GET',
            url: "/UzzoBinance/UzzoBinancePartialView",
            success: function (data) {
                $("#idDivTablePrice").html(data);
            },
            error: function (jXHR, textStatus, errorThrown) {
                console.log(errorThrown);
            }
        });

    });
}

function formSubmit() {
    $(function () {
        $.ajax({
            type: 'POST',
            success: function (data) {
                returnTableValues();
                document.getElementById("pageBeginCountdown").value = 30;
                document.getElementById("pageBeginCountdownText").textContent = 30;
                ProgressCountdown(30, 'pageBeginCountdown', 'pageBeginCountdownText');
            },
            error: function (jXHR, textStatus, errorThrown) {
                console.log(errorThrown);
            }
        });

    });

}

function ProgressCountdown(timeleft, bar, text) {
    return new Promise((resolve, reject) => {
        var countdownTimer = setInterval(() => {
            timeleft--;

            document.getElementById(bar).value = timeleft;
            document.getElementById(text).textContent = timeleft;

            if (timeleft <= 0) {
                clearInterval(countdownTimer);
                resolve(true);
            }
        }, 1000);
    });
}

function stopProcess(idInterval) {
    clearInterval(idInterval);
}