<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="ManageLiveSessionWeb.login" %>

<%-- 
    Document   : login
    Created on : 21-Nov-2018, 14:41:45
    Author     : AhmedEldeeb
--%>

<!DOCTYPE html>
<html lang="en">
<head>
    <title>Live Session Management</title>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <!--===============================================================================================-->
    <link rel="icon" type="image/png" href="images/icons/favicon.ico" />
    <!--===============================================================================================-->
    <link rel="stylesheet" type="text/css" href="vendor/bootstrap/css/bootstrap.min.css">
    <!--===============================================================================================-->
    <link rel="stylesheet" type="text/css" href="fonts/font-awesome-4.7.0/css/font-awesome.min.css">
    <!--===============================================================================================-->
    <link rel="stylesheet" type="text/css" href="vendor/animate/animate.css">
    <!--===============================================================================================-->
    <link rel="stylesheet" type="text/css" href="vendor/css-hamburgers/hamburgers.min.css">
    <!--===============================================================================================-->
    <link rel="stylesheet" type="text/css" href="vendor/select2/select2.min.css">
    <!--===============================================================================================-->
    <link rel="stylesheet" type="text/css" href="css/util.css">
    <link rel="stylesheet" type="text/css" href="css/main.css">
    <!--===============================================================================================-->
    <script>
        history.pushState(null, null, document.URL);
        window.addEventListener('popstate', function () {
            history.pushState(null, null, document.URL);
        });
    </script>
    <style>
        .parent {
            display: table;
        }

        .child {
            display: table-cell;
            vertical-align: middle;
        }
    </style>
</head>
<body>

    <div class="limiter">
        <div class="container-login100">
            <div class="wrap-login100 ">
                <div class="parent">
                    <div class="login100-pic child">
                        <img src="images/img-01.png" alt="IMG">
                    </div>
                </div>
                <form class="login100-form" method="post" id="login" runat="server" action="login.aspx">
                    <span class="login100-form-title">Live Session Management System
                    </span>
                    <div class="wrap-input100">
                        <asp:TextBox ID="username" CssClass="input100" runat="server" ToolTip="USER ID"></asp:TextBox>
                        <span class="focus-input100"></span>
                        <span class="symbol-input100">
                            <i class="fa fa-envelope" aria-hidden="true"></i>
                        </span>
                    </div>

                    <div class="wrap-input100">
                        <asp:TextBox ID="password1" CssClass="input100" runat="server" ToolTip="Password" TextMode="Password"></asp:TextBox>
                        <span class="focus-input100"></span>
                        <span class="symbol-input100">
                            <i class="fa fa-lock" aria-hidden="true"></i>
                        </span>
                    </div>

                    <div class="container-login100-form-btn">
                        <button class="login100-form-btn" id="btnlogin" runat="server" type="submit">
                            Login</button>
                    </div>
                    <div class="text-center p-t-136">
                        <p id="error"></p>
                    </div>
                </form>
                <span style="font-weight:bold">جميع الحقوق محفوظة لجامعة الملك فيصل &copy; عمادة التعلم الإلكتروني و التعليم المستمر
                </span>
            </div>

        </div>
    </div>




    <!--===============================================================================================-->
    <script src="vendor/jquery/jquery-3.2.1.min.js"></script>
    <!--===============================================================================================-->
    <script src="vendor/bootstrap/js/popper.js"></script>
    <script src="vendor/bootstrap/js/bootstrap.min.js"></script>
    <!--===============================================================================================-->
    <script src="vendor/select2/select2.min.js"></script>
    <!--===============================================================================================-->
    <script src="vendor/tilt/tilt.jquery.min.js"></script>
    <%--   <script src="js/login.js" type="text/javascript"></script>--%>
    <script>
        $('.js-tilt').tilt({
            scale: 1.1
        })
    </script>
    <!--===============================================================================================-->
    <script src="js/main.js"></script>

</body>
</html>
