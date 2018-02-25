function IndexOnLoad()
{
	IncludePages();

	console.log("IndexOnLoad()");
}

function IncludePages()
{
	// SideBar
	$.get("sidebar.html", function (data) { $("#sidebar").html(data); });

	// HeaderBar
	$.get("header.html", function (data) { $("#headerbar").html(data); });

	// Dashboard
	$.get("dashboard.html", function (data) { $("#dashboard").html(data); });

	// Content
	$.get("content.html", function (data) { $("#content").html(data); });

	// ChatBox
	$.get("chatbox.html", function (data) { $("#chatbox").html(data); });

	// RightSidebar
	$.get("rightsidebar.html", function (data) { $("#rightsidebar").html(data); });

	// BodyScripts
	$.get("bodyscripts.html", function (data) { $("#bodyscripts").html(data); });
}