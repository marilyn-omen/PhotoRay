<?php
require('./phpqrcode/qrlib.php');
session_start();
if(file_exists('photos/'.session_id().'.jpg')) {
	unlink('photos/'.session_id().'.jpg');
}
QRcode::png(session_id(), 'qr/'.session_id().'.png', 'L', 16, 1);
?>
<html>
<script type="text/javascript" src="http://code.jquery.com/jquery-1.8.3.min.js"></script>
<body style="background: black; color: white; font-family: segoe ui; text-align: center; padding: 0; margin: 0;">
<div id="qr">
	<h1>PHOTOSIGHT</h1>
	<br /><br />
	<img id="qrimg" src="qr/<?php echo session_id() ?>.png" />
	<br />
</div>
<img id="photo" style="height: 100%; display: none;" />
</body>
<script type="text/javascript">
var prevDate = -1;
function tryGetPhoto() {
$.ajax({ 
		url: 'check.php',
	    processData: false,
	    success: function(result) {
	    	if(result == -1) {
	    		$("#photo").hide();
	    		$("#qr").show();
	    	} else if(result != prevDate) {
				$("#photo").attr("src", "photos/<?php echo session_id() ?>.jpg?" + new Date().getTime());
				$("#photo").show();
				$("#qr").hide();
			}
	    }
	});
window.setTimeout(function() {
	tryGetPhoto();
}, 500);
};
$("#qrimg").load(function() {
	$.ajax({ 
			url: 'delete.php',
		    processData: false
		});
	}
);
$("#photo").hide();
window.setTimeout(function() {
	tryGetPhoto();
}, 500);
</script>
</html>