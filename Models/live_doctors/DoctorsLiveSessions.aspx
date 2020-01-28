<%@ Page Title="" Language="C#" MasterPageFile="~/Models/Doctors.Master" AutoEventWireup="true" CodeBehind="DoctorsLiveSessions.aspx.cs" Inherits="ManageLiveSessionWeb.Models.live_doctors.DoctorsLiveSessions" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Doctors Live Session</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!-- ace styles -->
    <link rel="stylesheet" href="../../assets/css/ace.min.css" class="ace-main-stylesheet" id="main-ace-style" />

    <script type="text/javascript">
        if ('ontouchstart' in document.documentElement) document.write("<script src='assets/js/jquery.mobile.custom.min.js'>" + "<" + "/script>");
    </script>

    <!-- page specific plugin scripts -->
    <script src="../../assets/js/jquery-ui.min.js"></script>
    <script src="../../assets/js/jquery.ui.touch-punch.min.js"></script>
    <style>
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
    <h1 class="text-center" style="color: red;">جدول المحاضرات المباشرة متاح للتغيير خلال 3 ايام</h1>

    <div id="schedule"></div>

    <script> 
        function collab(id) {
            debugger
            var obj = {
                APP_TERM: "", APP_LIVESESSION_NO: "", APP_SESSION_NUMBER: "", APP_DAY: "", APP_SESSION_TIME: "",
                COURSE_NUMBER: "", STAFF_NAME: "", STUDIO_NUMBER: "", STUDY_LEVEL: "", STUDY_LEVEL: "", SPECIFIC_MAJOR: "", GENERAL_MAJOR: "", EMAIL: "",
                SessionType: "", GROUPTYPE: "", COURSE_NAME: "", APP_DATE: ""

            };

            var dat = $("#linkbtn" + id);
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
            obj.APP_DATE = $(dat).data("appdate");

            var gender = $(dat).data("sessiontime").split(' ')[0];
           
            if (gender == "Female") {
                window.open($(dat).data("colabf"), '_blank');
            }
            else if (gender == "Male") {
                window.open($(dat).data("colabm"), '_blank');
            }
            $(".modal").show();

          

            var str = JSON.stringify({ liveupdate: obj, gender: gender });

            $.ajax({
                url: path + "BLL/B_liveDoctors/DoctorsLive.asmx/UpdateLiveSessionsCollabLink",
                type: "POST",
                dataType: "json",
                data: str,
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                     
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

        function edit(id) {

            
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
            obj.APP_DATE = $(dat).data("appdate");

            localStorage.setItem("editobj", JSON.stringify(obj));

            if (dat != null && dat != undefined) {

                window.open("editlivesessiontime.aspx", "_blank");

            } else {

                alert("يجب اختيار محاضرة مباشرة");

            }
        }

        $(function () {

            $(".modal").show();
            $.ajax({
                url: path + "BLL/B_liveDoctors/DoctorsLive.asmx/SelectDoctorSessions",
                type: "POST",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    $("#schedule").empty();

                    $("#schedule").append(data.d);
                },
                error: function (err) {
                    alert(JSON.stringify(err));
                },
                complete: function () {
                    $(".modal").hide();



                }
            });


        });
    </script>
</asp:Content>
