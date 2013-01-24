<?php

$sid = $_POST['sid'];
if($sid != '') {
	session_id($sid);
	session_start();
	$target_path = 'photos/'.session_id().'.jpg';
	if(move_uploaded_file($_FILES['uploadedfile']['tmp_name'], $target_path)) {
	    echo "The file ".basename($_FILES['uploadedfile']['name'])." has been uploaded";
	} else{
	    echo "There was an error uploading the file, please try again!";
	}
}
else {
	echo "Missing session Id";
}

?>