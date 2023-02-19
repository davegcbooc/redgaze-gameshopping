$().ready(function () {
    $("#btnEdit").hide();
    $("#btnUpdate").hide();
    $("#itmdesc").attr("readonly", "readonly");
    $("#itmprice").attr("readonly", "readonly");
    $("#itmqtyonhand").attr("readonly", "readonly");

    $("#btnSearch").click(function () {

        $("#btnEdit").attr("disabled", "disabled");
        var itemcode = $("#itmcode").val();

        $.post("../Home/SearchItem", {
            itemcode: itemcode
        }, function (res) {
            if (res[0].mess == 0) {
                $("#btnEdit").removeAttr("disabled");
                $("#btnEdit").show();
                $("#itmdesc").val(res[0].desc);
                $("#itmprice").val(res[0].price);
                $("#itmqtyonhand").val(res[0].qtyonhand);
            } else {
                alert('Invalid Item code');
            }
        });
    });

    $("#btnEdit").click(function () {
        $("#btnEdit").hide();
        $("#btnUpdate").show();
        $("#itmcode").prop("readonly", true);
        $("#itmdesc").removeAttr("readonly");
        $("#itmprice").removeAttr("readonly");
        $("#itmqtyonhand").removeAttr("readonly");

    });

    $("#btnUpdate").click(function () {

        var itemdesc = $("#itmdesc").val();
        var itemprice = $("#itmprice").val();
        var itemqtyonhand = $("#itmqtyonhand").val();
        var itemcode = $("#itmcode").val();

        $.post("../Home/UpdateItem", {
            itemdesc: itemdesc,
            itemprice: itemprice,
            itemqtyonhand: itemqtyonhand,
            itemcode: itemcode
        }, function (res) {
            if (res[0].mess == 0) {
                alert("Successfully Updated");
                $("#btnUpdate").hide();
                $("#itmcode").removeAttr("readonly").val();
                $("#itmdesc").prop("readonly", true).val();
                $("#itmprice").prop("readonly", true).val();
                $("#itmqtyonhand").prop("readonly", true).val();


            } else
                alert("Ooooppppssss.. Something Wrong");
        });
    });
});