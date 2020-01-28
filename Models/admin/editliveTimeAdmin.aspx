<%@ Page Title="" Language="C#" MasterPageFile="~/Models/Admin.Master" AutoEventWireup="true" CodeBehind="editliveTimeAdmin.aspx.cs" Inherits="ManageLiveSessionWeb.Models.admin.editliveTimeAdmin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@9"></script>

    <style>
        .swal2-popup {
            font-size: 14px !important;
        }

        table {
            table-layout: fixed;
            overflow-wrap: break-word;
            border-collapse: collapse !important;
        }

        table, td, th {
            border: 1px solid black;
            text-align: center !important;
            vertical-align: middle !important;
            font-weight: bold;
            font-size: 16px;
        }
    </style>
   <h2>تغيير ميعاد محاضرة مباشرة</h2>
    <div class="row" style="padding: 23px;" id="test">
    </div>

 
    <div class="widget-box">
        <div class="widget-header">
            <h4 class="widget-title">الأوقات المتاحة للتعديل</h4>
        </div>

        <div class="widget-body">
            <div class="widget-main">
                <div id="schedule"></div>
            </div>
        </div>
    </div>

    <script>
        var pdfPath = "";
        var within_3Days = true;
        var changeType = "";
        function edit(id) {

            Swal.fire({
                title: 'هل انت متأكد من التعديل ؟',
                text: "برجاء التاكد من التعديل",
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                cancelButtonText: 'لا',
                confirmButtonText: 'نعم قم بالتعديل'
            }).then((result) => {
                if (result.value) {

                    var obj = {
                        APP_TERM: "", APP_LIVESESSION_NO: "", APP_SESSION_NUMBER: "", APP_DAY: "", APP_SESSION_TIME: "",
                        COURSE_NUMBER: "", STAFF_NAME: "", STUDIO_NUMBER: "", STUDY_LEVEL: "", STUDY_LEVEL: "", SPECIFIC_MAJOR: "", GENERAL_MAJOR: "", EMAIL: "",
                        SessionType: "", GROUPTYPE: "", COURSE_NAME: "", APP_DATE: ""

                    };

                    var dat = $("#button" + id);
                    obj.APP_TERM = $(dat).data("term");  //144010
                    obj.APP_LIVESESSION_NO = $(dat).data("livesessionno");  //المحاضرة المباشرة الأولي
                    obj.APP_SESSION_NUMBER = $(dat).data("livesessionnumber");//session1
                    obj.APP_DAY = $(dat).data("appday");// 1    OR 2 or ....    7
                    obj.APP_SESSION_TIME = $(dat).data("sessiontime"); //Female [8:00 pm , 8:30 pm] OR Male [8:45 pm , 9:15 pm] ...
                    obj.COURSE_NUMBER = $(dat).data("coursenumber");  //DL","0608","201
                    obj.STAFF_NAME = $(dat).data("staffname");//د. فارس العصيمي
                    obj.STUDIO_NUMBER = $(dat).data("studionumber"); //1","18
                    obj.STUDY_LEVEL = $(dat).data("studylevel"); //1... 8 .....
                    obj.SPECIFIC_MAJOR = $(dat).data("specificmajor"); //ترخ سلم ....
                    obj.GENERAL_MAJOR = $(dat).data("generalmajor");
                    obj.EMAIL = $(dat).data("email");
                    obj.SessionType = $(dat).data("sessiontype");
                    obj.GROUPTYPE = $(dat).data("grouptype");
                    obj.COURSE_NAME = $(dat).data("coursename");

                    debugger
                    var execuses = { NEW_NOTES: "", NEW_DOC_PATH: "" };
                    //if (within_3Days == false) {
                    //    if (pdfPath != "" && $("#notes").val() != "") {
                    //        execuses.NEW_NOTES = $("#notes").val();
                    //        execuses.NEW_DOC_PATH = pdfPath;

                    //    }
                    //    else {

                    //        Swal.fire(
                    //            'ملاحظة!',
                    //            "يجب كتابة سبب التعديل و رفع ملف السبب ... مثلا : تذكرة سفر",
                    //            'error'
                    //        )
                    //        return;
                    //    }
                    //}

                    if (dat != null && dat != undefined || changeType != "") {
                        $(".modal").show();

                        var item = localStorage.getItem("editobj");
                        var obj = JSON.stringify({ MyLiveSession: JSON.parse(item), New_LiveSession: obj, changeType: changeType, within_3Days: within_3Days, execuses: execuses });


                        $.ajax({
                            url: path + "BLL/B_liveDoctors/DoctorsLive.asmx/ChangeLiveSessionsTime",
                            type: "POST",
                            dataType: "json",
                            data: obj,
                            contentType: "application/json; charset=utf-8",
                            success: function (data) {

                                Swal.fire({
                                    title: 'Success',
                                    text: "تم تغيير ميعاد المحاضرة المباشرة",
                                    icon: 'success',
                                    showCancelButton: false,
                                    confirmButtonColor: '#3085d6',
                                    cancelButtonColor: '#d33',
                                    confirmButtonText: 'OK!'
                                }).then((result) => {
                                    if (result.value) {
                                        changeType = "";
                                        within_3Days = true;
                                        window.close();
                                    }
                                })
                            },
                            error: function (err) {
                                $(".modal").hide();
                                alert(JSON.stringify(err));
                            },
                            complete: function () {
                                $(".modal").hide();
                            }
                        });

                    } else {

                    }
                }
            })


        }

        function GET_Period(obj) {

            if (obj.SessionType != "Male" && obj.SessionType != "Female") {

                switch (parseInt(obj.APP_SESSION_NUMBER.toString().replace("session", ""))) {
                    case 1:
                    case 2:
                        return " الفترة الاولي ";
                    case 3:
                    case 4:
                        return " الفترة الثانية ";
                    case 5:
                    case 6:
                        return " الفترة الثالثة ";
                }
            } else {
                var type = obj.SessionType == "Male" ? "طـلاب" : "طالـبات";
                switch (parseInt(obj.APP_SESSION_NUMBER.toString().replace("session", ""))) {
                    case 1:
                    case 2:
                        return "  الفترة الاولي - " + type;
                    case 3:
                    case 4:
                        return "  الفترة الثانية - " + type;
                    case 5:
                    case 6:
                        return " الفترة الثالثة  - " + type;
                }
            }
        }

        $(function () {
         


            var item = localStorage.getItem("editobj");
            if (item != null && item != undefined) {
                try {
                    var obj = {
                        APP_TERM: "", APP_LIVESESSION_NO: "", APP_SESSION_NUMBER: "", APP_DAY: "", APP_SESSION_TIME: "",
                        COURSE_NUMBER: "", STAFF_NAME: "", STUDIO_NUMBER: "", STUDY_LEVEL: "", STUDY_LEVEL: "", SPECIFIC_MAJOR: "", GENERAL_MAJOR: "", EMAIL: "",
                        SessionType: "", GROUPTYPE: "", COURSE_NAME: "", APP_DATE: ""

                    };
                    obj = JSON.parse(item);
                    var date = '';

                    try {

                        var sDate = obj.APP_DATE.split('/');
                        date = sDate[1] + "/" + sDate[0] + "/" + sDate[2].split(' ')[0];
                    } catch (err) { }

                    var str = "<table class='table table-bordered table-striped'><tbody>" +
                        " <tr style='background-color:aquamarine;'> <td colspan=2>بيانات المحاضرة المراد تغييرها</td></tr>" +
                        " <tr> <td colspan=2>" + date + "</td></tr>" +
                        "<tr><td> Studio : " + obj.STUDIO_NUMBER + "</td><td>  " + GET_Period(obj) + "</td></tr> " +
                        " <tr> <td>" + obj.COURSE_NUMBER + "</td> <td>" + obj.COURSE_NAME + "</td></tr>" +
                        " <tr> <td>" + obj.STAFF_NAME + "</td> <td>" + obj.GENERAL_MAJOR + "</td></tr > " +
                        " </tbody></table>";

                    $("#test").html(str);

                } catch (err) {
                    alert(err);
                }

                var parsedObj = JSON.parse(item);

                if (parsedObj.SessionType != "Male" && parsedObj.SessionType != "Female") {
                    var obj = JSON.stringify({ liveSession: parsedObj });

                    $(".modal").show();
                    $.ajax({
                        url: path + "BLL/B_liveDoctors/DoctorsLive.asmx/AvailableDoctorSessions",
                        type: "POST",
                        dataType: "json",
                        data: obj,
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            $("#schedule").empty();
                            if (data.d != "") {
                                var data = JSON.parse(data.d);
                                changeType = data.changeSessionType;

                                $("#schedule").append(data.availableDatesTable);
                                //if (data.within_3Days) {
                                //    within_3Days = data.within_3Days;
                                //    $("#within3daysReason").hide();
                                //}
                                //else {
                                //    within_3Days = data.within_3Days;
                                //    $("#within3daysReason").show();
                                //}
                            }
                            else {
                                Swal.fire(
                                    'ملاحظة!',
                                    "لا توجد مواعيد متاحة او انتهاء الحاضرات المباشرة",
                                    'info'
                                )
                            }
                        },
                        error: function (err) {
                            $(".modal").hide();
                            alert(JSON.stringify(err));
                        },
                        complete: function () {
                            $(".modal").hide();



                        }
                    });
                } else {


                    var obj = JSON.stringify({ liveSession: parsedObj });

                    $(".modal").show();
                    $.ajax({
                        url: path + "BLL/B_liveDoctors/DoctorsLive.asmx/AvailableDoctorSessionsProjects",
                        type: "POST",
                        dataType: "json",
                        data: obj,
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            $("#schedule").empty();

                            var data = JSON.parse(data.d);
                            changeType = data.changeSessionType;
                            $("#schedule").append(data.availableDatesTable);
                            //if (data.within_3Days) {
                            //    within_3Days = data.within_3Days;
                            //    $("#within3daysReason").hide();
                            //}
                            //else {
                            //    within_3Days = data.within_3Days;
                            //    $("#within3daysReason").show();
                            //}
                        },
                        error: function (err) {
                            $(".modal").hide();
                            alert(JSON.stringify(err));
                        },
                        complete: function () {
                            $(".modal").hide(); 
                        }
                    });
                } 
            } 
        });
  
    </script>
</asp:Content>
