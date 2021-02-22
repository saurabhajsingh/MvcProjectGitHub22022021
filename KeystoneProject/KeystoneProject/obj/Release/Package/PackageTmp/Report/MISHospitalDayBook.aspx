
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MISHospitalDayBook.aspx.cs" Inherits="KeystoneProject.Report.MISHospitalDayBook" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form2" runat="server">
        <div>
            <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
            <%--<CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="true" />--%>
             <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" HasCrystalLogo="False" BestFitPage="True" EnableDatabaseLogonPrompt="false" AutoDataBind="true" ToolPanelView="None" />
            <%--<CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="true" />--%>
        </div>
    </form>
</body>
</html>
