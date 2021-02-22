<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RptPatientPrescriptionNew.aspx.cs" Inherits="KeystoneProject.Report.RptPatientPrescriptionNew" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">


    <title></title>
    <link href="../Content/bootstrap.css" rel="stylesheet" />
    <link href="../Content/bootstrap.min.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">

       <div class="wrapper"> 
        <div class="content-wrapper">
              <section class="content-header">
                <div class="container-fluid">

                    <div>
                        <div class="row">
                            <div class="col-md-12 padding0 rowmargintop" style="width: 100%;">
                                <div class="col-md-6">
                                    <div class="col-md-4">
                                        <asp:Label ID="lbl" runat="server">Language</asp:Label>
                                    </div>
                                    <div class="col-md-8">
                                         <asp:DropDownList CssClass="form-control"   ID="ddllang" runat="server">
                                             <asp:ListItem  Value="English">English</asp:ListItem>  
                                             <asp:ListItem Value="Hindi">Hindi</asp:ListItem>  
                                             <asp:ListItem Value="Marathi">Marathi</asp:ListItem>  
                                             <asp:ListItem Value="Arabic">Arabic</asp:ListItem>  
                                             <asp:ListItem Value="Gujarati">Gujarati</asp:ListItem>  
                                             <asp:ListItem Value="Tamil">Tamil</asp:ListItem> 
                                              <asp:ListItem Value="Urdu">Urdu</asp:ListItem> 
                                              <asp:ListItem Value="Bengali">Bengali</asp:ListItem> 
                                              <asp:ListItem Value="Korean">Korean</asp:ListItem>  
                                             <asp:ListItem Value="French">French</asp:ListItem> 
                                   </asp:DropDownList>
                                    </div>
                                  
                                </div>
                                 <div class="col-md-6">
                                    
                                    
                                    <asp:Button CssClass="form-control" ID="btnprint" OnClick="btnprint_Click" runat="server" Text="Print"/>
                                  
                                </div>
                            </div>
                        </div>

                    </div>
                </div>
            </section>
            </div>
           </div>
    <div>
      <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
            <%--<CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="true" />--%>
             <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" HasCrystalLogo="False" BestFitPage="True" EnableDatabaseLogonPrompt="false" AutoDataBind="true" ToolPanelView="None" />
            <%--<CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="true" />--%>
    </div>
    </form>
</body>
</html>
