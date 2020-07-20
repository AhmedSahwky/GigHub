var AttendanceService = function () {
    var createAttendance = function (gigId, done, fail) {
        $.post("/api/Attendances", { gigId: gigId })
            .done(function () {
                button.removeClass("btn-default")
                    .addClass("btn-info")
                    .text("Going");
            })
            .done(done)
            .fail(fail);
    };

    var deleteAttendance = function (gigId, done, fail) {
        $.ajax({
            url: "/api/Attendances/" + gigId,
            method: "DELETE"
        })
            .done(done)
            .fail(fail);
    };

    return {
        createAttendance: createAttendance,
        deleteAttendance: deleteAttendance
    }
}();