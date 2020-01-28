<%@ Page Title="" Language="C#" MasterPageFile="~/Models/Admin.Master" AutoEventWireup="true" CodeBehind="searchlivesession.aspx.cs" Inherits="ManageLiveSessionWeb.Models.admin.searchlivesession" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <title>Assign Live Session Day</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link rel="stylesheet" href="//cdn.datatables.net/1.10.20/css/jquery.dataTables.min.css" />

    <script src="//cdn.datatables.net/1.10.20/js/jquery.dataTables.min.js"></script>
    <style>
        td, tr {
            font-weight: bold !important;
            font-size: 14px !important;
        }
    </style>
    <h1>
        <img src="../../images/seo.png" width="50px;" />
        Search Live Session</h1>
    <form class="form-horizontal" method="post" id="frmAssignDays">
        <div class="row">
            <div class="col-lg-6">
                <div class="form-group">
                    <label class="control-label col-sm-2" for="user_id">User:</label>
                    <div class="col-sm-10">
                        <select class="form-control" id="user_id" name="user_id">
                        </select>
                    </div>
                </div>
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
                <div class="form-group">
                    <label class="control-label col-sm-2" for="Day">Day:</label>
                    <div class="col-sm-10">
                        <select class="form-control" id="Day" name="Day">
                            <option value="0">ِALL</option>

                            <option value="1">1</option>
                            <option value="2">2</option>
                            <option value="3">3</option>
                            <option value="4">4</option>
                            <option value="5">5</option>
                            <option value="6">6</option>
                            <option value="7">7</option>
                        </select>
                    </div>
                </div>
            </div>

        </div>

        <div class="form-group">
            <div class="col-sm-offset-1     col-sm-10">
                <button type="submit" class="btn btn-primary">Submit</button>
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

            $("#frmAssignDays").submit(function (e) {
                e.preventDefault();
                $(".modal").show();

                debugger
                var str = JSON.stringify({
                    user_id: $("#user_id").val(),
                    termCode: $("#termCode").val(),
                    livesession: $("#livesession").val(),
                    Day: $("#Day").val(),

                });
                $.ajax({
                    url: path + "BLL/B_Admin/adminOp.asmx/SearchLiveSession",
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
                        //if (parseInt(data.d) > 0) {
                        //    swal("Done!", "Live session day " + $("#Day").val() + " Assign to " +  $("#user_id option:selected").text(), "success");
                        //}
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


            $(".modal").show();
            $.ajax({
                url: path + "BLL/B_Admin/adminOp.asmx/SelectUsers",
                type: "POST",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    $("#user_id").empty();

                    $("#user_id").append("<option value='0'>ALL</option>" + data.d);
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
