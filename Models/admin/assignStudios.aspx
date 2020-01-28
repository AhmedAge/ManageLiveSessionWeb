<%@ Page Title="" Language="C#" MasterPageFile="~/Models/Admin.Master" AutoEventWireup="true" CodeBehind="assignStudios.aspx.cs" Inherits="ManageLiveSessionWeb.Models.admin.assignStudios" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>تخصيص الاستوديوهات</title>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
        table {
            /*width: 100%;*/
            table-layout: fixed;
            overflow-wrap: break-word;
            border-collapse: collapse !important;
        }

        table, td, th {
            border: 1px solid black;
            text-align: center !important;
            vertical-align: middle !important;
            font-size:14px!important;
            font-weight:bold!important;
        }
    </style>

    <h2>Assign User Studios</h2>
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

                <table class="table table-bordered table-condensed table-striped table-hover">
                  <%--  <thead>
                        <tr>
                            <td>المستخدم</td>
                            <td>اليوم</td>
                            <td>الاستوديو</td>
                        </tr>
                    </thead>--%>
                    <tbody id="UserStudios">
                    </tbody>
                </table>

            </div>
        </div>
        <div class="row">
            <div class="col-lg-12">
                <div class="widget-box">
                    <div class="widget-header">
                        <h4 class="widget-title">User Assign Days</h4>
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
    </form>
    <br />
    &emsp; &emsp; 
    <button type="button" class="btn btn-primary" id="check"><i class="ace-icon fa fa-floppy-o bigger-120"></i>Save</button>


    <script>
        var daysinfo = { date: "", dayName: "", day: "" };
        var liveSessionDays = [];

        function SelectDay(id) {

            var lst = $("input[data-day='" + id + "']");
            $.each(lst, function (index, value) {
                if (document.getElementById("day" + id).checked) {
                    $(value).removeAttr("disabled");
                    $(value).removeClass("disabled");

                } else {
                    document.getElementById(value.id).checked = false;
                    $(value).attr("disabled", true);
                    $(value).addClass("disabled");
                }

            });
        }

        $(function () {

            $("#check").click(function (e) {
                e.preventDefault();

                if (confirm("هل تريد تأكيد التخصيص")) {
                    var studios = [];

                    var day = { daynumber: 0, studios: studios };

                    var days = [];


                    for (var i = 1; i <= 7; i++) {

                        if (document.getElementById("day" + i).checked) {
                            var lst = $("input[data-day='" + i + "']");
                            studios = [];
                            var enter = false;
                            $.each(lst, function (index, value) {
                                if ($(value).is(":checked")) {
                                    studios.push($(value).data("studio"));
                                    enter = true;
                                }
                            })
                            if (enter == true) {
                                day = { daynumber: 0, studios: studios };

                                day.daynumber = i;
                                day.studios = studios;

                                days.push(day);

                                studios = [];
                                enter = false;
                            }
                        }
                    };


                    if (days.length > 0) {
                        $(".modal").show();

                        if ($("#termCode").val() == null || $("#termCode").val() == undefined || $("#termCode").val() == "" ||
                            $("#livesession").val() == null || $("#livesession").val() == undefined || $("#livesession").val() == "" ||
                            $("#user_id").val() == null || $("#user_id").val() == undefined || $("#user_id").val() == "") {
                            Swal.fire(
                                'ملاحظة!',
                                "برجاء اختيار التيرم و المحاضرة المباشرة و المستخدم و من ثم اختر الايام و الاستوديوهات بكل يوم",
                                'error'
                            )
                            return;
                        }
                        var userStd = { term: "", liveSession: "", user: "", days: days };

                        userStd.user = $("#user_id").val();
                        userStd.term = $("#termCode").val();
                        userStd.liveSession = $("#livesession").val();
                        userStd.days = days;

                        var str = JSON.stringify({ userstudios: userStd });

                        $.ajax({
                            url: path + "BLL/B_Admin/adminOp.asmx/AssignUserStudios",
                            type: "POST",
                            dataType: "json",
                            async: true,
                            data: str,
                            contentType: "application/json; charset=utf-8",
                            success: function (data) {
                                Swal.fire(
                                    'مبروك!',
                                    data.d,
                                    'success'
                                )
                            },
                            error: function (err) {
                                alert(JSON.stringify(err));
                            },
                            complete: function () {
                                $(".modal").hide();
                            }
                        });

                    }
                    else {
                        Swal.fire(
                            'ملاحظة!',
                            "يجب اختيار بعض الايام و بعض الاستوديوهات",
                            'error'
                        )
                        return;
                    }

                }
            });

            $(".modal").show();
            $.ajax({
                url: path + "BLL/B_Admin/adminOp.asmx/SelectUsers",
                type: "POST",
                dataType: "json",
                async: true,
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    $("#user_id").empty();

                    $("#user_id").append(data.d);
                },
                error: function (err) {
                    alert(JSON.stringify(err));
                },
                complete: function () {
                    $(".modal").hide();
                }
            });

            //
            $("#livesession").change(function () {
                LiveSessionDates();
            });
            $("#termCode").change(function () {
                LiveSessionDates();
            });

            //

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


                $.ajax({
                    url: path + "BLL/B_Admin/adminOp.asmx/UserStudiosView",
                    type: "POST",
                    dataType: "json",
                    async: true,
                    data: str,
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        $("#UserStudios").empty();

                        $("#UserStudios").append(data.d);
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



            function buildDayStudios() {
                var str = "";
                for (var day = 1; day <= 7; day++) {
                    var date = liveSessionDays.find(x => x.day == day);
                    str += " <div class='alert alert-info'>" +
                        " <label class='inline' >" +
                        "   <input type='checkbox' class='ace  ace-checkbox-2' id='day" + day + "' onchange='SelectDay(" + day + ")'>" +
                        "       <span class='lbl' style='font-size:17px;color:black;font-weight:bold;'>&nbsp;Day " + day + " - " + date.date + " - " + date.dayName + "</span>" +
                        "                                      </label>" +
                        "&emsp; &emsp;&emsp;";
                    for (var studio = 1; studio <= 18; studio++) {
                        str += "<label class='inline'>" +
                            "              <input type='checkbox' class='ace  disabled ace-checkbox-2' id='day" + day + "studio" + studio + "' data-studio='" + studio + "'   data-day='" + day + "' >" +
                            "                 <span class='lbl'>&nbsp;Std " + studio + "&emsp;</span>" +
                            "                                                    </label>" +
                            "&nbsp;";


                    }
                    str += "</div>";
                }

                $("#daysStudios").html(str);
                str = "";



            }

            //calling build day function

            //


        });

    </script>
</asp:Content>
