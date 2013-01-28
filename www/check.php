<?php

session_start();
$filename = 'photos/'.session_id().'.jpg';
if(!file_exists($filename)) {
	echo -1;
} else {
	echo filemtime($filename);
}

?>