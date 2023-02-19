$('#btnlogin').click(function (){
    var usr = $('#usr').val();
    var pwd = $('#pwd').val();

    if (usr == "" || usr == null) {
        alert("Field Required");
        return false;
    }
    if (pwd == "" || pwd == null) {
        alert("Field Required");
        return false;
    }

    $.post("../Home/UserLogin", {
        username: usr,
        pwd: pwd
    }, function (res) {
        if (res[0].mess != 1) {
            window.location.href = "../Home/ListAllProducts";
        } else {
            alert('Invalid Credentials');
        }

    });


});