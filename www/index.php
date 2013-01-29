<?php
require('./phpqrcode/qrlib.php');
session_start();
if(file_exists('photos/'.session_id().'.jpg')) {
	unlink('photos/'.session_id().'.jpg');
}
QRcode::png(session_id(), 'qr/'.session_id().'.png', 'L', 16, 1);
?>
<html>
<title>PhotoRay</title>
<script type="text/javascript" src="jquery-1.9.0.min.js"></script>
<script type="text/javascript" src="jquery-ui-1.10.0.custom.min.js"></script>
<body style="color: white; font-family: segoe ui; text-align: center; padding: 0; margin: 0; background: black url('loader.gif') no-repeat center center;">
<div id="qr">
	<h1>PHOTORAY</h1>
	<br /><br />
	<img id="qrimg" src="qr/<?php echo session_id() ?>.png" />
	<br />
</div>
<img id="photo" style="height: 100%; display: none;" />
</body>
<script type="text/javascript">
var prevDate = -1;
var busy = false;
function updatePhotoSrc() {
	$("#photo").attr("src", "photos/<?php echo session_id() ?>.jpg?" + new Date().getTime());
}
function tryGetPhoto() {
if(!busy) {
$.ajax({ 
		url: 'check.php?' + new Date().getTime(),
	    processData: false,
	    success: function(result) {
	    	if(result == -1) {
	    		$("#photo").hide();
	    		$("#qr").show();
	    	} else if(result != prevDate) {
	    		busy = true;
	    		prevDate = result;
	    		if($("#qr").is(":visible")) {
	    			$("#qr").hide();
	    		}
	    		if($("#photo").is(":visible")) {
		    		$("#photo").hide("fade", 500, function() {
		    			updatePhotoSrc();
		    		});
	    		} else {
	    			updatePhotoSrc();
	    		}
			}
	    }
	});
}
window.setTimeout(function() {
	tryGetPhoto();
}, 500);
}
$("#qrimg").load(function() {
	$.ajax({ 
			url: 'delete.php',
		    processData: false
		});
});
$("#photo").load(function() {
	$("#photo").show("fade", 500, function() {
		busy = false;
	});
});
$("#photo").hide();
window.setTimeout(function() {
	tryGetPhoto();
}, 500);
</script>
</html>