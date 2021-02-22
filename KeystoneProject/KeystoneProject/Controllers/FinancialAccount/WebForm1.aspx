<%--<%@ Page  Title="Layout" Language="C#" MasterPageFile="/Views/Shared/_Layout.cshtml"   AutoEventWireup="true"%>--%>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="KeystoneProject.Controllers.FinancialAccount.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="UTF-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no" name="viewport">
    <title></title>
    <link rel="stylesheet" href="http://localhost:3636/Vendor/theme/dist/css/adminlte.min.css" />
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.4.0/css/bootstrap.min.css">
</head>
<body>
    <form id="form1" runat="server">
        <div class="wrapper">
            <h5 class="topheadline">ChequeLayout</h5>
            <section class="content-header">
                <div class="container-fluid">
                    <div class="row">
                        <div class="col-lg-12">
                            <div class="panel-primary">

                                <div class="row">

                                    <div class="col-lg-12">
                                        <div class="col-lg-12" style="background: #bddff7; padding: 0px">
                                            <div class="col-lg-9 padding0" style="font-family: roboto; padding: 4px">
                                                <span style="padding-right: 30px">ChequeLayout</span>

                                            </div>


                                        </div>

                                    </div>
                                </div>

                                <div class="pane-body">

                                    <div class="row">
                                        <div class="col-lg-12  rowmargintop rowmarginbottom">




                                            <div class="col-lg-4">
                                                <div class="col-lg-4">
                                                    <span>Bank Name</span>


                                                </div>
                                                <div class="col-lg-8">


                                                    <asp:DropDownList ID="cmbBankName"  CssClass="form-control" runat="server"></asp:DropDownList>

                                                </div>
                                            </div>
                                            <div class="col-lg-4">
                                                <div class="col-lg-4">
                                                    <span>Leyout Name</span>


                                                </div>
                                                <div class="col-lg-8">
                                                    <asp:TextBox ID="txtChequeLayout" runat="server" CssClass="form-control"></asp:TextBox>

                                                </div>
                                            </div>


                                            <div class="col-lg-4">
                                                <div class="col-lg-4">
                                                    <input type="button" value="Test Print" />


                                                </div>
                                                <div class="col-lg-2">
                                                    <span>Browser</span>


                                                </div>
                                                <div class="col-lg-6">
                                                    <asp:FileUpload ID="btnBrowse" runat="server" />
                                                </div>
                                            </div>







                                        </div>
                                    </div>

                                </div>

                            </div>
                        </div>
                    </div>

                    <div class="row ">
                        <div class="col-md-9 rowmargintop">

                            <!-- Tab panes -->


                            <div class="form-group" style="margin-bottom: 10px;">


                                <div class="col-md-12 padding0 rowmargintop">
                                    <div class="panel-primary" style="box-shadow: none">
                                        <div class="box-header">
                                        </div>
                                        <!-- /.box-header -->

                                        <!-- /.box-body -->
                                    </div>
                                    <!-- /.box -->
                                </div>

                            </div>


                        </div>
                        <div class="col-md-9 rowmargintop padding0">

                            <!-- Tab panes -->


                            <div class="form-group" style="margin-bottom: 10px;">
                                <fieldset class="the-fieldset">

                                    <div class="col-md-12 padding0 rowmargintop">
                                        <div class="panel-primary" style="box-shadow: none">
                                            <div class="box-header">
                                                <div class="x_content">
                                                    <iframe height="400" id="ifrmchkprint" frameborder="0" style="width: 100%">
                                                        <img src="https://www.freeiconspng.com/uploads/patient-icon-8.png" id="img1" name="img1" />

                                                    </iframe>
                                                </div>
                                            </div>
                                            <!-- /.box-header -->
                                            <!-- /.box-body -->
                                        </div>
                                        <!-- /.box -->
                                    </div>
                                </fieldset>
                            </div>


                        </div>

                        <div class="col-lg-3 rowmargintop">
                            <div class="panel-primary" style="box-shadow: none">
                                <div class="box-header with-border">
                                    <h3 class="panelheading">Cheque Setting</h3>
                                </div>

                                <div class="form-horizontal">
                                    <div class="panel-body padding0">

                                        <div class="col-lg-12 padding0 rowmargintop">

                                            <div class="col-lg-12 padding0">

                                                <div class="col-sm-2">
                                                    <span>Height</span>
                                                </div>
                                                <div class="col-sm-4">

                                                    <asp:TextBox ID="txtHieght" CssClass="form-control"  runat="server">5215</asp:TextBox>
                                                </div>
                                                <div class="col-sm-2">
                                                    <span>wight</span>
                                                </div>
                                                <div class="col-sm-4">
                                                    <asp:TextBox ID="txtWidth" CssClass="form-control" runat="server">11520</asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-lg-12 padding0 rowmargintop">

                                            <div class="col-lg-12 padding0">

                                                <div class="col-sm-2">
                                                    <span>Date</span>
                                                </div>
                                                <div class="col-sm-10">
                                                    <asp:DropDownList CssClass="form-control" ID="cmbFieldName" runat="server">
                                                        
                                                         <asp:ListItem>DATE</asp:ListItem>
                                                         <asp:ListItem>>PAYEE NAME</asp:ListItem>
                                                         <asp:ListItem>AMOUNT IN WORD -1</asp:ListItem>
                                                         <asp:ListItem>AMOUNT IN WORD -2</asp:ListItem> 
                                                        <asp:ListItem>AMOUNT</asp:ListItem> 
                                                    </asp:DropDownList>
                                                  
                                                </div>

                                            </div>
                                        </div>
                                        <div class="col-lg-12 padding0 rowmargintop">

                                            <div class="col-lg-12 padding0">

                                                <div class="col-sm-2" style="padding-right: 0px">
                                                    <span>Left</span>
                                                </div>
                                                <div class="col-sm-4">
                                                    <asp:TextBox ID="txtPayeeLeft" CssClass="form-control" runat="server">1080</asp:TextBox>
                                                </div>
                                                <div class="col-sm-2" style="padding-right: 0px">
                                                    <span>Top</span>
                                                </div>
                                                <div class="col-sm-4">
                                                    <asp:TextBox ID="txtPayeeTop" CssClass="form-control" runat="server">1080</asp:TextBox>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-lg-12 padding0 rowmargintop">

                                            <div class="col-lg-12 padding0  ">
                                                <div class="col-sm-12 " style="text-align: -webkit-center;">
                                                  
                                                    <asp:Button ID="btnUp_Click"  runat="server" Text=" ^ " OnClick="btnUp_Click_Click" />

                                                    <br />

                                                   
                                                     <asp:Button ID="btnLeft_Click"  runat="server" Text=" < " OnClick="btnLeft_Click_Click" />

                                                    &nbsp;&nbsp;&nbsp;
                                                     <asp:Button ID="btnRieght_Click"  runat="server" Text=" > " OnClick="btnRieght_Click_Click" />
                                                    <br />
                                                    <input type="button" id="btnDown_Click" value=" V " />
                                                </div>

                                            </div>
                                        </div>
                                        <div class="col-lg-12 padding0 rowmargintop rowmarginbottom">

                                            <div class="col-lg-12 padding0">

                                                <div class="col-sm-12" style="padding-right: 0px">
                                                    <table id="datatable-responsive" class="table table-striped table-bordered dataTable">
                                                        <thead>
                                                            <tr>
                                                                <th>LayOut Name</th>
                                                                <th>Background</th>
                                                                <th>ChequeLayoutID</th>
                                                                <th>BankID</th>
                                                            </tr>
                                                        </thead>
                                                        <tbody id="tbl">
                                                        </tbody>
                                                    </table>
                                                </div>


                                            </div>
                                        </div>
                                    </div>

                                </div>

                            </div>

                        </div>
                    </div>

                    <div class="col-lg-12" style="text-align: center; display: block; margin-top: 10px">
                        <div class="btn-group dropdown-split-warning">

                            <button type="submit" class="btndivcss"><span class="fa fa-save buttoniconcss"></span>Save</button>


                        </div>
                        <div class="btn-group dropdown-split-warning">

                            <button type="button" class="btndivcss"><span class="fa fa-trash buttoniconcss"></span>Delete</button>


                        </div>
                        <div class="btn-group dropdown-split-warning">

                            <button type="reset" class="btndivcss"><span class="fa fa-refresh buttoniconcss"></span>Clear</button>


                        </div>
                        <div class="btn-group dropdown-split-warning">

                            <button type="button" class="btndivcss"><span class="fa fa-sign-out buttoniconcss"></span>Exit</button>


                        </div>

                    </div>

                </div>
            </section>
        </div>
    </form>

    <script src="https://adminlte.io/themes/AdminLTE/bower_components/jquery-ui/jquery-ui.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/4.0.0/js/bootstrap.min.js"></script>

</body>
</html>
