<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="OraclePasswordChanger.Index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
      <meta charset="utf-8"/>
        <title>Oracle Account Manager</title>
        <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
        <link rel="icon" href="favico.ico" />
        <link href="css/normalize.css" rel="stylesheet" type="text/css" media="all"/
        <link href="css/font-awesome.min.css" rel="stylesheet" type="text/css" media="all"/>
        <link href="css/themify-icons.css" rel="stylesheet" type="text/css" media="all" />
        <link href="css/bootstrap.css" rel="stylesheet" type="text/css" media="all" />
        <link href="css/theme.css" rel="stylesheet" type="text/css" media="all" />
        <link href="css/custom.css" rel="stylesheet" type="text/css" media="all" />
        <link href='http://fonts.googleapis.com/css?family=Lato:300,400%7CRaleway:100,400,300,500,600,700%7COpen+Sans:400,500,600' rel='stylesheet' type='text/css'/>
        <link href="http://fonts.googleapis.com/css?family=Roboto+Condensed:100,300,400,700,700italic" rel="stylesheet" type="text/css"/>
        <link href="css/font-robotocondensed.css" rel="stylesheet" type="text/css"/>
        <link href="css/jquery-ui.css" rel="stylesheet" media="all"/>
</head>
<body class="btn-rounded scroll-assist">
    <form id="form1" runat="server">
   <div class="nav-container">
		</div>
		
		<div class="main-container">
		<section>
		        <div class="container">
		            <div class="row"> 
		                <div class="col-sm-6 center-block">
                            <div id="tabs" class="feature bordered">
                                <div class="header">
                                <img id="logo" src="favico.ico"/>
		                        <h3 class="uppercase mb-xs-24" style="margin-bottom:20px">Oracle Account Manager</h3>
                                </div>
                                <ul>
                                <li><a href="#tabs-1">Reset Password</a></li>
                                <li><a href="#tabs-2">Unlock Account</a></li>
                                </ul>
                                <div id="tabs-1">

                                    <div class="inputGroup">
                                        <h5>Database :</h5>
                                        <input type="search" id="searchQuery"  runat="server"/>
                                    </div>
                                    <div class="inputGroup">
                                        <h5>Username :</h5>
                                        <asp:TextBox id="tb_Username" runat="server"></asp:TextBox>
                                    </div>
                                    <div class="inputGroup">
                                        <h5>New Password :</h5>
                                        <asp:TextBox id="tb_Password" TextMode="Password" runat="server"></asp:TextBox> <asp:Button ID="b_GeneratePassword" Text="Generate" OnClick="Generate_Password" runat="server"/>
                                    </div>
                                    <div class="inputGroup">
                                    <h5>Reset Reason :</h5>
                                        <asp:TextBox id="tb_Reason" runat="server"></asp:TextBox>
                                    </div>
                                    <asp:Button text="Reset Password" runat="server" OnClick="Reset_Password"/>
                                    <asp:Label id="l_Success" runat="server"></asp:Label>            
                                    <asp:Label id="l_Error" runat="server"></asp:Label>                 
                                </div>
                                <div id="tabs-2">
                                    <div class="inputGroup">
                                        <h5>Database :</h5>
                                        <input type="search" id="searchQuery2"  runat="server"/>
                                    </div>
                                    <div class="inputGroup">
                                        <h5>Account :</h5>
                                        <asp:TextBox id="tb_Username2" runat="server"></asp:TextBox>
                                    </div>
                                    <div class="inputGroup">
                                        <h5>Email Recipients :</h5>
                                        <asp:TextBox id="tb_Recipients" placeholder="user1;user2;user3" runat="server"></asp:TextBox>
                                    </div>
                                    <div class="inputGroup">
                                    <h5>Unlock Reason :</h5>
                                        <asp:TextBox id="tb_Reason2" runat="server"></asp:TextBox>
                                    </div>
                                    <asp:Button text="Unlock Account" runat="server" OnClick="Unlock_Account"/>
                                    <asp:Label id="l_Success2" runat="server"></asp:Label>            
                                    <asp:Label id="l_Error2" runat="server"></asp:Label>                 
                                </div>
                            </div>
		                </div>
		                
		            </div>
		            
		        </div>
		        
		    </section>			
			
            <br />
				
 	
			<footer class="footer-2 bg-dark text-center-xs">
				<div class="container">
					<div class="row">
						<div class="col-sm-4">
							<a href="http://alice/"><img class="image-xxs fade-half" alt="Pic" src="img/weblogo.png"/></a>
						</div>
					
						<div class="col-sm-4 text-center">
							<span class="fade-half">
								Only members of the Identity Operations Team are authorized to use this tool.
							</span>
						</div>

                      	<div class="col-sm-4 text-right">
                            <span class="fade-half">
								  Last Updated by <a href="https://staffdirectory.pc.internal.macquarie.com/UserProfile.aspx?u=kolanday" target="_blank">kolanday</a>
							</span>
						</div>
					

					</div>
				</div>
			</footer>
		</div>
		
				
	    <script src="js/jquery.min.js"></script>
        <script src="js/bootstrap.min.js"></script>
        <script src="js/parallax.js"></script>
        <script src="js/scripts.js"></script>
        <script src="js/scripts.js"></script>		
        <script src="js/jquery-ui.min.js" type="text/javascript"></script>
                 <script>
                     $(function () {
                         $("#tabs").tabs();
                     });
       </script>
       <script>
           function selectTab1() {
               var index = $('#tabs li a').index($('a[href="#tabs-1"]').get(0));
               $('#tabs').tabs({ selected: index });
           }
           function selectTab2() {
               var index = $('#tabs li a').index($('a[href="#tabs-2"]').get(0));
               $('#tabs').tabs({ selected: index });
           }
       </script>
        
               
        <script type="text/javascript">
            $(function () {
                $("[id$=searchQuery]").autocomplete({
                    source: function (request, response) {
                        $.ajax({
                            url: '<%=ResolveUrl("Index.aspx/Get_Databases") %>',
                                data: "{ 'prefix': '" + request.term + "'}",
                                dataType: "json",
                                type: "POST",
                                contentType: "application/json; charset=utf-8",
                                success: function (data) {
                                    response($.map(data.d, function (item) {
                                        return {
                                            label: item,
                                            val: item
                                        }
                                    }))
                                },
                                error: function (response) {
                                    alert(response.responseText);
                                },
                                failure: function (response) {
                                    alert(response.responseText);
                                }
                            });
                        },
                        select: function (e, i) {
                            $("[id$=hfCustomerId]").val(i.item.val);
                        },
                        minLength: 1
                    });
                });
    </script>
         <script type="text/javascript">
             $(function () {
                 $("[id$=searchQuery2]").autocomplete({
                     source: function (request, response) {
                         $.ajax({
                             url: '<%=ResolveUrl("Index.aspx/Get_Databases") %>',
                            data: "{ 'prefix': '" + request.term + "'}",
                            dataType: "json",
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            success: function (data) {
                                response($.map(data.d, function (item) {
                                    return {
                                        label: item,
                                        val: item
                                    }
                                }))
                            },
                            error: function (response) {
                                alert(response.responseText);
                            },
                            failure: function (response) {
                                alert(response.responseText);
                            }
                        });
                    },
                    select: function (e, i) {
                        $("[id$=hfCustomerId]").val(i.item.val);
                    },
                    minLength: 1
                });
            });
    </script>
    </form>
</body>
</html>
