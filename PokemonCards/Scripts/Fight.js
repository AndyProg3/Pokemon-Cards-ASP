
window.onload = function () {
    function loadFightFromTemplate() {
        var fight = $("#fight-id").val();
        var user_id = $("#user-id").val();

        //On page load, load the pokemon that will attack
        $.ajax({
            url: '/Fighting/Attack',
            dataType: 'text',
            type: 'post',
            data: { "fight": fight, "user-id": user_id },
            success: function (data) {
                $("#fight-container").empty();
                $("#fight-container").append(data);
            }
        });
    }


    loadFightFromTemplate();


    $(".btn-attack").click(function () {
        var id = this.getAttribute("attack-id");
        var user_id = $("#user-id").val();
        var fight = $("#fight-id").val();

        $("#user-pokemon-img").css("animation", "shake 0.5s");

        window.setTimeout(function () {
            $("#user-pokemon-img").css("animation", "");
        }, 500);

        $.ajax({
            url: '/Fighting/Attack',
            dataType: 'text',
            type: 'post',
            data: { "attack": id, "user-id": user_id, "fight": fight },
            success: function (data) {
                $("#fight-container").empty().append(results);

                window.setTimeout(function () {
                    $("#comp-pokemon-img").css("animation", "shake 0.5s");

                    window.setTimeout(function () {
                        $("#comp-pokemon-img").css("animation", "");
                    }, 500);
                }, 1000);
            }
        });
    });

}