<%@ Page Title="" Language="C#" MasterPageFile="~/Models/Admin.Master" AutoEventWireup="true" CodeBehind="users.aspx.cs" Inherits="ManageLiveSessionWeb.Models.admin.users" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>User Management
    </title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link rel="stylesheet" href="//cdn.datatables.net/1.10.20/css/jquery.dataTables.min.css" />

    <script src="//cdn.datatables.net/1.10.20/js/jquery.dataTables.min.js"></script>

    <h1>
        <img src="../../assets/images/avatars/user.png" width="50px;" />
        Users</h1>
    <div class="row">
        <div class="col-lg-12">
            <table class="table table-bordered table-hover" id="datatable">
                <thead>
                    <tr>
                        <th>User ID</th>
                        <th>USER NAME</th>
                        <th>MOBILE</th>
                        <th>USER EMAIL</th>
                        <th>ACTIVE</th>
                        <th>Edit</th>
                        <th>Delete</th>

                    </tr>
                </thead>
                <tbody id="users">
                </tbody>
            </table>
        </div>
    </div>
    <hr />

    <div class="widget-box">
        <div class="widget-header">
            <h4 class="smaller">Users
													<small>Add,edit users</small>
            </h4>
        </div>

        <div class="widget-body">
            <div class="widget-main">
                <form class="form-horizontal" id="frmAssignDays">
                    <div class="form-group">
                        <label class="control-label col-sm-2" for="userid">User ID</label>
                        <div class="col-sm-10">
                            <input class="form-control" id="userid" name="userid" required />
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="control-label col-sm-2" for="username">User Name</label>
                        <div class="col-sm-10">
                            <input class="form-control" id="username" name="username" required />
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="control-label col-sm-2" for="mobile">Mobile</label>
                        <div class="col-sm-10">
                            <input class="form-control" id="mobile" name="mobile" required />
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="control-label col-sm-2" for="email">Email Address</label>
                        <div class="col-sm-10">
                            <input class="form-control" id="email" type="email" name="email" required />
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="control-label col-sm-2" for="active">Active</label>
                        <div class="col-sm-10">
                            <select class="form-control" id="active" name="active">
                                <option value="Y">YES</option>
                                <option value="N">NO</option>
                            </select>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-sm-offset-2 col-sm-10">
                            <button type="submit" class="btn btn-primary">Submit</button>

                            <input type="button" class="btn btn-default" onclick="resetForm();" value="Reset">
                        </div>
                    </div>
                </form>
            </div>
        </div>
    </div>


    <script>
        var obj = { ID: 0, USER_ID: "", USER_NAME: "", USER_EMAIL: "", MOBILE: "", ACTIVE: "" };

        function editUser(id) {




            var data = $("#edit" + id).data("info").toString().split('&');

            obj.ID = data[0];
            obj.USER_ID = data[1];
            obj.USER_NAME = data[2];
            obj.MOBILE = data[3];
            obj.USER_EMAIL = data[4];

            obj.ACTIVE = data[5];

            $("#userid").val(obj.USER_ID);
            $("#username").val(obj.USER_NAME);
            $("#email").val(obj.USER_EMAIL);
            $("#mobile").val(obj.MOBILE);
            $("#active").val(obj.ACTIVE);


        }

        function deleteUser(id) {
            if (confirm("Are you sure to delete user?")) {
                var str = JSON.stringify({ ID: id });
                $(".modal").show();
                $.ajax({
                    url: path + "BLL/B_Admin/adminOp.asmx/DeleteUser",
                    type: "POST",
                    dataType: "json",
                    async: false,
                    data: str,
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        if (data.d > 0) {

                            Swal.fire({
                                title: "Done!",
                                text: "User Delete Successfully",
                                icon: 'success',
                                showCancelButton: false,
                                confirmButtonColor: '#3085d6',

                                confirmButtonText: 'Ok!'
                            }).then((result) => {
                                if (result.value) {
                                    loadUsers();
                                }
                            })
                        }
                    },
                    error: function (err) {
                        $(".modal").hide()

                    },
                    complete: function () {

                        $(".modal").hide();
                    }
                });

            }
        }

        var edit = false;
        function resetForm() {
            obj = { ID: 0, USER_ID: "", USER_NAME: "", USER_EMAIL: "", MOBILE: "", ACTIVE: "Y" };

            document.getElementById("frmAssignDays").reset();
        }


    </script>


    <script> 
        function loadUsers() {
            $(".modal").show();

            $.ajax({
                url: path + "BLL/B_Admin/adminOp.asmx/SelectUsersTable",
                type: "POST",
                dataType: "json",
                async: false,
                contentType: "application/json; charset=utf-8",
                success: function (data) {


                    try {
                        if ($.fn.DataTable.isDataTable("#datatable"))
                            $('#datatable').DataTable().clear().destroy();
                    } catch (err) {
                    }



                    $("#users").empty();
                    $("#users").html(data.d);
                },
                error: function (err) {
                    $(".modal").hide()
                    alert(JSON.stringify(err));
                },
                complete: function () {
                    $('#datatable').DataTable({ "ordering": false, "pageLength": 100, "bDestroy": true });
                    $(".modal").hide();
                }
            });

            resetForm();
        }
        $(function () {

            loadUsers();


            $("#frmAssignDays").submit(function (e) {
                e.preventDefault();

                obj.USER_ID = $("#userid").val();
                obj.USER_NAME = $("#username").val();
                obj.USER_EMAIL = $("#email").val();
                obj.MOBILE = $("#mobile").val();
                obj.ACTIVE = $("#active").val();

                var str = JSON.stringify({ 'user': obj });

                $(".modal").show();

                $.ajax({
                    url: path + "BLL/B_Admin/adminOp.asmx/SaveUser",
                    type: "POST",
                    dataType: "json",
                    async: false,
                    data: str,
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        if (data.d > 0) {

                            Swal.fire({
                                title: "Done!",
                                text: data.d == 1 ? "User Updated Successfully" : "User Added Successfully",
                                icon: 'success',
                                showCancelButton: false,
                                confirmButtonColor: '#3085d6',

                                confirmButtonText: 'Ok!'
                            }).then((result) => {
                                if (result.value) {
                                    loadUsers();
                                }
                            })
                        } else {
                            Swal.fire(
                                'already found',
                                'user found with this same User ID ',
                                'error'
                            )
                        }
                    },
                    error: function (err) {
                        $(".modal").hide()

                    },
                    complete: function () {

                        $(".modal").hide();
                    }
                });



            });

        });
    </script>
</asp:Content>
