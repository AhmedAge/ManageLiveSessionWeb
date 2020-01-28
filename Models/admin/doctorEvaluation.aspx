<%@ Page Title="" Language="C#" MasterPageFile="~/Models/Admin.Master" AutoEventWireup="true" CodeBehind="doctorEvaluation.aspx.cs" Inherits="ManageLiveSessionWeb.Models.admin.doctorEvaluation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>تقييم الاستاذ
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

        table {
            /*width: 100%;*/
            /*table-layout: fixed;*/
            overflow-wrap: break-word;
            border-collapse: collapse !important;
        }

        table, td, th {
            border: 1px solid black;
            text-align: center !important;
            vertical-align: middle !important;
            font-size: 17px !important;
            font-weight: bold !important;
        }

        textarea {
            border: 1px solid black;
            vertical-align: middle !important;
            font-size: 17px !important;
            font-weight: bold !important;
        }



        td > input {
            width: 100%;
        }

        .widget-header > .widget-title {
            line-height: 36px;
            padding: 0;
            margin: 15px !important;
            display: inline;
            font-weight: bold;
        }

        select > option, select {
            font-size: large !important;
        }
    </style>

    <h2>Doctor Evaluation</h2>
    <form method="post" id="frmEval">
        <div class="row">
            <div class="col-lg-6">
                <h2>اختر المدرس </h2>

                <select class="form-control" id="coursenumber" name="coursenumber" required>
                </select>
            </div>
        </div>
        <hr />
        <div class="row">

            <div class="col-lg-12">
                <table class="table table-bordered table-hover" dir="rtl">
                    <thead>
                        <tr>
                            <th>م</th>
                            <th>السؤال</th>
                            <th>نعم</th>
                            <th>لا</th>
                            <th>ملاحظات</th>
                        </tr>
                    </thead>
                    <tbody id="quest">
                    </tbody>
                </table>
            </div>
        </div>
        <div class="row">
            <div class="col-lg-12" dir="rtl">

                <div class="widget-box">
                    <div class="widget-header">
                        <h4 class="widget-title">ملاحظات</h4>
                    </div>

                    <div class="widget-body">
                        <div class="widget-main">
                            <textarea class="form-control" id="GeneralNotes" placeholder="ملاحظات"></textarea>

                        </div>
                    </div>
                </div>



            </div>
            <div class="col-lg-12 text-center">
                <br />
                <button type="submit" class="btn btn-primary" id="check"><i class="ace-icon fa fa-floppy-o bigger-120"></i>Save</button>
            </div>
        </div>
    </form>

    <script>
        var questionLst = 0;
        $(function () {
            var data1 = localStorage.getItem("studioInfo");
            if (data1 == undefined) {
                window.close();
                return;
            }

            var data = JSON.parse(data1);
            var answers = JSON.stringify({ term: data.term, liveSession: data.liveSession, stdInfo: data.info });


            $(".modal").show();
            $.ajax({
                url: path + "BLL/B_Admin/adminOp.asmx/DayStudioSessions",
                type: "POST",
                dataType: "json",
                async: true,
                data: answers,
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    $("#coursenumber").empty();

                    $("#coursenumber").append("<option value='0'>اختر المدرس</option>" + data.d);
                },
                error: function (err) {
                    alert(JSON.stringify(err));
                },
                complete: function () {
                    $(".modal").hide();
                }
            });

            $("#coursenumber").change(function () {
                var data1 = localStorage.getItem("studioInfo");
                if (data1 == undefined) {
                    window.close();
                    return;
                }
                var data = JSON.parse(data1);

                var condition = JSON.stringify({ term: data.term, liveSession: data.liveSession, stdInfo: data.info, course_doctor: $("#coursenumber").val() });


                $(".modal").show();
                $.ajax({
                    url: path + "BLL/B_Admin/adminOp.asmx/EvalQuestions",
                    type: "POST",
                    dataType: "json",
                    data: condition,
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {

                        if (data.d == "") {
                            $("#quest").html("<h1>لا توجد بيانات متاحة</h1>");
                            return;
                        }

                        var table = JSON.parse(data.d);

                        var str = "";

                        $.each(table, function (index, value) {
                            if (value.ANSWER == '' && value.QUESTIONID != '0') {
                                str += "<tr><td>" + parseInt(index + 1) + "</td><td>" + value.QUESTIONTEXT + "</td><td> <label class='inline' > " +
                                    "<input name='form-field-radio" + index + "' data-check='yes' data-id='" + value.QUESTIONID + "'  type='radio' class='ace input-lg'> " +
                                    "       <span class='lbl' style='font-size:17px;color:black;font-weight:bold;'></span>" +
                                    "                                      </label></td><td><label class='inline' > " +
                                    "<input name='form-field-radio" + index + "' data-check='no' data-id='" + value.QUESTIONID + "' type='radio' class='ace input-lg'> " +
                                    "       <span class='lbl' style='font-size:17px;color:black;font-weight:bold;'></span>" +
                                    "                                      </label></td><td><input type='text' placeholder='ملاحظات'  id='notes" + value.QUESTIONID + "' /></td></tr>";
                                $("#GeneralNotes").val("");
                                questionLst = table.length;
                            }
                            else {
                                if (value.QUESTIONID == '0') {
                                    $("#GeneralNotes").val(value.NOTES);
                                    return;
                                }
                                var yes = '';
                                var no = '';
                                var notes = '';
                                if (value.ANSWER == 'Y') {
                                    yes = 'checked';
                                    no = '';
                                } else if (value.ANSWER == 'N') {
                                    yes = '';
                                    no = 'checked';
                                }
                                str += "<tr><td>" + parseInt(index + 1) + "</td><td>" + value.QUESTIONTEXT + "</td><td> <label class='inline' > " +
                                    "<input name='form-field-radio" + index + "' data-check='yes' data-id='" + value.QUESTIONID + "'  " + yes + " type='radio' class='ace input-lg'> " +
                                    "       <span class='lbl' style='font-size:17px;color:black;font-weight:bold;'></span>" +
                                    "                                      </label></td><td><label class='inline' > " +
                                    "<input name='form-field-radio" + index + "' data-check='no' data-id='" + value.QUESTIONID + "'  " + no + "  type='radio' class='ace input-lg'> " +
                                    "       <span class='lbl' style='font-size:17px;color:black;font-weight:bold;'></span>" +
                                    "                                      </label></td><td><input type='text' placeholder='ملاحظات' value='" + value.NOTES + "'  id='notes" + value.QUESTIONID + "' /></td></tr>";
                                questionLst = table.length - 1;
                            }
                        });

                        table = null;

                        $("#quest").html(str);
                    },
                    error: function (err) {
                        alert(JSON.stringify(err));
                    },
                    complete: function () {
                        $(".modal").hide();
                    }
                });
            });


            $("#frmEval").submit(function (e) {
                e.preventDefault();

                var lst = $("input[type='radio']:checked");
                if (lst.length < questionLst) {
                    alert("يجب الاجابة علي جميع الأسئلة ");
                    return;
                }

                var questionsAns = [];
                $.each(lst, function (i, v) {
                    var singleQ = { ID: "", Yes: "", No: "", Notes: "" };
                    singleQ.ID = $(v).data("id");
                    singleQ.Yes = $(v).data("check") == "yes" ? "yes" : "no";

                    singleQ.No = $(v).data("check") == "no" ? "yes" : "no";
                    singleQ.Notes = $("#notes" + singleQ.ID).val();

                    questionsAns.push(singleQ);


                });



                if (confirm("هل انت متأكدة من ارسال التقييم")) {

                    var data1 = localStorage.getItem("studioInfo");
                    if (data1 == undefined) {
                        window.close();
                        return;
                    }

                    var data = JSON.parse(data1);
                    var answers = { user: data.user, term: data.term, liveSession: data.liveSession, info: data.info, coursenumber: $("#coursenumber").val(), GeneralNotes: $("#GeneralNotes").val(), questionsInfo: questionsAns };


                    var str = JSON.stringify({ answer: answers });

                    $.ajax({
                        url: path + "BLL/B_Admin/adminOp.asmx/DoctorEvaluation",
                        type: "POST",
                        dataType: "json",
                        async: false,
                        data: str,
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {

                            Swal.fire({
                                title: 'مبروك!',
                                text: data.d,
                                icon: 'success',
                                showCancelButton: false,
                                confirmButtonColor: '#3085d6',
                                confirmButtonText: 'تم اضافة التقييم'
                            }).then((result) => {
                                if (result.value) {
                                    $("#quest").empty();
                                    $("#coursenumber").val(0);
                                    $("#GeneralNotes").val("");
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
            });

        });
    </script>
</asp:Content>
