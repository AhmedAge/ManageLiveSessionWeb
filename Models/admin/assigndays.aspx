<%@ Page Title="" Language="C#" MasterPageFile="~/Models/Admin.Master" AutoEventWireup="true" CodeBehind="assigndays.aspx.cs" Inherits="ManageLiveSessionWeb.Models.admin.assigndays" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <title>Manage Live Session Day</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link rel="stylesheet" href="//cdn.datatables.net/1.10.20/css/jquery.dataTables.min.css" />

    <script src="//cdn.datatables.net/1.10.20/js/jquery.dataTables.min.js"></script>
    <style>
          .table>tbody>tr>td, .table>tbody>tr>th,  .table>thead>tr>td, .table>thead>tr>th{
             width:12.5%!important;
               vertical-align: middle!important;
               text-align: center!important;
             
        }

          .table>tbody>tr>th,   .table>thead>tr>th{
            font-weight:bold!important;
            font-size:18px!important;
             
        }

            td, tr {
            font-weight: bold !important;
            font-size: 14px !important;
        }

    </style>

      <div class="col-sm-12">
        <h3 class="header smaller lighter red">Show Users Days <button id="ShowUsersDays" class="btn btn-info btn-md">
										 
											Show
										</button>
        </h3>

         
             <table class='table  table-bordered table-hover compact' id='datatable1'>
                </table>

         
 
    </div>

    <h1>
        <img src="../../images/settings.png" width="50px;" />
        Manage Live Session Day</h1>
    <form class="form-horizontal" method="post" id="frmAssignDays">
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
         <div class="form-group">
            <label class="control-label col-sm-1" for="SESSION_NO">SESSION NO:</label>
            <div class="col-sm-11">
                <select class="form-control" id="SESSION_NO" name="SESSION_NO">
                    <option value="ALL">ALL</option> 
                    <option value="session1">SESSION 1 - الفترة الأولي</option>
                    <option value="session2">SESSION 2 - الفترة الثانية</option>
                    <option value="session3">SESSION 3 - الفترة الثالثة</option> 
                </select>
            </div>
        </div>
        <div class="form-group">
            <div class="col-sm-offset-1 col-sm-11">
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


        function Assign() {

            var str = JSON.stringify({
                user_id: $("#user_id").val(),
                termCode: $("#termCode").val(),
                livesession: $("#livesession").val(),
                Day: $("#Day").val(),
                SESSION_NO: $("#SESSION_NO").val()

            });

            $.ajax({
                url: path + "BLL/B_Admin/adminOp.asmx/AssignDay",
                type: "POST",
                dataType: "json",
                async: false,
                data: str,
                contentType: "application/json; charset=utf-8",
                success: function (data) {

                    //$("#data").empty();

                    // $("#dataTable").dataTable().fnDestroy();
                    try {
                        if ($.fn.DataTable.isDataTable("#datatable"))
                            $('#datatable').DataTable().clear().destroy();
                    } catch (err) {
                    }

                    $("#datatable").html(data.d);
                    if (data.d != "لا يوجد بيانات متاحة") {
                        Swal.fire("Done!", "Live session day " + $("#Day").val() + " Assign to " + $("#user_id option:selected").text(), "success");
                    }
                },
                error: function (err) {
                    alert(JSON.stringify(err));
                },
                complete: function () {
                    $('#datatable').DataTable({ "ordering": false, "pageLength": 100, "bDestroy": true });

                    $("#ShowUsersDays").trigger("click");
                    $(".modal").hide();


                }
            });
        }

        $(function () {

            $("#frmAssignDays").submit(function (e) {
                e.preventDefault();
                $(".modal").show();

                Swal.fire({
                    title: 'Are you sure?',
                    text: "You want to assign Live Session Day!",
                    icon: 'warning',
                    showCancelButton: true,
                    confirmButtonColor: '#3085d6',
                    cancelButtonColor: '#d33',
                    confirmButtonText: 'Yes, Assign Day!'
                }).then((result) => {
                    if (result.value) {

                        var str = JSON.stringify({
                            user_id: $("#user_id").val(),
                            termCode: $("#termCode").val(),
                            livesession: $("#livesession").val(),
                            Day: $("#Day").val(),
                            SESSION_NO: $("#SESSION_NO").val()
                        });

                        $.ajax({
                            url: path + "BLL/B_Admin/adminOp.asmx/CheckAssignDay",
                            type: "POST",
                            dataType: "json",
                            async: false,
                            data: str,
                            contentType: "application/json; charset=utf-8",
                            success: function (data) {
                                debugger;
                                if (data.d == "0") {
                                    Assign();

                                } else {
                                    Swal.fire({
                                        title: 'Are you sure?',
                                        text: data.d,
                                        icon: 'warning',
                                        showCancelButton: true,
                                        confirmButtonColor: '#3085d6',
                                        cancelButtonColor: '#d33',
                                        confirmButtonText: 'Yes, Assign Day!'
                                    }).then((result) => {
                                        if (result.value) {
                                            Assign();
                                        }
                                    }); 
                                } 
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
                        $(".modal").hide();
                    }
                })



            });


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
                }
            });


        });
    </script>

</asp:Content>
