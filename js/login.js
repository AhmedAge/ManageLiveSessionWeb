/* 
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

history.pushState(null, null, document.URL);
window.addEventListener('popstate', function () {
    history.pushState(null, null, document.URL);
});

$(function() {
    
     $("#form-username").on("keypress", function (event) {
        if (event.which == 13 && !event.shiftKey) {
            event.preventDefault();
            // $("#submitForm").submit();
            $("#btnlogin").trigger("click");
        }
    });
    $("#form-password").on("keypress", function (event) {
        if (event.which == 13 && !event.shiftKey) {
            event.preventDefault();
            //$("#submitForm").submit();
            $("#btnlogin").trigger("click");
        }
    });
    
     
    $("#login").submit(function(e) {
e.preventDefault();
      
        var $this = $("#btnlogin");
        $this.button('loading');
        $.ajax({
            url: 'LoginService',
            type: "POST",
            data: {'username': $("#form-username").val(), 'password': $("#form-password").val()},
            headers: {"method": "login"},
            success: function(data) {
                if (data == "Success_Login")
                    window.location.href = "Search.aspx";
               else
               {
                   $("#error").html("User ID or Password is incorrect");
               }
                $this.button('reset');
            },
            error: function() {

            },
            complete: function(jqXHR, textStatus) {
                $this.button('reset');
            }
        });

    });

});