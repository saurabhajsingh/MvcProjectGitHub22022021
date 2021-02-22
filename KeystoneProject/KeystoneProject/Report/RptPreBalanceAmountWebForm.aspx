<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RptPreBalanceAmountWebForm.aspx.cs" Inherits="KeystoneProject.Report.RptPreBalanceAmountWebForm" %>


<%--<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>--%>




<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script>
       
      

    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
            
            <%--<CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="true" />--%>
     <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" HasCrystalLogo="False" BestFitPage="True" EnableDatabaseLogonPrompt="false" AutoDataBind="true" ToolPanelView="None" />
            <%--<CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="true" />--%>
        </div>
     
    </form>
</body>
</html>

