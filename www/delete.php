<?php

session_start();
$filename = 'qr/'.session_id().'.png';
if(file_exists($filename)) {
	unlink($filename);
}

?>