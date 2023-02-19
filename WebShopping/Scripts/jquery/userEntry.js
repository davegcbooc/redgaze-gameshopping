$(function () {
    var userCode;
    ///
    $.post("../Home/getUserCode", {
        //this is part when you are going to send data to the server.
    }, function (res) {
        var num = parseInt(res[0].usrcode) + 1;
        userCode = $("#txtusrid").val("U" + num + (new Date()).getTime());
        //$("#txtusrid").prop('disabled', true);
    });
    ///
    $("#txtusrid").val(userCode);
    $("#userRecord").submit(function () {
        //$("#txtusrid").prop('disabled', false);
    });


});

/*$(function () {
        
        var usrcode;
        $("#txtusrid").val((new Date()).getTime());
        var userid = JSON.stringify(usrcode);
        $("#txtusrid").val(userid);
        $("#btnSignUp").click(function () {

            var usrid = $("#txtusrid").val();
            var usrlname = $("#txtlname").val();
            var usrfname = $("#txtfname").val();
            var usradd = $("#txtaddress").val();
            var usrcontact = $("#txtcontact").val();
            var usremail = $("#txtemail").val();
            var usrpswrd = $("#txtpswrd").val();
            var usrname = $("#txtusername").val();
            var usrrole = $("#txtrole").val();

            $.post("../Home/UserSignup", {
                usrid: usrid,
                usrlname: usrlname,
                usrfname: usrfname,
                usradd: usradd,
                usrcontact: usrcontact,
                usremail: usremail,
                usrpswrd: usrpswrd,
                usrname: usrname,
                usrrole: usrrole

            }, function (res) {

                if (res[0].mess == 0) {
                    alert("Congrats! User Successfully Registered");
                    window.location.href = "../Home/Login";
                } else
                    alert("Opss.. Something went wrong");
            });
        });        
}); */







