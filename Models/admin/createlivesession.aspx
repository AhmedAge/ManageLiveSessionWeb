<%@ Page Title="" Language="C#" MasterPageFile="~/Models/Admin.Master" AutoEventWireup="true" CodeBehind="createlivesession.aspx.cs" Inherits="ManageLiveSessionWeb.Models.admin.createlivesession" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <title>Creation Live Session</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link rel="stylesheet" href="//cdn.datatables.net/1.10.20/css/jquery.dataTables.min.css" />

    <script src="//cdn.datatables.net/1.10.20/js/jquery.dataTables.min.js"></script>
    <link href="https://fonts.googleapis.com/css?family=Cairo&display=swap" rel="stylesheet">

    <style> 

        td, tr {
            font-weight: bold !important;
            font-size: 14px !important;
        }

        a {
            cursor: pointer !important;
        }

        .message-star {
            vertical-align: middle;
            margin: 2px 4px 0 6px;
            font-size: 20px !important;
            cursor: pointer;
        }
      
      
    </style>
     
    <h1>
        <img src="../../images/create.png" width="50px;" />
        Creation Live Session Collaborate</h1>
    <form class="form-horizontal" method="post" id="frmAssignDays">
        <div class="row">
            <div class="col-lg-12">
                <div class="form-group">
                    <label class="control-label col-sm-1" for="user_id">User:</label>
                    <div class="col-sm-11">
                        <select class="form-control" id="user_id" name="user_id">
                        </select>
                    </div>
                </div>

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
                    <label class="control-label col-sm-1" for="Day">Day:</label>
                    <div class="col-sm-11">
                        <select class="form-control" id="Day" name="Day">
                           <%-- <option value="1">1</option>
                            <option value="2">2</option>
                            <option value="3">3</option>
                            <option value="4">4</option>
                            <option value="5">5</option>
                            <option value="6">6</option>
                            <option value="7">7</option>--%>
                        </select>
                    </div>
                </div>
            </div>
        </div>

        <div class="form-group">
            <div class="col-sm-offset-1     col-sm-11">
                <button type="submit" class="btn btn-danger">Create</button>
            </div>
        </div>

    </form>
    <br />
    <div class="row">
        <div class="col-lg-12">
            <div id="userDay">
                <table class='table table-boardered table-striped' id='datatable'>
                </table>
            </div>
        </div>
    </div>

    <script>

        $(function () {
            $("#ShowUsersDays").click(function (e) {
                e.preventDefault();
                $(".modal").show();
                var str = JSON.stringify({
                    termCode: $("#termCode").val(),
                    livesession: $("#livesession").val()
                });

                $.ajax({
                    url: path + "BLL/B_Admin/adminOp.asmx/ShowUsersDays",
                    type: "POST",
                    dataType: "json",
                    async: false, 
                    data: str,
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        $("#datatable1").html(data.d);
                    },
                    error: function (err) {
                        $(".modal").hide()
                        alert(JSON.stringify(err));
                    },
                    complete: function () {
                        $(".modal").hide();
                    }
                });


            });

        });
        function addEntryFemale(id) {
            $(".modal").show();


            copyToClipboard("addfemale" + id);

            var str = JSON.stringify({
                user_id: $("#user_id").val(),
                termCode: $("#termCode").val(),
                livesession: $("#livesession").val(),
                Day: $("#Day").val(),
                SESSION_NO: $("#addfemale" + id).data("sessionno"),
                STUDIO_NO: $("#addfemale" + id).data("studiono"),
                GENDER: $("#addfemale" + id).data("gender"),
            });
            $.ajax({
                url: path + "BLL/B_Admin/adminOp.asmx/AddEntry_CreateLiveSession",
                type: "POST",
                dataType: "json",
                async: false,
                data: str,
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    if (parseInt(data.d) <= 0)
                        alert("Error in mark record");
                },
                error: function (err) {
                    $(".modal").hide()
                    alert(JSON.stringify(err));
                },
                complete: function () {
                    $(".modal").hide();
                }
            });


            window.open($("#addfemale" + id).data("link"), '_blank');


        }
        function addEntryMale(id) {
            $(".modal").show();

            copyToClipboard("addmale" + id);

            var str = JSON.stringify({
                user_id: $("#user_id").val(),
                termCode: $("#termCode").val(),
                livesession: $("#livesession").val(),
                Day: $("#Day").val(),
                SESSION_NO: $("#addmale" + id).data("sessionno"),
                STUDIO_NO: $("#addmale" + id).data("studiono"),
                GENDER: $("#addmale" + id).data("gender"),
            });
            $.ajax({
                url: path + "BLL/B_Admin/adminOp.asmx/AddEntry_CreateLiveSession",
                type: "POST",
                dataType: "json",
                async: false,
                data: str,
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    if (parseInt(data.d) <= 0)
                        alert("Error in mark record");
                },
                error: function (err) {
                    $(".modal").hide()
                    alert(JSON.stringify(err));
                },
                complete: function () {
                    $(".modal").hide();
                }
            });


            window.open($("#addmale" + id).data("link"), '_blank');


        }

        $(function () {

            $(".modal").show();
            $.ajax({
                url: path + "BLL/B_Admin/adminOp.asmx/SelectUsers",
                type: "POST",
                dataType: "json",
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
                    $("#user_id").trigger("change");
                }
            });


        
            $("#user_id").change(function () {
                $(".modal").show();
                debugger
                var str = JSON.stringify({
                    user_id: $("#user_id").val(),
                    termCode: $("#termCode").val(),
                    livesession: $("#livesession").val()
                });
                $.ajax({
                    url: path + "BLL/B_Admin/adminOp.asmx/User_days",
                    type: "POST",
                    dataType: "json",
                    async: false,
                    data: str,
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        $("#Day").empty();
                        $("#Day").append(data.d);
                    },
                    error: function (err) {
                        $(".modal").hide()
                        alert(JSON.stringify(err));
                    },
                    complete: function () {
                        $(".modal").hide();
                    }
                });



            });

            

            $("#frmAssignDays").submit(function (e) {
                e.preventDefault();
                $(".modal").show();

                var str = JSON.stringify({
                    user_id: $("#user_id").val(),
                    termCode: $("#termCode").val(),
                    livesession: $("#livesession").val(),
                    Day: $("#Day").val()
                });
                $.ajax({
                    url: path + "BLL/B_Admin/adminOp.asmx/CreateSearchLiveSession",
                    type: "POST",
                    dataType: "json",
                    async: false,
                    data: str,
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {

                        //$("#data").empty();

                        // $("#dataTable").dataTable().fnDestroy();
                        try {
                            if ($.fn.DataTable.isDataTable("#datatable")) {
                                // $('#datatable').DataTable().clear().destroy();

                                $('#datatable').DataTable().destroy();
                                $('#example tbody').empty();
                            }
                        } catch (err) {
                        }

                        $("#datatable").html(data.d);
                    },
                    error: function (err) {
                        alert(JSON.stringify(err));
                    },
                    complete: function () {

                        $('#datatable').DataTable({ "ordering": false, "pageLength": 100, "bDestroy": true });
                        //{
                        //    "pageLength": 50,

                        //    "bDestroy": true
                        //});
                        $(".modal").hide();

                    }
                });

            });
        });


        function copyToClipboard(elementId) {

            // Create a "hidden" input
            var aux = document.createElement("input");

            // Assign it the value of the specified element
            aux.setAttribute("value", document.getElementById(elementId).innerHTML);

            // Append it to the body
            document.body.appendChild(aux);

            // Highlight its content
            aux.select();

            // Copy the highlighted text
            document.execCommand("copy");

            // Remove it from the body
            document.body.removeChild(aux);

        }


    </script>

</asp:Content>
