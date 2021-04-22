

window.onload = function () {
    window.setTimeout(function () {
        $(".alert").css("display", "none");
    }, 3500);

    function collapseToggle(btnId) {
        var x = $(btnId).attr("href");
        $(".collapse " + x).collapse();
        $(".collapse:not(" + x + ")").collapse('hide');

    }

    $("#comp-btn").click(function () {
        collapseToggle("#comp-btn");

        if ($("#comp-btn").hasClass("btn-primary")) {
            $("#comp-btn").addClass("btn-info").removeClass("btn-primary");
            $("#user-btn").addClass("btn-primary").removeClass("btn-info");
        }
        else {
            $("#comp-btn").addClass("btn-primary").removeClass("btn-info");
        }
    });

    $("#user-btn").click(function () {
        collapseToggle("#user-btn");

        if ($("#user-btn").hasClass("btn-primary")) {
            $("#user-btn").addClass("btn-info").removeClass("btn-primary");
            $("#comp-btn").addClass("btn-primary").removeClass("btn-info");
        }
        else {
            $("#user-btn").addClass("btn-primary").removeClass("btn-info");
        }
    });
}