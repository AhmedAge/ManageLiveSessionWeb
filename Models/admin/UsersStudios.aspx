<%@ Page Title="" Language="C#" MasterPageFile="~/Models/Admin.Master" AutoEventWireup="true" CodeBehind="UsersStudios.aspx.cs" Inherits="ManageLiveSessionWeb.Models.admin.UsersStudios" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>استوديوهات المستخدمين

    </title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
        .c:hover {
            font-weight: bold;
            cursor: pointer;
        }

        .lbl {
            vertical-align: middle !important;
        }
    </style>
    <h2><i class="menu-icon fa fa-camera"></i>User Studios</h2>
    <form class="form-horizontal" method="post" id="frmAssignDays">
        <div class="row">
            <div class="col-lg-12">

                <div class="form-group">
                    <label class="control-label col-sm-1" for="termCode">Term Code:</label>
                    <div class="col-sm-11">
                        <select class="form-control" id="termCode" name="termCode">
                            <option value="144010">144010</option>
                            <option value="144020">144020</option>
                            <option value="144030">144030</option>
                            <option value="144110">144110</option>
                            <option value="144120">144120</option>
                            <option value="144130">144130</option>
                            <option value="144210">144210</option>
                            <option value="144220">144220</option>
                            <option value="144230">144230</option>
                            <option value="144310">144310</option>
                            <option value="144320">144320</option>
                            <option value="144330">144330</option>
                        </select>
                    </div>
                </div>
                <div class="form-group">
                    <label class="control-label col-sm-1" for="livesession">Live Session :</label>
                    <div class="col-sm-11">
                        <select class="form-control" id="livesession" name="livesession">
                            <option value="المحاضرة المباشرة الأولي">المحاضرة المباشرة الأولي</option>
                            <option value="المحاضرة المباشرة الثانية">المحاضرة المباشرة الثانية</option>
                            <option value="المحاضرة المباشرة الثالثة">المحاضرة المباشرة الثالثة</option>
                            <option value="المحاضرة المباشرة الرابعة">المحاضرة المباشرة الرابعة</option>
                        </select>
                    </div>
                </div>
                <div class="form-group">
                    <label class="control-label col-sm-1" for="user_id">User:</label>
                    <div class="col-sm-11">
                        <select class="form-control" id="user_id" name="user_id">
                        </select>
                    </div>
                </div>
            </div>

        </div>
    </form>
    <div class="row">
        <div class="col-lg-12">
            <div class="widget-box">
                <div class="widget-header">
                    <h4 class="widget-title">User Days & Studios</h4>
                </div>

                <div class="widget-body">
                    <div class="widget-main">
                        <div class="widget-body">
                            <div class="widget-main" id="daysStudios">
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <script>
        function OpenStdInfo(stdId) {
            var info = { info: $("#check" + stdId).data("info"), user: $("#user_id").val(), term: $("#termCode").val(), liveSession: $("#livesession").val() };

            localStorage.setItem("studioInfo", JSON.stringify(info));
            window.open("doctorEvaluation.aspx", "_blank");
             
        }
        function buildDayStudios(data) {
            var str = "";
            if (data == undefined)
                return;
           
            $.each(data.days, function (index, value) {

                var date = liveSessionDays.find(x => x.day == value.daynumber);

                str += " <div class='alert alert-info'>" +
                    " <label class='inline c' >" +
                    "   <img src='../../images/ok.png'  width='30px'>" +
                    "       <span class='lbl' style='font-size:17px;color:black;font-weight:bold;'>&nbsp;Day " + value.daynumber + " - " + date.date + " - " + date.dayName + "</span>" +
                    "                                      </label>" +
                    "&emsp; &emsp;&emsp;";
               
                for (var i = 1; i <= 18; i++) {
                    
                    var std = value.studios.find(x => x == i);


                    if (std == undefined) {
                        str += "<label class='inline'  >" +
                            "   <img src='../../images/no.png'  width='13px'>" +
                            "                 <span class='lbl'>Std " + i + "</span>" +
                            "                                                    </label>&emsp;" +
                            "&nbsp;";
                    } else {

                        str += "<label class='inline c' onclick='OpenStdInfo(" + std+")' id='check" + std + "' data-info='" + std+"-"+ value.daynumber + "-" + date.date + "-" + date.dayName + "' >" +
                            "   <img src='../../images/ok.png'  width='25px'>" +
                            "                 <span class='lbl'>Std " + std + "</span>" +
                            "                                                    </label>&emsp;" +
                            "&nbsp;";
                    }
                }

                //$.each(value.studios, function (i,studio) {
                //    str += "<label class='inline c'  >" +
                //        "   <img src='../../images/ok.png'  width='20px'>" +
                //        "                 <span class='lbl'>Std " + studio + "</span>" +
                //        "                                                    </label>&emsp;" +
                //        "&nbsp;"; 
                //});
                str += "</div>";
            }); 

            $("#daysStudios").html(str);
            str = "";
        } 

        $("#user_id").change(function () {
            if ($("#user_id").val() == 0) {
                alert("من فضلك قم باختيار مستخدم");
                return;
            }
            var str = JSON.stringify({ user: $("#user_id").val(), term: $("#termCode").val(), liveSession: $("#livesession").val() });

            $.ajax({
                url: path + "BLL/B_Admin/adminOp.asmx/UserStudios",
                type: "POST",
                dataType: "json",
                async: true,
                data: str,
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    
                    if (data.d == "") {
                        $("#daysStudios").html("<h1>لا توجد بيانات متاحة</h1>" );
                        return;
                    }

                    var data = JSON.parse(data.d);

                    buildDayStudios(data);


                },
                error: function (err) {
                    alert(JSON.stringify(err));
                },
                complete: function () {
                    $(".modal").hide();
                }
            });
        });

        $(function () {


            $(".modal").show();
            $.ajax({
                url: path + "BLL/B_Admin/adminOp.asmx/SelectUsers",
                type: "POST",
                dataType: "json",
                async: true,
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    $("#user_id").empty();

                    $("#user_id").append("<option value='0'>اختر مستخدم</option>"+data.d);
                },
                error: function (err) {
                    alert(JSON.stringify(err));
                },
                complete: function () {
                    $(".modal").hide();
                }
            });

            function LiveSessionDates() {
                var str = JSON.stringify({ term: $("#termCode").val(), liveSession: $("#livesession").val() });

                $.ajax({
                    url: path + "BLL/B_Admin/adminOp.asmx/LiveSessionDates",
                    type: "POST",
                    dataType: "json",
                    async: true,
                    data: str,
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        var datesLst = JSON.parse(data.d);
                        liveSessionDays = [];
                        $.each(datesLst, function (index, value) {
                            daysinfo = { date: "", dayName: "", day: "" };
                            daysinfo.date = value.date;
                            daysinfo.dayName = value.dayName;
                            daysinfo.day = value.day;

                            liveSessionDays.push(daysinfo);
                        });
                        buildDayStudios();

                    },
                    error: function (err) {
                        alert(JSON.stringify(err));
                    },
                    complete: function () {
                        $(".modal").hide();
                    }
                });
            }

            LiveSessionDates();




        });
    </script>
</asp:Content>
