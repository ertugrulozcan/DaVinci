var LOCALIZATION_SERVICE_URL = "https://ertis.azurewebsites.net/api/localization/";

var currentDictionary = null;

function FetchLocalizationDictionary(culture)
{
	if (window.XMLHttpRequest) {
		// code for modern browsers
		xmlhttp = new XMLHttpRequest();
	}
	else {
		// code for old IE browsers
		xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
	}

	var httpReq = xmlhttp;
	httpReq.onreadystatechange = function () {
		if (this.readyState == 4) {
			if (this.status == 200) {
				console.log("LocalizationData fetched.");
				SetCurrentDictionary(JSON.parse(this.responseText));
			}
		}
	};

	httpReq.open("GET", LOCALIZATION_SERVICE_URL + culture, true);
	httpReq.setRequestHeader('Access-Control-Allow-Headers', '*');
	httpReq.setRequestHeader('Access-Control-Allow-Origin', '*');
	httpReq.setRequestHeader("Content-type", "application/json");
	httpReq.send();
}

function SetCurrentDictionary(dict)
{
	currentDictionary = dict;
	BindStrings(currentDictionary.KeyValueDictionary);
}

function Localize(culture)
{
	console.log("Localize()");

	FetchLocalizationDictionary(culture);
}

function BindStrings(data) {
	$.each(data, function (key, val) {
		try {
			elements = document.getElementsByName(key);
			for (var i = 0; i < elements.length; i++)
			{
				if (elements[i].tagName == 'INPUT')
					elements[i].placeholder = val;
				else
					elements[i].innerHTML = val;
			}
		}
		catch (exc) {
			console.log("Hata : " + exc.message + "(key:" + key + ", value:" + val + ")");
		}
	});
	
	console.log("String veriler bind edildi.");
}