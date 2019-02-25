var modalNoteDetailBodyId = "#modal_notedetail_body";

$(function () {
    $("#modal_notedetail").on('show.bs.modal', function (e) {
        var btn = $(e.relatedTarget);
        noteid = btn.data("note-id");

        $(modalNoteDetailBodyId).load("/Note/GetNoteText/" + noteid);
    })
});