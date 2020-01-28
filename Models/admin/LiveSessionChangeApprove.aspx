<%@ Page Title="" Language="C#" MasterPageFile="~/Models/Admin.Master" AutoEventWireup="true" CodeBehind="LiveSessionChangeApprove.aspx.cs" Inherits="ManageLiveSessionWeb.Models.admin.LiveSessionChangeApprove" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>طلبات تغيير مواعيد المحاضرات المباشرة
    </title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
        table {
            /*width: 100%;*/
            /*table-layout: fixed;
            overflow-wrap: break-word;*/
            border-collapse: collapse !important;
        }

        table, td, th {
            border: 1px solid black;
            text-align: center !important;
            vertical-align: middle !important;
            font-size: 17px !important;
            font-weight: bold !important;
        }

        /*.btn-mini > .ace-icon, .btn-minier > .ace-icon, .btn-xs > .ace-icon {
            margin-right: 0px !important;
        }

        .btn-group-minier > .btn, .btn-minier {
            padding: 5 5px !important;
            line-height: 18px;
            border-width: 2px;
            font-size: 12px;
        }*/
    </style>

    <h2>طلبات تغيير مواعيد المحاضرات المباشرة
    </h2>

    <form class="form-horizontal" method="post" id="frmRequests">
        <div class="row">
            <div class="col-lg-6">
                <div class="form-group">
                    <label class="control-label col-sm-2" for="termCode">Term Code:</label>
                    <div class="col-sm-10">
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
            </div>
        </div>
        <div class="row">
            <div class="col-lg-6">
                <div class="form-group">
                    <label class="control-label col-sm-2" for="livesession">Live Session :</label>
                    <div class="col-sm-10">
                        <select class="form-control" id="livesession" name="livesession">
                            <option value="المحاضرة المباشرة الأولي">المحاضرة المباشرة الأولي</option>
                            <option value="المحاضرة المباشرة الثانية">المحاضرة المباشرة الثانية</option>
                            <option value="المحاضرة المباشرة الثالثة">المحاضرة المباشرة الثالثة</option>
                            <option value="المحاضرة المباشرة الرابعة">المحاضرة المباشرة الرابعة</option>
                        </select>
                    </div>
                </div>
            </div>
        </div>

        <div class="form-group">
            <div class="col-sm-offset-1     col-sm-10">
                <button type="submit" id="showRequests" class="btn btn-danger">Show Requests</button>
            </div>
        </div>

    </form>
    <div class="row">
        <div class="col-lg-12" id="requests">
        </div>
    </div>
    <script>
        $(function () {

            $("#frmRequests").submit(function (e) {
                e.preventDefault();

                $(".modal").show();
                var str = JSON.stringify({
                    term: $("#termCode").val(),
                    liveSession: $("#livesession").val()
                });
                $.ajax({
                    url: path + "BLL/B_Admin/adminOp.asmx/ShowRequests",
                    type: "POST",
                    dataType: "json",
                    data: str,
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        $("#requests").empty();

                        $("#requests").html(data.d);
                    },
                    error: function (err) {
                        alert(JSON.stringify(err));
                    },
                    complete: function () {
                        $(".modal").hide();
                    }
                });
            });
        });

        function Download(id) {
            var obj = {
                APP_TERM: "", APP_LIVESESSION_NO: "", APP_SESSION_NUMBER: "", APP_DAY: "", APP_SESSION_TIME: "",
                COURSE_NUMBER: "", STAFF_NAME: "", STUDIO_NUMBER: "", STUDY_LEVEL: "", STUDY_LEVEL: "", SPECIFIC_MAJOR: "", GENERAL_MAJOR: "", EMAIL: "",
                SessionType: "", GROUPTYPE: "", COURSE_NAME: "", APP_DATE: "", NEW_APP_DATE: ""
            };

            var dat = $("#downloadbtn" + id);
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

            window.location.href = "../../_handlers/download.ashx?APP_TERM=" + obj.APP_TERM +
                "&APP_LIVESESSION_NO=" + obj.APP_LIVESESSION_NO +
                "&APP_DAY=" + obj.APP_DAY +
                "&STUDIO_NUMBER=" + obj.STUDIO_NUMBER +
                "&COURSE_NUMBER=" + obj.COURSE_NUMBER +
                "&STUDY_LEVEL=" + obj.STUDY_LEVEL +
                "&APP_SESSION_NUMBER=" + obj.APP_SESSION_NUMBER +
                "&EMAIL=" + obj.EMAIL ;
         

        }


        function Approve(id) {

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
                        SessionType: "", GROUPTYPE: "", COURSE_NAME: "", APP_DATE: "", NEW_APP_DATE: ""
                    };


                    var dat = $("#approvebtn" + id);
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


                    var newobj = {
                        APP_SESSION_NUMBER: "", APP_DAY: "",
                        APP_SESSION_TIME: "", COURSE_NUMBER: "", STAFF_NAME: "", STUDIO_NUMBER: "", STUDY_LEVEL: "",
                        SPECIFIC_MAJOR: "", GENERAL_MAJOR: "", EMAIL: "", SESSIONTYPE: "", GROUPTYPE: "", COURSE_NAME: "", APP_DATE: ""
                    };


                    newobj.APP_TERM = $(dat).data("term");  //144010
                    newobj.APP_LIVESESSION_NO = $(dat).data("livesessionno");  //المحاضرة المباشرة الأولي


                    newobj.APP_SESSION_NUMBER = $(dat).data("newlivesessionnumber");
                    newobj.APP_DAY = $(dat).data("newappday");
                    newobj.APP_SESSION_TIME = $(dat).data("newsessiontime");
                    newobj.COURSE_NUMBER = $(dat).data("newcoursenumber");
                    newobj.STAFF_NAME = $(dat).data("newstaffname");
                    newobj.STUDIO_NUMBER = $(dat).data("newstudionumber");
                    newobj.STUDY_LEVEL = $(dat).data("newstudylevel");
                    newobj.SPECIFIC_MAJOR = $(dat).data("newspecificmajor");
                    newobj.GENERAL_MAJOR = $(dat).data("newgeneralmajor");
                    newobj.EMAIL = $(dat).data("newemail");
                    newobj.SESSIONTYPE = $(dat).data("newsessiontype");
                    newobj.GROUPTYPE = $(dat).data("newgrouptype");
                    newobj.COURSE_NAME = $(dat).data("newcoursename");
                    newobj.APP_DATE = $(dat).data("newappdate");

                    var str = JSON.stringify({ MyLiveSession: obj, New_LiveSession: newobj });

                    $.ajax({
                        url: path + "BLL/B_Admin/adminOp.asmx/ApproveRequets",
                        type: "POST",
                        dataType: "json",
                        data: str,
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            Swal.fire({
                                title: 'Success',
                                text: data.d,
                                icon: 'success',
                                showCancelButton: false,
                                confirmButtonColor: '#3085d6',
                                cancelButtonColor: '#d33',
                                confirmButtonText: 'OK!'
                            }).then((result) => {
                                if (result.value) {

                                    $("#showRequests").trigger("click");
                                }
                            })
                        },
                        error: function (err) {
                            alert(JSON.stringify(err));
                        },
                        complete: function () {
                            $(".modal").hide();
                        }
                    });


                }
            })
          

        }
    </script>
</asp:Content>
