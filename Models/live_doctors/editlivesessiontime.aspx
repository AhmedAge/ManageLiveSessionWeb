<%@ Page Title="" Language="C#" MasterPageFile="~/Models/Doctors.Master" AutoEventWireup="true" CodeBehind="editlivesessiontime.aspx.cs" Inherits="ManageLiveSessionWeb.Models.live_doctors.editlivesessiontime" %>

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
   
    <div class="row" style="padding: 23px;" id="test">
    </div>


    <div class="widget-box" id="within3daysReason">
        <div class="widget-header">
            <h4 class="widget-title">برجاء ادخال سبب التعديل</h4>
        </div>

        <div class="widget-body">
            <div class="widget-main">
                <form class="form-horizontal" role="form">
                    <div class="form-group">
                        <label class="col-sm-1 control-label no-padding-right" for="form-field-1">ملاحظات التعديل </label>
                        <div class="col-sm-9">
                            <textarea id="notes" placeholder="ملاحظات التعديل" class="col-xs-10 col-sm-5"></textarea>
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="col-sm-1 control-label no-padding-right" for="form-field-1">رفع ملف السبب (pdf) (مثلا: تذكرة..)</label>

                        <div class="col-sm-9">
                            <label class="ace-file-input col-xs-10 col-sm-5">
                                <input type="file" id="id-input-file-2" accept="application/pdf,image/jpeg,image/jpg">
                            </label>
                            <button class="btn btn-xs btn-primary" id="uploadpdf"><i class="ace-icon fa fa-cloud-upload"></i>Upload</button>
                        </div>
                    </div>
                </form>
            </div>
        </div>
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
        var within_3Days = false;
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
                    if (within_3Days == false) {
                        if (pdfPath != "" && $("#notes").val() != "") {
                            execuses.NEW_NOTES = $("#notes").val();
                            execuses.NEW_DOC_PATH = pdfPath;

                        }
                        else {

                            Swal.fire(
                                'ملاحظة!',
                                "يجب كتابة سبب التعديل و رفع ملف السبب ... مثلا : تذكرة سفر",
                                'error'
                            )
                            return;
                        }
                    }

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
                                    text: within_3Days == false?  "تم رفع الطلب بنجاح" : "تم تغيير ميعاد المحاضرة المباشرة",
                                    icon: 'success',
                                    showCancelButton: false,
                                    confirmButtonColor: '#3085d6',
                                    cancelButtonColor: '#d33',
                                    confirmButtonText: 'OK!'
                                }).then((result) => {
                                    if (result.value) {
                                        changeType = "";
                                        within_3Days = false;
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
            $('#id-input-file-2').ace_file_input({
                no_file: 'No File ...',
                btn_choose: 'Choose',
                btn_change: 'Change',
                droppable: false,
                onchange: null,
                thumbnail: false, //| true | large
                whitelist: 'pdf'
                //blacklist:'exe|php'
                //onchange:''
                //
            });


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
                        " <tr style='background-color:yellow;'> <td colspan=2>بيانات المحاضرة المراد تغييرها</td></tr>" +
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
                                if (data.within_3Days) {
                                    within_3Days = data.within_3Days;
                                    $("#within3daysReason").hide();
                                }
                                else {
                                    within_3Days = data.within_3Days;
                                    $("#within3daysReason").show();
                                }
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
                            if (data.within_3Days) {
                                within_3Days = data.within_3Days;
                                $("#within3daysReason").hide();
                            }
                            else {
                                within_3Days = data.within_3Days;
                                $("#within3daysReason").show();
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
                }

            }

        });

        //path + "BLL/B_liveDoctors/DoctorsLive.asmx/AvailableDoctorSessions",
        //upload video
        $("#uploadpdf").on("click", function (e) {
            e.preventDefault();
            var files = $("#id-input-file-2").prop("files");
            if (files.length <= 0) {
                Swal.fire(
                    'Notes!',
                    "برجاء رفع سبب تعديل ميعاد المحاضرة المباشرة",
                    'warning'
                )
                return false;
            }

            var ext = files[0].name.toString().match(/\.([^\.]+)$/)[1];
            if (ext.toLowerCase() === "pdf" || ext.toLowerCase() === "jpg") {
                var data = new FormData();
                if (ext.toLowerCase() === "pdf") {
                    $.each(files, function (key, value) {

                        data.append("pdfFile", value);
                    });
                }
                if (ext.toLowerCase() === "jpg") {
                    $.each(files, function (key, value) {

                        data.append("iconFile", value);
                    });
                }

                $(".modal").show();
                $.ajax({
                    type: "POST",
                    url: path + "_handlers/upload.ashx",
                    data: data,
                    processData: false,
                    contentType: false
                }).done(function (data) {
                    //do stuff with the data you got back.

                    if (data === "Not Acceptable" || data === "") { alert("You should select *.pdf or *.jpg file format max size 5 MB"); return; };
                    pdfPath = data;
                    // $("#path").html(path);
                    // alert("File uploaded successfully");
                    Swal.fire(
                        'Notes!',
                        "تم رفع الملف بنجاح , يمكنك الان اختيار تاريخ المحاضرة المرغوب التغيير الية",
                        'success'
                    )

                }).always(function () {
                    $(".modal").hide();

                });
            }
            else { alert("You should select *.pdf or *.jpg file format"); return; };


        });
//end upload file



    </script>
</asp:Content>
