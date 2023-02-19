
function getImagePreview() {
    var txt = (event.target.files[0].name).split(".").pop().toLowerCase();
    if (txt == "jpg" || txt == "png" || txt == "gif" || txt == "bmp") {
        var image = URL.createObjectURL(event.target.files[0]);
        var imagediv = document.getElementById("preview");
        var newimg = document.createElement('img');
        newimg.src = image;
        newimg.width = "200";
        imagediv.appendChild(newimg);
    } else {
        alert("File uploaded is invalid");
        $("#preview").html("");
        $("#uploadImg").val("");
    }
}

function resetForm() {
    if (confirm("Want to clear all?")) {
        $("#txtName").val("")
        $("#txtprice").val("")
        $("#txtonhand").val("")
        $("#txtDesc").val("")
        $("#preview").html("");
        $("#uploadImg").val("");
    }
}


$(function () {
    var numCode;
    ///
    $.post("../Home/getItemCode", {
        //this is part when you are going to send data to the server.
    }, function (res) {
        var num = parseInt(res[0].itmcode) + 1;
        numCode = $("#txtCode").val("I" + num + (new Date()).getTime());
        //$("#txtCode").prop('disabled', true);
    });
    ///
    $("#txtCode").val(numCode);
    $("#productRecord").submit(function () {
        //$("#txtCode").prop('disabled', false);
    });


});
